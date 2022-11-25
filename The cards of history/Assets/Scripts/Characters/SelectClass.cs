using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectClass : MonoBehaviour
{
    public CharacterMakerManager chMan;
    public void ClassSelector(int i)
    {
        switch (i)
        {
            case 0:
                chMan.copyChar._class = Character._Class.Agile;
                break;
            case 1:
                chMan.copyChar._class = Character._Class.Gunner;
                break;
            case 2:
                chMan.copyChar._class = Character._Class.Tank;
                break;
            case 3:
                chMan.copyChar._class = Character._Class.Psychic;
                break;
            default:
                break;
        }
        chMan.setClass();
    }

}
