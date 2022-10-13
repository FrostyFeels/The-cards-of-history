using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MapSpawnSelect : MonoBehaviour
{
    public int characterIndex;
    public TileStats[] playerSpots = new TileStats[5];
    public TileStats[] enemySpots = new TileStats[5];

    [SerializeField] private MapGen mapGen;
    [SerializeField] private bool ally;

    [SerializeField] private TextMeshProUGUI characterText;

    private void Start()
    {
        mapGen = GameObject.Find("MapGenerator").GetComponent<MapGen>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            characterIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            characterIndex = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            characterIndex = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            characterIndex = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            characterIndex = 5;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (ally)
                SetPlayerSpawns();
            else
                SetEnemySpawns();
        }
    }

    public void SetPlayerSpawns()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.collider;
            var renderer = selection.GetComponent<Renderer>();

            renderer.material = mapGen.playerSpotMat;

            if (playerSpots[characterIndex - 1] != null)
            {
                playerSpots[characterIndex - 1].gameObject.GetComponentInChildren<TextMeshPro>().enabled = false;

                TileStats tile = playerSpots[characterIndex - 1];
                MaterialManager.SetMaterial(playerSpots[characterIndex - 1].gameObject.GetComponent<Renderer>(), mapGen.map[(int)tile._ID.y].map[(int)tile._ID.x + ((int)tile._ID.z * mapGen.map[(int)tile._ID.y].gridSizeX)]._materialID);


            }
            playerSpots[characterIndex - 1] = selection.GetComponent<TileStats>();
            playerSpots[characterIndex - 1].gameObject.GetComponentInChildren<TextMeshPro>().text = characterIndex.ToString();
            playerSpots[characterIndex - 1].gameObject.GetComponentInChildren<TextMeshPro>().enabled = true;
        }
    }
    public void SetEnemySpawns()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.collider;
            var renderer = selection.GetComponent<Renderer>();

            renderer.material = mapGen.enemySpotMat;

            if (enemySpots[characterIndex - 1] != null)
            {
                enemySpots[characterIndex - 1].gameObject.GetComponentInChildren<TextMeshPro>().enabled = false;

                TileStats tile = enemySpots[characterIndex - 1];
                MaterialManager.SetMaterial(enemySpots[characterIndex - 1].gameObject.GetComponent<Renderer>(), mapGen.map[(int)tile._ID.y].map[(int)tile._ID.x + ((int)tile._ID.z * mapGen.map[(int)tile._ID.y].gridSizeX)]._materialID);


            }
            enemySpots[characterIndex - 1] = selection.GetComponent<TileStats>();
            enemySpots[characterIndex - 1].gameObject.GetComponentInChildren<TextMeshPro>().text = characterIndex.ToString();
            enemySpots[characterIndex - 1].gameObject.GetComponentInChildren<TextMeshPro>().enabled = true;
        }
    }
    public void ChangeAllySelect()
    {
        ally = !ally;

        if (ally)
            characterText.text = "Ally";
        else
            characterText.text = "Enemy";
    }
}
