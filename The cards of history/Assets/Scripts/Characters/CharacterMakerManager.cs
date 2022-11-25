using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterMakerManager : MonoBehaviour
{
    //Number of characters on the screen
    [SerializeField] private int maxCharacters = 25;

    [Header("Characters")]
    public Character curChar;
    public Character copyChar;

    [Header("Prefabs")]
    [SerializeField] private GameObject filledSlot;
    [SerializeField] private GameObject emptySlot;

    [SerializeField] private GameObject contentHolder;
    public List<Character> characters = new List<Character>();



    public GameObject charSelectPanel;

    [Header("CharacteStats")]
    public GameObject charEditorPanel;
    public TextMeshProUGUI dmg_buff;
    public TextMeshProUGUI def_buff;
    public TextMeshProUGUI supp_buff;
    public TextMeshProUGUI speed;
    public TMP_InputField charName;
    public TextMeshProUGUI skillpoints;

    [SerializeField] private Image agile;
    [SerializeField] private Image gunner;
    [SerializeField] private Image tank;
    [SerializeField] private Image psychic;


    public FillCharacterSlot fillSlot;

    void Start()
    {
        object[] objectCharacters = Resources.LoadAll("Characters");
        foreach (Character _character in objectCharacters)
        {
            characters.Add(_character);
        }

        characters.Sort(SortByName);



        fillSlot.FillScrollView(contentHolder.transform, characters.ToArray(), maxCharacters, true);
    }
    
    //Sets the texts once you selected a character
    public void SetTexts()
    {
        if (!curChar.filled)
            curChar.characterName = "";

        dmg_buff.text = curChar.dmg_buff.ToString();
        def_buff.text = curChar.def_buff.ToString();
        supp_buff.text = curChar.supp_buff.ToString();
        speed.text = curChar.speed.ToString();
        charName.text = curChar.characterName.ToString();
        skillpoints.text = skillpoints.text + curChar.skillPoints.ToString();

        if(curChar.filled)
            setClass();
    }

    //Sets the class if the character was a filled slot or when called when a class is selected
    public void setClass()
    {
        switch (copyChar._class)
        {
            case Character._Class.Agile:
                agile.enabled = true;
                gunner.enabled = false;
                psychic.enabled = false;
                tank.enabled = false;
                break;
            case Character._Class.Gunner:
                gunner.enabled = true;
                agile.enabled = false;
                tank.enabled = false;
                psychic.enabled = false;
                break;
            case Character._Class.Tank:
                tank.enabled = true;
                agile.enabled = false;
                psychic.enabled = false;
                gunner.enabled = false;
                break;
            case Character._Class.Psychic:
                psychic.enabled = true;
                gunner.enabled = false;
                tank.enabled = false;
                agile.enabled = false;
                break;
        }
    }

    //Saves the characters stats
    public void Save()
    {
        curChar.name = charName.text;
        curChar.characterName = charName.text;
        curChar.speed = copyChar.speed;
        curChar.dmg_buff = copyChar.dmg_buff;
        curChar.supp_buff = copyChar.supp_buff;
        curChar.def_buff = copyChar.def_buff;
        curChar._class = copyChar._class;
        curChar.filled = true;

        curChar.SetDirty();
        OnSaveOrDelete();
    }

    //Deletes the characters stats resetting it
    public void Delete()
    {
        curChar.characterName = "zzzz";
        curChar.speed = 0;
        curChar.dmg_buff = 0;
        curChar.supp_buff = 0;
        curChar.def_buff = 0;
        curChar.skillPoints = 5;
        curChar.filled = false;

        curChar.SetDirty();
        OnSaveOrDelete();
    }

    //Deletes all characers
    public void resetall()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            curChar = characters[i];
            Delete();
        }

        fillSlot.DeleteScrollView(contentHolder.transform);

        characters.Sort(SortByName);

        fillSlot.FillScrollView(contentHolder.transform, characters.ToArray(), maxCharacters, true);
    }

    //This is called when a change is made so that the content view is refilled
    public void OnSaveOrDelete()
    {
        charEditorPanel.SetActive(false);
        charSelectPanel.SetActive(true);

        fillSlot.DeleteScrollView(contentHolder.transform);

        characters.Sort(SortByName);

        fillSlot.FillScrollView(contentHolder.transform, characters.ToArray(), maxCharacters, true);
    }

    //This sorts the content view by name
    static int SortByName(Character c1, Character c2)
    {
        return c1.characterName.CompareTo(c2.characterName);
    }

}
