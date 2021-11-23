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
    private GameObject BrickPrefab, PlayerPrefab;

    [SerializeField]
    private Shader BrickShader;

    [SerializeField]
    private GameObject BrickParent;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    private PlayerController Player;
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
        ResetPlayer();
    }

    /// <summary>
    /// Reset Bricks
    /// </summary>
    /// <param name="_bRandomizeLayerColors">Override And Forcefully Randomize Layer Colors</param>
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

        // Randomize colors for brick layers
        if(_bRandomizeLayerColors)
        {
            lLayerColors.Clear();

            // Add random color for each row
            for (int i = 0; i < BrickRows; i++)
            {
                lLayerColors.Add(new Color(Random.Range(0.5f, 0.85f), Random.Range(0.5f, 0.85f), Random.Range(0.5f, 0.85f)));
            }
        }

        BrickCount = 0;

        // Run through all rows/layers
        for (int i = 0; i < BrickRows; i++)
        {
            // If this row/layer doesn't have a color assigned
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

    /// <summary>
    /// Reset Player States
    /// </summary>
    /// <param name="_bResetPaddle">Reset Player's Paddle</param>
    public void ResetPlayer(bool _bResetPaddle = false)
    {
        Vector3 ScreenToWorld = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));

        // If there's no reference to player controller
        if(!Player)
        {
            // Instantiate player and get reference
            GameObject PlayerObject = Instantiate(PlayerPrefab, new Vector3(0.0f, -ScreenToWorld.y + 0.5f, 0.0f), Quaternion.identity);
            Player = PlayerObject.GetComponent<PlayerController>();
        }

        // Reset player ball and paddle state
        Player.ResetBall(_bResetPaddle);
    }

    private void Update()
    {
        // Reset Key
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetBricks(true);
            ResetPlayer(true);
        }
    }

    /// <summary>
    /// Brick Is Destroyed
    /// </summary>
    public void DestroyedBrick()
    {
        // Increase score and decrease brick count
        Score += 100;
        BrickCount -= 1;

        // Update Score
        if(ScoreText)
        {
            ScoreText.text = "Score: " + Score.ToString();
        }

        // Check if all bricks are destroyed
        if (BrickCount <= 0)
        {
            ResetBricks(true);
        }
    }
}
