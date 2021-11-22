 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton reference
    private static GameManager GameManagerSingleton = null;
    public static GameManager GetGameManager() { return GameManagerSingleton; }

    [SerializeField][Range(1, 100)]
    private int BrickRows = 5, BrickColumns = 10;

    [SerializeField]
    private List<Color> lLayerColors = new List<Color>();

    [SerializeField]
    private GameObject BrickPrefab;

    [SerializeField]
    private Shader BrickShader;

    [SerializeField]
    private GameObject BrickParent;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    private int Score = 0;
    private int BrickCount = 0;

    private void Awake()
    {
        if (GameManagerSingleton == null)
            GameManagerSingleton = this;
        else if (GameManagerSingleton != this)
            Destroy(gameObject);

        if (ScoreText)
        {
            ScoreText.text = "Score: " + Score.ToString();
        }

        ResetBricks();
    }

    public void ResetBricks(bool _bRandomizeLayerColors = false)
    {
        Vector3 ScreenToWorld = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));

        // Get brick size based on screen size and layers and columns to spawn
        float BrickWidth = ScreenToWorld.x / (BrickColumns * 0.5f);
        float BrickHeight = ScreenToWorld.y / BrickRows;

        // Set brick parent to the top of the screen view
        BrickParent.transform.position = new Vector3(0.0f, ScreenToWorld.y, 0.0f);

        // Destroy any existing bricks
        foreach (Transform child in BrickParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if(_bRandomizeLayerColors)
        {
            lLayerColors.Clear();

            for (int i = 0; i < BrickRows; i++)
            {
                lLayerColors.Add(new Color(Random.Range(0.5f, 0.85f), Random.Range(0.5f, 0.85f), Random.Range(0.5f, 0.85f)));
            }
        }

        BrickCount = 0;

        // Run through all rows/layers
        for (int i = 0; i < BrickRows; i++)
        {
            // If this row/layer doesn't have a color assigned to the bricks
            if (i > lLayerColors.Count - 1)
            {
                lLayerColors.Add(new Color(Random.Range(0.5f, 0.85f), Random.Range(0.5f, 0.85f), Random.Range(0.5f, 0.85f)));
            }

            // Create new material with color of layer
            Material LayerMaterial = new Material(BrickShader);
            LayerMaterial.color = lLayerColors[i];

            for (int j = 0; j < BrickColumns; j++)
            {
                // Instantiate brick
                GameObject BrickObject = Instantiate(BrickPrefab, BrickParent.transform);
                MeshRenderer BrickRenderer = BrickObject.GetComponent<MeshRenderer>();

                // Set size and position
                BrickObject.transform.localScale = new Vector3(BrickWidth - 0.05f, BrickHeight - 0.05f, 1.0f);
                BrickObject.transform.localPosition = new Vector3((j * BrickWidth) - ScreenToWorld.x + (BrickWidth * 0.5f), (i * BrickHeight) - ScreenToWorld.y + (BrickHeight * 0.5f), 0.0f);

                // Set material
                BrickRenderer.material = LayerMaterial;

                BrickCount++;
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetBricks(true);
        }
    }

    public void DestroyedBrick()
    {
        Score += 100;
        BrickCount -= 1;

        if(ScoreText)
        {
            ScoreText.text = "Score: " + Score.ToString();
        }

        if (BrickCount <= 0)
        {
            ResetBricks(true);
        }
    }
}
