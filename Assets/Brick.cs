using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Brick : NetworkBehaviour
{
    [SerializeField]
    private Shader BrickShader;

    [SerializeField][SyncVar]
    private int iLayer = 0;
    public void SetLayer(int _iLayerIndex) { iLayer = _iLayerIndex; }

    private void Start()
    {
        // Create new material with color of layer
        Material LayerMaterial = new Material(BrickShader);
        LayerMaterial.color = GameManager.GetGameManager().GetLayerColor()[iLayer];
        GetComponent<MeshRenderer>().material = LayerMaterial;
    }
}
