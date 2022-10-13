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

    public GameObject stats;

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("onmouseEnter");
        if(stats != null)
            stats.SetActive(true);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("OnMouseExit");
        if(stats != null)
            stats.SetActive(false);
    }



}
