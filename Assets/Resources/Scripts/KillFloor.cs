using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFloor : MonoBehaviour
{
    private void OnCollisionEnter(Collision _Collision)
    {
        // Reset Ball
        if(_Collision.transform.tag == "Ball")
        {
            _Collision.transform.parent.GetComponent<PaddleController>().LocalResetBall();
        }
    }
}
