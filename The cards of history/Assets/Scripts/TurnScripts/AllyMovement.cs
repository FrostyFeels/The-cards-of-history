using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMovement : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private MapStats stats;


    //Starts the path of the first move turn then starts the next turn when done
    public IEnumerator StartPath(List<Vector3> path, int nodeIndex, GameObject character, int turnCount)
    {
        int tilesize = stats.map[0].tileSize;
        yield return StartCoroutine(Move(path, nodeIndex, character, tilesize));
        turnCount++;
        if (turnManager.turns.Count > turnCount)
        {
            turnManager.DoTurn(turnCount);
        }
        else
        {
            foreach (GameObject _UIElement in turnManager.UiElements)
            {
                Destroy(_UIElement);
            }
            turnManager.turns.Clear();
        }
    }

    //Moves the character one node then rerunning it with the next index
    public IEnumerator Move(List<Vector3> path, int nodeIndex, GameObject character, int tilesize)
    {
        Vector3 _RealStart = Vector3.zero;

        Vector3 _NextNode = path[nodeIndex];
        Vector3 _RealEnd = new Vector3((int)_NextNode.x, (int)_NextNode.y + 1, -(int)_NextNode.z) * tilesize;

        float timeElapsed = 0;
        float duration = .25f;

        _RealStart = character.transform.position;



        while (timeElapsed < duration)
        {
            character.transform.position = Vector3.Lerp(_RealStart, _RealEnd, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        character.transform.position = _RealEnd;
        character.GetComponent<CharacterInfo>().SetPos(_NextNode);

        if (path.Count > nodeIndex + 1)
        {
            yield return StartCoroutine(Move(path, nodeIndex + 1, character, tilesize));
        }
    }

}
