using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    public CharacterSaveAttack attack;
    public CharacterSpawn spawn;
    public CharacterPathLogic pathLogic;
    public CharacterInfo character;

    [SerializeField] private LayerMask charMask;


 
    public MapStats stats;


    public List<GameObject> _Characters = new List<GameObject>();
    public List<GameObject> _Enemies = new List<GameObject>();

    public List<MonoBehaviour> classes = new List<MonoBehaviour>();


    public enum Mode
    {
        _Moving,
        _Attacking
    }

    public Mode mode;

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
    public void SelectChar()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200 , charMask))
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
