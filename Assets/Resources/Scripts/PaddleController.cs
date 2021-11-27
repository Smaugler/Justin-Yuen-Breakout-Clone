using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody BallBody, PaddleBody;

    [SerializeField]
    private BoxCollider PaddleCollider;

    [SerializeField]
    private GameObject Ball, Paddle;

    [SerializeField]
    [Range(0.0f, 20.0f)]
    private float MovementSpeed = 5.0f;

    [SerializeField]
    [Range(0.0f, 1000.0f)]
    private float LaunchStrength = 500.0f;

    [SerializeField]
    [Range(0.0f, 135.0f)]
    private float LaunchAngleRange = 90.0f;

    [SerializeField]
    [Range(0.0f, 0.75f)]
    private float SmoothMovement = 0.1f;

    private Vector3 v3Velocity;
    private Vector3 v3RefVelocity = Vector3.zero;
    private bool BallLaunched = false;

    private void Awake()
    {
        Paddle.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

    private void Update()
    {
        // Reset Key
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBall();
        }
    }

    private void FixedUpdate()
    {
        MovePaddle();
        UpdateBall();
    }

    /// <summary>
    /// Update Paddle Movements
    /// </summary>
    private void MovePaddle()
    {
        float MovementInput = 0.0f;

        // Move Paddle Left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            MovementInput -= 1.0f;
        }

        // Move Paddle Right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            MovementInput += 1.0f;
        }

        // Move paddle with smooth movement
        //v3Velocity = Vector2.SmoothDamp(v2Velocity, new Vector3(MovementInput, 0.0f, 0.0f) * MovementSpeed, ref v2RefVelocity, SmoothMovement);

        //Vector2.SmoothDamp(v2Velocity, new Vector3(MovementInput, 0.0f, 0.0f) * MovementSpeed, ref v2RefVelocity, SmoothMovement);
        v3Velocity = Vector3.SmoothDamp(v3Velocity, new Vector3(MovementInput, 0.0f, 0.0f) * MovementSpeed, ref v3RefVelocity, SmoothMovement);

        Vector3 ScreenToWorld = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));

        // Check if Paddle is going off screen
        if (Paddle.transform.position.x + v3Velocity.x + (Paddle.transform.localScale.x * PaddleCollider.size.x * 0.5f) > (ScreenToWorld.x) ||
            Paddle.transform.position.x + v3Velocity.x - (Paddle.transform.localScale.x * PaddleCollider.size.x * 0.5f) < (-ScreenToWorld.x))
        {
            v3Velocity = Vector3.zero;
        }
        else
        {
            Paddle.transform.position = Paddle.transform.position + new Vector3(v3Velocity.x, v3Velocity.y, 0.0f);
        }
    }

    /// <summary>
    /// Update Ball States
    /// </summary>
    private void UpdateBall()
    {
        if (!BallLaunched)
        {
            Ball.transform.position = Vector3.Lerp(Ball.transform.position, new Vector3(Paddle.transform.position.x, Paddle.transform.position.y + 1.0f, 0.0f), Time.deltaTime * 10.0f);
        }

        if (!BallLaunched && Input.GetKey(KeyCode.Space))
        {
            BallLaunched = true;

            float RandomAngle = Random.Range(0.0f - (LaunchAngleRange * 0.5f), 0 + (LaunchAngleRange * 0.5f));

            float xcomponent = Mathf.Cos(RandomAngle * Mathf.PI / 180) * LaunchStrength;
            float ycomponent = Mathf.Sin(RandomAngle * Mathf.PI / 180) * LaunchStrength;

            BallBody.AddForce(ycomponent, xcomponent, 0);
        }
    }

    /// <summary>
    /// Reset Ball State
    /// </summary>
    /// <param name="_bResetPaddle">Reset Paddle State</param>
    public void ResetBall()
    {
        // Reset Ball velocity, position and launch state
        BallLaunched = false;
        BallBody.velocity = Vector3.zero;
        Ball.transform.position = new Vector3(Paddle.transform.position.x, Paddle.transform.position.y + 1.0f, 0.0f);
    }

    private void OnDrawGizmosSelected()
    {
        // Get the x and y component of the minimum angle of launch
        float xMinComponent = Mathf.Cos((((LaunchAngleRange + 180.0f) * 0.5f)) * Mathf.PI / 180) * 2.0f;
        float yMinComponent = Mathf.Sin((((LaunchAngleRange + 180.0f) * 0.5f)) * Mathf.PI / 180) * 2.0f;

        // Get the x and y component of the maximum angle of launch
        float xMaxComponent = Mathf.Cos((0.0f - ((LaunchAngleRange - 180.0f) * 0.5f)) * Mathf.PI / 180) * 2.0f;
        float yMaxComponent = Mathf.Sin((0.0f - ((LaunchAngleRange - 180.0f) * 0.5f)) * Mathf.PI / 180) * 2.0f;

        // Draw lines
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Ball.transform.position, Ball.transform.position + (new Vector3(xMinComponent, yMinComponent, 0.0f)));
        Gizmos.DrawLine(Ball.transform.position, Ball.transform.position + (new Vector3(xMaxComponent, yMaxComponent, 0.0f)));
    }
}
