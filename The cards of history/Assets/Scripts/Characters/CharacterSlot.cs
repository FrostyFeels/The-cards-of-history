using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI className;


    public TextMeshProUGUI health;
    public TextMeshProUGUI dmg;
    public TextMeshProUGUI def;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI supp;

    public GameObject stats;

    public Character _Character;
    public CharacterMakerManager charMaker;

    public void Start()
    {
        
    }

    //Makes the player see the stats of a card
    public void OnPointerEnter(PointerEventData eventData)
    {
        print("onmouseEnter");
        if (stats != null)
            stats.SetActive(true);

    }

    //Makes the player stop seeing the stats of a card
    public void OnPointerExit(PointerEventData eventData)
    {
        print("OnMouseExit");
        if (stats != null)
            stats.SetActive(false);
    }

    public void FillStats()
    {
        name.text = _Character.characterName;
        className.text = _Character._class.ToString();

        dmg.text = dmg.text + _Character.dmg_buff.ToString();
        health.text = health.text + _Character.health.ToString();
        speed.text = speed.text + _Character.speed.ToString();
        def.text = def.text + _Character.def_buff.ToString();
        supp.text = supp.text + _Character.supp_buff.ToString();
    }

    //Makes a copy then turns on the right UI and sends characterManager to do the text
    public void SendCharacterData()
    {
        charMaker = GameObject.Find("Character panel").GetComponent<CharacterMakerManager>();

        makeCopy();

        charMaker.curChar = _Character;
       
        charMaker.charEditorPanel.SetActive(true);
        charMaker.charSelectPanel.SetActive(false);

        charMaker.SetTexts();
    }

    //Makes a copy so that it only saves when you press the button
    public void makeCopy()
    {
        Character character = new Character();
        character.def_buff = _Character.def_buff;
        character.dmg_buff = _Character.dmg_buff;
        character.supp_buff = _Character.supp_buff;
        character.speed = _Character.speed;
        character.skillPoints = _Character.skillPoints;

        charMaker.copyChar = character;
    }

}
