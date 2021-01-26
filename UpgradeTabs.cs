using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeTabs : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public Image image;

    public void SetInfo(Color color, string textinfo)
    {
        textField.text = textinfo;
        image.color = color;
    }
    public void ChangeColor(Color color)
    {
        image.color = color;
    }
    public Color GetCurrentColor()
    {
        return image.color;
    }
}
