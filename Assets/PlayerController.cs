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

    [SerializeField][Range(0.0f, 0.75f)]
    private float SmoothMovement = 0.1f;

    private bool BallLaunched = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        UpdatePaddle();
        UpdateBall();
    }

    private void UpdatePaddle()
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

    private void UpdateBall()
    {
        if(!BallLaunched)
        {
            Ball.transform.position = Vector3.Lerp(Ball.transform.position, new Vector3(Paddle.transform.position.x, Paddle.transform.position.y + 1.0f, 0.0f), Time.deltaTime * 10.0f);
        }

        if(!BallLaunched && Input.GetKey(KeyCode.Space))
        {
            BallLaunched = true;

            float RandomAngle = Random.Range(0.0f - (90.0f * 0.5f), 0 + (90.0f * 0.5f));

            float xcomponent = Mathf.Cos(RandomAngle * Mathf.PI / 180) * LaunchStrength;
            float ycomponent = Mathf.Sin(RandomAngle * Mathf.PI / 180) * LaunchStrength;

            BallBody.AddForce(ycomponent, xcomponent, 0);
        }
    }

    public void ResetBall()
    {
        BallLaunched = false;
        Ball.transform.position = new Vector3(Paddle.transform.position.x, Paddle.transform.position.y + 1.0f, 0.0f);
        BallBody.velocity = Vector3.zero;
    }
}