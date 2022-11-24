using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStats : MonoBehaviour
{

    //To get the position of the tile
    public Vector3 _ID;
    public Material defaultMaterial, material;

    public void Start()
    {
        material = GetComponent<Material>();
    }
}
