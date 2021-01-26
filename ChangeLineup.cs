using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLineup : MonoBehaviour
{
    public GameObject leftArrow;
    public GameObject rightArrow;

    private void Start()
    {
        TurnOffAll();
    }

    public void TurnOnUp()
    {
        leftArrow.SetActive(true);
    }
    public void TurnOnDown()
    {
        rightArrow.SetActive(true);
    }
    public void TurnOffDown()
    {
        rightArrow.SetActive(false);
    }
    public void TurnOffUp()
    {
        leftArrow.SetActive(false);
    }
    public void TurnOffAll()
    {
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
    }
}
