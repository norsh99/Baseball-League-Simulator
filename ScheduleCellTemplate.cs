using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScheduleCellTemplate : MonoBehaviour
{
    public TextMeshProUGUI cell;
    public Image background;

    public void LoadInfo(string textInfo, Color newBackgroundColor)
    {
        cell.text = textInfo;
        background.color = newBackgroundColor;
    }
}
