using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTeam : MonoBehaviour
{
    public GameObject panel;
    public void OnMouseDown()
    {
        panel.SetActive(true);
    }

    public void GoBack()
    {
        panel.SetActive(false);
    }
}
