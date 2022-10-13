using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMakerManager : MonoBehaviour
{
    public int maxCharacters = 25;
    public GameObject filledSlot;
    public GameObject emptySlot;

    public GameObject contentHolder;
    public List<Character> characters = new List<Character>();
    
    void Start()
    {
        object[] objectCharacters = Resources.LoadAll("Characters");
        foreach (Character _character in objectCharacters)
        {
            characters.Add(_character);
        }

        FillScrollView();
    }

    public void FillScrollView()
    {
        for (int i = 0; i < maxCharacters; i++)
        {
            if (i < characters.Count)
            {
                GameObject newSlot = Instantiate(filledSlot, contentHolder.transform);
                CharacterSlot info = newSlot.GetComponent<CharacterSlot>();

                info.name.text = characters[i].characterName;
                info.className.text = characters[i].characterDescription;

                info.dmg.text = info.dmg.text + characters[i].dmg.ToString();
                info.health.text = info.health.text + characters[i].health.ToString();
                info.speed.text = info.speed.text + characters[i].speed.ToString();
                info.def.text = info.def.text + characters[i].def.ToString();
            }
            else
            {
                GameObject newSlot = Instantiate(emptySlot, contentHolder.transform);
            }
        }
    }


}
