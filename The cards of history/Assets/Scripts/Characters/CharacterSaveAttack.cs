using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSaveAttack : MonoBehaviour
{
    [SerializeField] private CharacterManager main;

    [SerializeField] private TurnManager manager;

    [SerializeField] private Renderer[] directionBlocks;
    private GameObject directionHolder;

    [SerializeField] private Color defaultColor;

    public CharacterInfo character;

    private bool choosingDirection;
    private Vector2 dir;



    // Update is called once per frame
    void Update()
    {
        if(choosingDirection)
        {
            ChooseDirection();
        }
    }

    //TODO: Here will be the script that holds all the information for saving the attacks

    public void NewSelect()
    {
        if(defaultColor == null)
        {
            defaultColor = directionBlocks[0].GetComponent<Material>().color;
        }

        foreach (Renderer _direction in directionBlocks)
        {
            _direction.material.color = defaultColor;
            directionHolder.SetActive(false);
        }

        directionHolder = character.gameObject.transform.GetChild(0).gameObject;

        directionBlocks = directionHolder.GetComponentsInChildren<Renderer>();
        directionHolder.gameObject.SetActive(true);


    }

    public void ChooseDirection()
    {

    }

    public void SaveTheAttack()
    {
        AttackTurn attack = new AttackTurn(character, dir);
    }
}
