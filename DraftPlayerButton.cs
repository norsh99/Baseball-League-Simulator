using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DraftPlayerButton : MonoBehaviour
{
    public Image buttonImage;
    public List<Color> draftColors;
    public TextMeshProUGUI valueTextBox;

    public void SetInfo(bool canPurchase, string costOrError)
    {
        if (canPurchase)
        {
            valueTextBox.text = costOrError;
            buttonImage.color = draftColors[0];
        }
        else
        {
            valueTextBox.text = costOrError;
            buttonImage.color = draftColors[1];


        }
    }
    public void SetForSale(string valueCost)
    {
        valueTextBox.text = "Trade Player: " + valueCost;
        buttonImage.color = draftColors[2];
    }
}
