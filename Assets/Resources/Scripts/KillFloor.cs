using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFloor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Reset Ball
        if(collision.transform.tag == "Ball")
        {
            collision.transform.parent.GetComponent<PlayerController>().ResetBall();
        }
    }
}
