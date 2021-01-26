using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreboardCell : MonoBehaviour
{

    public TextMeshProUGUI infoBox;
    public TextMeshProUGUI cellTextBox1;
    public TextMeshProUGUI cellTextBox2;


    public void LoadInfo(string info, string cell1, string cell2)
    {
        infoBox.text = info;
        cellTextBox1.text = cell1;
        cellTextBox2.text = cell2;
    }



}
