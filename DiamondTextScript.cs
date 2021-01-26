using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiamondTextScript : MonoBehaviour
{

    public GameObject blackBox;
    public TextMeshProUGUI textBox;
   



    public void UpdateText(string name)
    {
        if (name == "" || name == null)
        {
            blackBox.SetActive(false);
        }else
        {
            blackBox.SetActive(true);
            textBox.text = name;
        }
    }

}
