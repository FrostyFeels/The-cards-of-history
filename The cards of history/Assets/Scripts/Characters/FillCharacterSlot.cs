using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillCharacterSlot : MonoBehaviour
{

    public GameObject filledSlot;
    public GameObject emptySlot;
    public CharacterMakerManager characterMakerManager;
    public void FillScrollView(Transform holder, Character[] characters, int characterLimit, bool characterMaker)
    {
        for (int i = 0; i < characterLimit; i++)
        {
            if (characters[i].filled)
            {
                GameObject newSlot = Instantiate(filledSlot, holder.transform);
                CharacterSlot info = newSlot.GetComponent<CharacterSlot>();
                info._Character = characters[i];

                if (characterMaker)
                    info.charMaker = characterMakerManager;
                info.FillStats();

            }
            else if (!characters[i].filled)
            {
                GameObject newSlot = Instantiate(emptySlot, holder.transform);
                CharacterSlot info = newSlot.GetComponent<CharacterSlot>();
                info._Character = characters[i];

                if (characterMaker)
                    info.charMaker = characterMakerManager;
            }

            
                
        }
    }

    public void DeleteScrollView(Transform holder)
    {
        foreach (Transform child in holder)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
