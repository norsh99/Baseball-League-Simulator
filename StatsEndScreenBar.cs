using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsEndScreenBar : MonoBehaviour
{
    public TextMeshProUGUI nameTextbox;
    public TextMeshProUGUI positionTextbox;
    public TextMeshProUGUI xpTextbox;
    public TextMeshProUGUI upgradeTextbox;

    public GameObject upgradeBox;

    public ProgressBar upgradeProgressBar;
    public AthleticBadge athleticBadge;

    public Color upgradeColor;

    private Player player;

    private int prevLevel;
    private int prevMaxXP;
    private int prevMinRange;
    private int prevXP;

    //Animated Vars
    private int animatedCurrentXP;
    private bool animateAll;
    private int xpAnimate;

    public void LoadInfo(Player player, float timer)
    {
        animateAll = false;

        this.player = player;
        int[] allPrevStats = player.GetPrevStats();

        //Progress Bar
        prevLevel = allPrevStats[0];
        prevMaxXP = allPrevStats[1];
        prevMinRange = allPrevStats[2];
        prevXP = allPrevStats[3];

        upgradeProgressBar.minimum = prevMinRange;
        upgradeProgressBar.maximum = prevMaxXP;
        upgradeProgressBar.current = 0;
        animatedCurrentXP = 0;
        xpAnimate = 0;

        //Text Boxes
        nameTextbox.text = player.GetNameOfPlayer();
        positionTextbox.text = player.GetPositionString();
        xpTextbox.text = xpAnimate.ToString();
        upgradeTextbox.text = player.GetLatestUpgradeCoins().ToString();


        //Badge
        athleticBadge.UpdateBadgeAndStars(prevLevel, player.GetAthleticsStat());




        StartCoroutine(Delay(timer));

    }
    private void Update()
    {
        if (animateAll)
        {
            IncreaseXpProgressBar();
            AnimateXpText();
        }

    }

    private void IncreaseXpProgressBar()
    {
        if (animatedCurrentXP <= player.GetLatestGameXP() + prevXP)
        {
            animatedCurrentXP += 5;
            upgradeProgressBar.current = animatedCurrentXP;
        }
        if (animatedCurrentXP >= prevMaxXP)
        {
            //Increase the level on the badge

            athleticBadge.UpdateBadgeAndStars(player.GetLevelStat(), player.GetAthleticsStat());
            upgradeProgressBar.color = upgradeColor;
            athleticBadge.SetStarColor(upgradeColor);
        }
    }

    IEnumerator Delay(float timer)
    {
        yield return new WaitForSeconds(timer);
        animateAll = true;
    }
    private void AnimateXpText()
    {
        if (xpAnimate <= player.GetLatestGameXP())
        {
            xpAnimate += 1;
            xpTextbox.text = xpAnimate.ToString();

        }
    }
}
