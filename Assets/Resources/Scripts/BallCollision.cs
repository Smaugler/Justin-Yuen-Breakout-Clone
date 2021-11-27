using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    private void Awake()
    {
        Physics.IgnoreLayerCollision(3, 3);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if object is a brick
        if(collision.transform.tag == "Brick")
        {
            // Destroy brick and gain score
            GameManager.GetGameManager().DestroyedBrick(collision.gameObject);
        }
    }
}
