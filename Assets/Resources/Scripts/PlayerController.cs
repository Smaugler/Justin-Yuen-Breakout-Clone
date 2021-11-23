using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody BallBody, PaddleBody;

    [SerializeField]
    private GameObject Ball, Paddle;

    [SerializeField][Range(0.0f, 20.0f)]
    private float MovementSpeed = 5.0f;

    [SerializeField]
    [Range(0.0f, 1000.0f)]
    private float LaunchStrength = 500.0f;

    [SerializeField]
    [Range(0.0f, 135.0f)]
    private float LaunchAngleRange = 90.0f;

    [SerializeField][Range(0.0f, 0.75f)]
    private float SmoothMovement = 0.1f;

    private bool BallLaunched = false;

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
        Vector2 v2RefVelocity = Vector2.zero;
        PaddleBody.velocity = Vector2.SmoothDamp(PaddleBody.velocity, new Vector3(MovementInput, 0.0f, 0.0f) * MovementSpeed, ref v2RefVelocity, SmoothMovement);
    }

    /// <summary>
    /// Update Ball States
    /// </summary>
    private void UpdateBall()
    {
        if(!BallLaunched)
        {
            Ball.transform.position = Vector3.Lerp(Ball.transform.position, new Vector3(Paddle.transform.position.x, Paddle.transform.position.y + 1.0f, 0.0f), Time.deltaTime * 10.0f);
        }

        if(!BallLaunched && Input.GetKey(KeyCode.Space))
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
    public void ResetBall(bool _bResetPaddle = false)
    {
        // Resets Paddle velocity and position
        if(_bResetPaddle)
        {
            PaddleBody.velocity = Vector3.zero;
            Paddle.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }

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