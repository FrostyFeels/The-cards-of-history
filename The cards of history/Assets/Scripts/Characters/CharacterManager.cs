using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    [SerializeField] private CharacterSaveAttack attack;
    [SerializeField] private CharacterSpawn spawn;
    [SerializeField] private CharacterPathLogic pathLogic;
    [SerializeField] private CharacterInfo character;

    [SerializeField] private LayerMask charMask;



    [SerializeField] private MapStats stats;


    [SerializeField] private List<GameObject> _Characters = new List<GameObject>();
    [SerializeField] private List<GameObject> _Enemies = new List<GameObject>();

    //diffrent scripts
    [SerializeField] private List<MonoBehaviour> classes = new List<MonoBehaviour>();

    private int camToMouseRange = 200;


    public enum Mode
    {
        _Moving,
        _Attacking
    }

    public Mode mode;



    //This is the manager for the diffrent character parts like setting up the player and enemies how their walk logic works and how their saving of attacks work

    public void Start()
    {

        spawn.SetPlayers(stats.playerSpots, stats.map[0].tileSize);
        spawn.SetEnemies(stats.enemySpots, stats.map[0].tileSize);

        for (int i = 0; i < spawn._allyCharacters.Length; i++)
        {
            _Characters.Add(spawn._allyCharacters[i]);
            _Characters[i].GetComponent<CharacterInfo>().SetPos(stats.playerSpots[i]._ID);    
        }

        for (int i = 0; i < spawn._EnemyCharacters.Length; i++)
        {
            _Enemies.Add(spawn._EnemyCharacters[i]);
            _Enemies[i].GetComponent<CharacterInfo>().SetPos(stats.enemySpots[i]._ID);
        }

        classes.Add(attack);
        classes.Add(pathLogic);

    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectChar();
        }         
    }

    //For selecting a character while its your turn
    public void SelectChar()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, camToMouseRange, charMask))
        {
            if(hit.collider.CompareTag("Player"))
            {
                character = hit.collider.gameObject.GetComponent<CharacterInfo>();
                pathLogic.selected = character;
                attack.character = character;
                ModeLogic();
            }
        }
    }

    //Changing the mode to what mode it needs to be while disabling the other scripts
    public void ModeLogic()
    {
        foreach (MonoBehaviour _class in classes)
        {
            _class.enabled = false;
        }

        switch (mode)
        {
            case Mode._Moving:
                pathLogic.enabled = true;
                pathLogic.NewPath();
                break;
            case Mode._Attacking:
                attack.enabled = true;
                attack.NewSelect();
                break;
        }
    }



    

}
