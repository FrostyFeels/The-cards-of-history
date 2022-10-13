using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "SO/Character")]
public class Character : ScriptableObject
{
    public int characterID;
    public string characterName;
    public string characterDescription;
    public int health;
    public int speed;
    public int dmg;
    public int def;

    public Material mat;
}
