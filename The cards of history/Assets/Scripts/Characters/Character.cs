using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "SO/Character")]
public class Character : ScriptableObject
{
    //Character stats


    public int characterID;
    public string characterName;
    public _Class _class;
    public int health;
    public int speed;
    public int dmg_buff;
    public int def_buff;
    public int supp_buff;
    public int skillPoints;
    public Material mat;

    public bool filled;
    public enum _Class
    {
        Agile,
        Gunner,
        Tank,
        Psychic
    }
}
