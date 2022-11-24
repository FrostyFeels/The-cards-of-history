using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamSelectorManager : MonoBehaviour
{
    public FillCharacterSlot fillSlot;


    public Team[] teams;
    public Team currentTeam;

    [SerializeField] private Image[] teamImages;
    [SerializeField] private Image lastSelected;
    [SerializeField] private List<GameObject> _DisableUI;


    [SerializeField] private GameObject teamslotspanel;
    [SerializeField] private GameObject CharacterSlotsPanel;

    [SerializeField] private Transform holder;
    [SerializeField] private int teamSize;
    

    private void Start()
    {
        teams = new Team[5];

        for (int i = 0; i < teams.Length; i++)
        {
            teams[i] = new Team();
        }
        FillTeams();
    }

    public void FillTeams()
    {
        for (int i = 0; i < teams.Length; i++)
        {

          
            Character[] characters = new Character[teamSize];

            int number = i + 1;
            characters = Resources.LoadAll<Character>("Teams/Team " + number);

            Debug.Log(teams[i]);

            teams[i].characters = characters;
            teams[i].teamId = i;
            Debug.Log(characters.Length);

        }
    }

    public void SelectSlot()
    {
        EnableUI(CharacterSlotsPanel);
        DeleteView();
    }

    public void SelectTeam(int teamNumber)
    {
        if(lastSelected != null)
            lastSelected.enabled = false;

        teamImages[teamNumber].enabled = true;
        lastSelected = teamImages[teamNumber];

        currentTeam = teams[teamNumber];
        FillSlots(teamNumber);
        EnableUI(teamslotspanel);
    }

    public void FillSlots(int teamNumber)
    {
        Debug.Log(teamNumber);
        Debug.Log(teams[teamNumber].characters.Length);
        fillSlot.FillScrollView(holder, teams[teamNumber].characters, teamSize, false);
    }

    public void EnableUI(GameObject ui)
    {
        foreach (GameObject _UI in _DisableUI)
        {
            _UI.SetActive(false);
        }

        _DisableUI.Clear();

        ui.SetActive(true);
        _DisableUI.Add(ui);
    }

    public void DeleteView()
    {
        fillSlot.DeleteScrollView(holder.transform);
    }
}
