using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundry : MonoBehaviour
{
    [SerializeField]
    private BoxCollider Top, Bottom, Left, Right;

    private void Awake()
    {
        Vector3 ScreenToWorld = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));

        // Set Top Boundry
        Top.center = new Vector3(0.0f, ScreenToWorld.y + 0.5f, 0.0f);
        Top.size = new Vector3(ScreenToWorld.x * 2.0f, 1, 1);

        // Set Bottom Boundry
        Bottom.center = new Vector3(0.0f, -ScreenToWorld.y - 0.5f, 0.0f);
        Bottom.size = new Vector3(ScreenToWorld.x * 2.0f, 1, 1);

        // Set Left Boundry
        Left.center = new Vector3(-ScreenToWorld.x - 0.5f, 0.0f, 0.0f);
        Left.size = new Vector3(1, ScreenToWorld.y * 2.0f, 1);

        // Set Right Boundry
        Right.center = new Vector3(ScreenToWorld.x + 0.5f, 0.0f, 0.0f);
        Right.size = new Vector3(1, ScreenToWorld.y * 2.0f, 1);
    }
}
