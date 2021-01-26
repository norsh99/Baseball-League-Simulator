using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuPlayerInfoCell : MonoBehaviour
{
    public ProgressBar overallStatProgressBar;
    public AthleticBadge athleticBadge;
    public TextMeshProUGUI positionTextBox;
    public TextMeshProUGUI nameTextBox;
    public TextMeshProUGUI valueTextBox;

    public List<Color> progressBarColor;

    private Player player;

    private PickPlayersScreen pps;

    private bool isBought;
    public GameObject boughtScreen;
    public GameObject mainMenuScreen;
    public TextMeshProUGUI purchaseScreenName;


    public void LoadInfo(Player player, PickPlayersScreen pps)
    {
        this.player = player;
        this.pps = pps;
        isBought = false;

        overallStatProgressBar.minimum = 0;
        overallStatProgressBar.maximum = 100;
        overallStatProgressBar.current = player.GetPlayerBaseStatsRating();
        overallStatProgressBar.color = FindColor(player.GetPlayerBaseStatsRating());

        athleticBadge.UpdateBadgeAndStars(player.GetLevelStat(), player.GetAthleticsStat());
        positionTextBox.text = player.GetPositionString();
        nameTextBox.text = player.GetNameOfPlayer();
        valueTextBox.text = player.GetValue().ToString("c0");
        purchaseScreenName.text = player.GetNameOfPlayer();
    }


    private Color FindColor(float score)
    {
        Color theColor;
        if (score <= 30)
        {
            theColor = progressBarColor[0];
        }
        else if (score >= 70)
        {
            theColor = progressBarColor[2];
        }
        else
        {
            theColor = progressBarColor[1];
        }
        return theColor;
    }

    public void ButtonClick()
    {
        if (pps != null && !isBought)
        {
            pps.ActivatePlayerInfoScreen(player, this);
        }
       
    }

    public void DraftPlayer()
    {
        boughtScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
        isBought = true;
    }

}
