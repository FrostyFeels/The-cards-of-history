using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSaveAttack : MonoBehaviour
{
    [SerializeField] private CharacterManager main;

    [SerializeField] private TurnManager manager;

    public Renderer[] directionBlocks;
    public GameObject directionHolder;

    public Color defaultColor;

    public CharacterInfo character;

    public bool choosingDirection;
    public Vector2 dir;



    // Update is called once per frame
    void Update()
    {
        if(choosingDirection)
        {
            ChooseDirection();
        }
    }

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
