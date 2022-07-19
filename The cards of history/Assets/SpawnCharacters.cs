using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCharacters : MonoBehaviour
{
    public TileStats[] playerspots;
    public MapStats map;

    [SerializeField] private GameObject[] _FriendlyCharacters;

    public void Start()
    {
        playerspots = map.playerSpots;
       
        setCharacters();
    }

    public void setCharacters()
    {
        for (int i = 0; i < _FriendlyCharacters.Length; i++)
        {
            _FriendlyCharacters[i].transform.position = playerspots[i].transform.position + new Vector3(0, 1 * map.map[0].tileSize - .5f, 0);
        }
    }
}
