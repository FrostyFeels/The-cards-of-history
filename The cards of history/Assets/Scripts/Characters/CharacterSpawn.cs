using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawn : MonoBehaviour
{


    public GameObject[] _allyCharacters;
    public GameObject[] _EnemyCharacters;

    //TODO: Figure out what the formula is for making the characters spawn on the right spot instead off (1 * tilesize - .5f)
    //Just remember it makes the enemies not stick in the ground
    public void SetPlayers(TileStats[] spawns, int tilesize)
    {
        
        
        for (int i = 0; i < _allyCharacters.Length; i++)
        {
            _allyCharacters[i].transform.position = spawns[i].transform.position + new Vector3(0, 1 * tilesize - .5f, 0);
        }
    }

    public void SetEnemies(TileStats[] spawns, int tilesize)
    {
        for (int i = 0; i < _EnemyCharacters.Length; i++)
        {
            _EnemyCharacters[i].transform.position = spawns[i].transform.position + new Vector3(0, 1 * tilesize - .5f, 0);
        }
    }
}
