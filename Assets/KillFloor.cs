using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFloor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Ball")
        {
            collision.transform.parent.GetComponent<PlayerController>().ResetBall();
        }
    }
}
