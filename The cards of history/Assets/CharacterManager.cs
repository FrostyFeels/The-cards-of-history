using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject _CurCharacter;
    [SerializeField] private LayerMask charMask;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectChar();
            Debug.Log("OwO2");
        }
            
    }

    public void SelectChar()
    {
        Debug.Log("OwO3");
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200 , charMask))
        {
            Debug.Log("OwO");
            if(hit.collider.CompareTag("Player"))
            {
                _CurCharacter = hit.collider.gameObject;
            }
        }
    }

}
