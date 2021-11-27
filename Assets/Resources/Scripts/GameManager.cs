 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class GameManager : NetworkBehaviour
{
    // Singleton reference
    private static GameManager GameManagerSingleton = null;
    public static GameManager GetGameManager() { return GameManagerSingleton; }

    [SerializeField][Range(1, 100)]
    private int iBrickRows = 5, iBrickColumns = 10;

    [SerializeField][SyncVar]
    private List<Color> lLayerColors = new List<Color>();
    public List<Color> GetLayerColor() { return lLayerColors; }

    [SerializeField]
    private GameObject BrickPrefab;

    [SerializeField]
    private GameObject BrickParent;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SyncVar(hook = nameof(UpdateScore))]
    private int iScore = 0;

    [SyncVar]
    private int iBrickCount = 0;
    public int GetBrickCount() { return iBrickCount; }

    private void Awake()
    {
        if (GameManagerSingleton == null)
            GameManagerSingleton = this;
        else if (GameManagerSingleton != this)
            Destroy(gameObject);

        // Set score text
        if (ScoreText)
        {
            ScoreText.text = "Score: " + iScore.ToString();
        }
    }

    /// <summary>
    /// Reset Bricks
    /// </summary>
    /// <param name="_bRandomizeLayerColors">Override And Forcefully Randomize Layer Colors</param>
    public void ResetBricks(bool _bRandomizeLayerColors = false)
    {
        // Reset bricks if this is server
        if (!isServer)
        {
            return;
        }

        Vector3 v3ScreenToWorld = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));

        // Get brick size based on screen size and layers and columns to spawn
        float fBrickWidth = v3ScreenToWorld.x / (iBrickColumns * 0.5f);
        float fBrickHeight = v3ScreenToWorld.y / iBrickRows;

        // Destroy any existing bricks
        foreach (Transform _Child in BrickParent.transform)
        {
            Destroy(_Child.gameObject);
        }

        // Randomize colors for brick layers
        if(_bRandomizeLayerColors)
        {
            lLayerColors.Clear();

            // Add random color for each row
            for (int i = 0; i < iBrickRows; i++)
            {
                lLayerColors.Add(new Color(Random.Range(0.5f, 0.85f), Random.Range(0.5f, 0.85f), Random.Range(0.5f, 0.85f)));
            }
        }

        // Reset brick count
        iBrickCount = 0;

        // Run through all rows/layers
        for (int i = 0; i < iBrickRows; i++)
        {
            // If this row/layer doesn't have a color assigned
            if (i > lLayerColors.Count - 1)
            {
                lLayerColors.Add(new Color(Random.Range(0.5f, 0.85f), Random.Range(0.5f, 0.85f), Random.Range(0.5f, 0.85f)));
            }

            for (int j = 0; j < iBrickColumns; j++)
            {
                // Instantiate brick
                GameObject BrickObject = Instantiate(BrickPrefab, BrickParent.transform);

                BrickObject.GetComponent<Brick>().SetLayer(i);

                // Set size and position
                BrickObject.transform.localScale = new Vector3(fBrickWidth - 0.05f, fBrickHeight - 0.05f, 1.0f);
                BrickObject.transform.localPosition = new Vector3((j * fBrickWidth) - v3ScreenToWorld.x + (fBrickWidth * 0.5f), (i * fBrickHeight) + (fBrickHeight * 0.5f), 0.0f);

                // Spawn object in network
                NetworkServer.Spawn(BrickObject);

                iBrickCount++;
            }
        }
    }

    /// <summary>
    /// Score Hook That Updates Score Text On Change
    /// </summary>
    /// <param name="_iOldScore">The Previous Score Value</param>
    /// <param name="_iNewScore">The New Score Value</param>
    void UpdateScore(int _iOldScore, int _iNewScore)
    {
        // Update Score
        if (ScoreText)
        {
            ScoreText.text = "Score: " + _iNewScore.ToString();
        }
    }

    /// <summary>
    /// Brick Is Destroyed
    /// </summary>
    public void DestroyedBrick(GameObject _Brick = null)
    {
        // Only destroy brick if this is server
        if(!isServer)
        {
            return;
        }

        // Increase score and decrease brick count
        iScore += 100;
        iBrickCount -= 1;

        // Destroy brick object
        if(_Brick)
        {
            Destroy(_Brick);
        }

        // Check if all bricks are destroyed
        if (iBrickCount <= 0)
        {
            ResetBricks();
        }
    }
}