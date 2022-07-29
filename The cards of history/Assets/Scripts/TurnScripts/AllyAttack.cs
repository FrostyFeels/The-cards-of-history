using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyAttack : MonoBehaviour
{
    public MapStats stats;
    public TurnManager turnManager;

    public CharacterInfo character;

    public List<MapData> data = new List<MapData>();

    public void Attack(CharacterInfo character, Vector2 dir, int turnIndex)
    {
        this.character = character;

        if (dir.y > 0) Up();
        if (dir.y < 0) Down();
        if (dir.x > 0) Right();
        if (dir.x < 0) Left();

        StartCoroutine(CleanColor());

        foreach (GameObject item in turnManager.UiElements)
        {
            Destroy(item);
        }
        turnManager.turns.Clear();
        turnManager.UiElements.Clear();

        turnManager.DoTurn(turnIndex);
    }

    public IEnumerator CleanColor()
    {
        yield return new WaitForSeconds(2);
        foreach (MapData _data in data)
        {
            MaterialManager.SetMaterial(_data.GetRender(), _data._materialID);
        }
    }
    public void Up()
    {
        Vector2 playerPos = new Vector2(character.pos.x, character.pos.z);
        Vector2 attackMid = new Vector2(character.attackMap.startPoint.x, character.attackMap.startPoint.y);

        foreach (MapData _data in character.attackMap.map)
        {
            if (_data._selected)
            {
                Vector2 tilePos = new Vector2(_data.xPos, _data.zPos);
                Vector2 realTilePos = tilePos - attackMid;
                Vector2 start = playerPos + realTilePos;


                if ((start.x > 0 && start.x < stats.map[0].gridSizeX) && (start.y > 0 && start.y < stats.map[0].gridSizeY))
                {
                    MapData tile = stats.mapData[(int)start.x, 0, (int)start.y];

                    MaterialManager.SetMaterial(tile.GetRender(), "Green");
                }

            }
        }
    }
    public void Down()
    {
        Vector2 playerPos = new Vector2(character.pos.x, character.pos.z);
        Vector2 attackMid = new Vector2(character.attackMap.startPoint.x, character.attackMap.startPoint.y);

        foreach (MapData _data in character.attackMap.map)
        {
            if (_data._selected)
            {
                Vector2 tilePos = new Vector2(_data.xPos, -_data.zPos);
                Vector2 realTilePos = tilePos - new Vector2(attackMid.x, -attackMid.y); ;
                Vector2 start = playerPos + realTilePos;


                if ((start.x > 0 && start.x < stats.map[0].gridSizeX) && (start.y > 0 && start.y < stats.map[0].gridSizeY))
                {
                    MapData tile = stats.mapData[(int)start.x, 0, (int)start.y];

                    MaterialManager.SetMaterial(tile.GetRender(), "Green");
                }

            }
        }
    }
    public void Right()
    {
        Vector2 playerPos = new Vector2(character.pos.x, character.pos.z);
        Vector2 attackMid = new Vector2(character.attackMap.startPoint.y, character.attackMap.startPoint.x);

        foreach (MapData _data in character.attackMap.map)
        {
            if (_data._selected)
            {
                Vector2 tilePos = new Vector2(-_data.zPos, _data.xPos);
                Vector2 realTilePos = tilePos - new Vector2(-attackMid.x, attackMid.y);
                Vector2 start = playerPos + realTilePos;


                if ((start.x > 0 && start.x < stats.map[0].gridSizeX) && (start.y > 0 && start.y < stats.map[0].gridSizeY))
                {
                    MapData tile = stats.mapData[(int)start.x, 0, (int)start.y];

                    MaterialManager.SetMaterial(tile.GetRender(), "Green");
                }

            }
        }
    }
    public void Left()
    {
        Vector2 playerPos = new Vector2(character.pos.x, character.pos.z);
        Vector2 attackMid = new Vector2(character.attackMap.startPoint.y, character.attackMap.startPoint.x);

        foreach (MapData _data in character.attackMap.map)
        {
            if (_data._selected)
            {
                Vector2 tilePos = new Vector2(_data.zPos, _data.xPos);
                Vector2 realTilePos = tilePos - attackMid;
                Vector2 start = playerPos + realTilePos;


                if ((start.x > 0 && start.x < stats.map[0].gridSizeX) && (start.y > 0 && start.y < stats.map[0].gridSizeY))
                {
                    MapData tile = stats.mapData[(int)start.x, 0, (int)start.y];

                    MaterialManager.SetMaterial(tile.GetRender(), "Green");
                }

            }
        }
    }
}
