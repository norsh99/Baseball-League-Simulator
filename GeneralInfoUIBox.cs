using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneralInfoUIBox : MonoBehaviour
{
    public AthleticBadge athleticBadge;

    public TextMeshProUGUI fieldPosition;
    public TextMeshProUGUI valueTextBox;
    public TextMeshProUGUI purchasedValue;


    public ProgressBar radialXPBar;

    private Player player;

    public void LoadInfo(Player player)
    {
        this.player = player;

        athleticBadge.UpdateBadgeAndStars(player.GetLevelStat(), player.GetAthleticsStat());

        fieldPosition.text = player.GetPositionString();
        valueTextBox.text = player.GetValue().ToString("C0");
        if (purchasedValue != null)
        {
            purchasedValue.text = player.GetBoughtValue().ToString("C0");
        }


        radialXPBar.minimum = 0;
        radialXPBar.maximum = player.GetMaxXP();
        radialXPBar.current = player.GetXP();
    }
}
