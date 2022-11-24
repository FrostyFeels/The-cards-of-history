using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public AllyMovement movement;
    public List<Turn> turns = new List<Turn>();
    public GameObject contentUI;
    public GameObject _UIElemant;

    public List<GameObject> UiElements = new List<GameObject>();
    

    //this is a protoype probally wont work with final vision

    public void DoTurn(int turn)
    {
        Turn _turn = turns[turn];
        _turn.DoTurn(turn);
    }
    
    public void AddUIElement()
    {
        Debug.Log("owO");
        GameObject turn = Instantiate(_UIElemant, contentUI.transform);
        turn.GetComponentInChildren<TextMeshProUGUI>().text = turns.Count.ToString();

        if (turns[turns.Count - 1].GetType() == typeof(MoveTurn))
        {
            turn.GetComponent<Image>().color = Color.blue;
        }

        if (turns[turns.Count - 1].GetType() == typeof(AttackTurn))
        {
            turn.GetComponent<Image>().color = Color.green;
        }

        UiElements.Add(turn); 
    }

}

public class Turn
{
    public GameObject character;
    public MapStats stats;
    public TurnManager manager;

    
    public virtual void DoTurn(int turn) 
    {

    }
}

public class MoveTurn : Turn
{
    public List<Vector3> path = new List<Vector3>();
    public MoveTurn(GameObject _Character, List<Vector3> paths, MapStats _stats)
    {
        foreach (Vector3 _Node in paths)
        {
            path.Add(_Node);
        }
        character = _Character;
        stats = _stats;
    }

    public void addTurn(MoveTurn turn, TurnManager manager)
    {
        this.manager = manager;
        manager.turns.Add(turn);
    }


    public override void DoTurn(int turn)
    {
        manager.movement.StartCoroutine(manager.movement.StartPath(path, 1, character, turn));
    }
}

public class AttackTurn : Turn
{
    CharacterInfo characterInfo;
    Vector2 dir;

    public AttackTurn(CharacterInfo characterInfo, Vector2 dir)
    {
        this.characterInfo = characterInfo;
        this.dir = dir; 
    }

    public void addTurn(AttackTurn turn, TurnManager turnManager)
    {
        manager = turnManager;
        manager.turns.Add(turn);
    }

    public override void DoTurn(int turn)
    {
    }
}
