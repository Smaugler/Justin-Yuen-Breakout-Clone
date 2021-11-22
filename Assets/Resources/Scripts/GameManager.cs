 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    private Vector3 Temp;

    private void Awake()
    {
        Temp = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));

        float BrickWidth = Temp.x / (BrickColumns * 0.5f);
        float BrickHeight = Temp.y / BrickRows ;

        BrickParent.transform.position = new Vector3(0.0f, Temp.y, 0.0f);

        for (int i = 0; i < BrickRows; i++)
        {
            if(i > lLayerColors.Count - 1)
            {
                lLayerColors.Add(new Color(Random.Range(0.5f, 0.85f), Random.Range(0.5f, 0.85f), Random.Range(0.5f, 0.85f)));
            }

            for (int j = 0; j < BrickColumns; j++)
            {
                GameObject BrickObject = Instantiate(BrickPrefab, BrickParent.transform);
                MeshRenderer BrickRenderer = BrickObject.GetComponent<MeshRenderer>();
                BrickObject.transform.localScale = new Vector3(BrickWidth - 0.15f, BrickHeight - 0.15f, 1.0f);
                BrickObject.transform.localPosition = new Vector3((j * BrickWidth) - Temp.x + (BrickWidth * 0.5f), (i * BrickHeight) - Temp.y + (BrickHeight * 0.5f), 0.0f);

                BrickRenderer.material = new Material(BrickShader);
                BrickRenderer.material.color = lLayerColors[i];
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
