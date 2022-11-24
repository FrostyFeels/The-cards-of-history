using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    //Keeps track of position the character number and how many moves it can do
    public Vector3Int pos;
    public int _Index;
    public int _Moves;

    //Used to set the position to the grid layout
    public void SetPos(Vector3 _pos)
    {
        pos = new Vector3Int((int)_pos.x, (int)_pos.y, (int)_pos.z);
    }

}
