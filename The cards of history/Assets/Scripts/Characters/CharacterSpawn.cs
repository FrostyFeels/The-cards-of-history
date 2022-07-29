using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawn : MonoBehaviour
{
    [SerializeField] public GameObject[] _allyCharacters;
    public void setCharacters(TileStats[] spawns, int tilesize)
    {
        Debug.Log("runs");
        for (int i = 0; i < _allyCharacters.Length; i++)
        {
            _allyCharacters[i].transform.position = spawns[i].transform.position + new Vector3(0, 1 * tilesize - .5f, 0);
        }
    }
}
