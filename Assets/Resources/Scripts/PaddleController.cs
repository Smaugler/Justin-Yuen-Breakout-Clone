using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PaddleController : NetworkBehaviour
{
	[SerializeField]
	private Rigidbody BallBody;

	[SerializeField]
	private BoxCollider PaddleCollider;

	[SerializeField]
	private GameObject Ball, Paddle;

	[SerializeField]
	[Range(0.0f, 20.0f)]
	private float fMovementSpeed = 5.0f;

	[SerializeField]
	[Range(0.0f, 1000.0f)]
	private float fLaunchStrength = 500.0f;

	[SerializeField]
	[Range(0.0f, 135.0f)]
	private float fLaunchAngleRange = 90.0f;

	[SerializeField]
	[Range(0.0f, 0.75f)]
	private float fSmoothMovement = 0.1f;

	private Vector3 v3Velocity;
	private Vector3 v3RefVelocity = Vector3.zero;
	private bool bBallLaunched = false;

    private void Start()
    {
		Vector3 v3ScreenToWorld = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));

		Paddle.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

		// Set offset depending on client being host or not
		transform.position = new Vector3(0.0f, -v3ScreenToWorld.y + ((isServer) ? 0.25f : 2.25f), 0.0f);

		if(isServer)
        {
			GameManager.GetGameManager().ResetBricks();
        }
	}

    private void Update()
	{
		// Reset Key
		if (Input.GetKeyDown(KeyCode.R) && isLocalPlayer && bBallLaunched)
		{
			LocalResetBall();
		}
	}

	private void FixedUpdate()
	{
		if(isLocalPlayer)
		{
			MovePaddle();
			UpdateBall();
		}
	}

	/// <summary>
	/// Update Paddle Movements
	/// </summary>
	private void MovePaddle()
	{
		float fMovementInput = 0.0f;

		// Move Paddle Left
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			fMovementInput -= 1.0f;
		}

		// Move Paddle Right
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			fMovementInput += 1.0f;
		}

		// Move paddle with smooth movement
		v3Velocity = Vector3.SmoothDamp(v3Velocity, new Vector3(fMovementInput, 0.0f, 0.0f) * fMovementSpeed, ref v3RefVelocity, fSmoothMovement);

		Vector3 v3ScreenToWorld = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));

		// Check if Paddle is going off screen
		if (Paddle.transform.position.x + v3Velocity.x + (Paddle.transform.localScale.x * PaddleCollider.size.x * 0.5f) > (v3ScreenToWorld.x) ||
			Paddle.transform.position.x + v3Velocity.x - (Paddle.transform.localScale.x * PaddleCollider.size.x * 0.5f) < (-v3ScreenToWorld.x))
		{
			v3Velocity = Vector3.zero;
		}
		else
		{
			// Update paddle position
			Paddle.transform.position = Paddle.transform.position + new Vector3(v3Velocity.x, v3Velocity.y, 0.0f);
		}
	}

	/// <summary>
	/// Update Ball States
	/// </summary>
	private void UpdateBall()
	{
		// If Ball is already launched
		if(bBallLaunched)
        {
			return;
        }

		// Keep ball locked to paddle if not launched
		Ball.transform.position = Vector3.Lerp(Ball.transform.position, new Vector3(Paddle.transform.position.x, Paddle.transform.position.y + 1.0f, 0.0f), Time.deltaTime * 10.0f);

		// Launch ball
		if (Input.GetKey(KeyCode.Space))
		{
			bBallLaunched = true;

			// Random angle between given value
			float fRandomAngle = Random.Range(0.0f - (fLaunchAngleRange * 0.5f), 0 + (fLaunchAngleRange * 0.5f));

			// Get X & Y component of the direction with random angle
			float fXComponent = Mathf.Cos(fRandomAngle * Mathf.PI / 180) * fLaunchStrength;
			float fYcomponent = Mathf.Sin(fRandomAngle * Mathf.PI / 180) * fLaunchStrength;

			// Apply force to ball rigidbody
			BallBody.AddForce(fYcomponent, fXComponent, 0);
		}
	}

	/// <summary>
	/// Reset Ball In Local Network
	/// </summary>
	public void LocalResetBall()
	{
		if (!bBallLaunched)
		{
			return;
		}

		// Reset Ball velocity, position and launch state
		bBallLaunched = false;
		BallBody.velocity = Vector3.zero;
		Ball.transform.position = new Vector3(Paddle.transform.position.x, Paddle.transform.position.y + 1.0f, 0.0f);
	}

    private void OnPlayerDisconnected()
    {
		Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
	{
		// Get the x and y component of the minimum angle of launch
		float xMinComponent = Mathf.Cos((((fLaunchAngleRange + 180.0f) * 0.5f)) * Mathf.PI / 180) * 2.0f;
		float yMinComponent = Mathf.Sin((((fLaunchAngleRange + 180.0f) * 0.5f)) * Mathf.PI / 180) * 2.0f;

		// Get the x and y component of the maximum angle of launch
		float xMaxComponent = Mathf.Cos((0.0f - ((fLaunchAngleRange - 180.0f) * 0.5f)) * Mathf.PI / 180) * 2.0f;
		float yMaxComponent = Mathf.Sin((0.0f - ((fLaunchAngleRange - 180.0f) * 0.5f)) * Mathf.PI / 180) * 2.0f;

		// Draw lines
		Gizmos.color = Color.red;
		Gizmos.DrawLine(Ball.transform.position, Ball.transform.position + (new Vector3(xMinComponent, yMinComponent, 0.0f)));
		Gizmos.DrawLine(Ball.transform.position, Ball.transform.position + (new Vector3(xMaxComponent, yMaxComponent, 0.0f)));
	}
}
