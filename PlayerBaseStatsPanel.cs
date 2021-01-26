using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayerBaseStatsPanel : MonoBehaviour
{
    public TextMeshProUGUI playerName;

    public ProgressBar batterStatsBar;
    public ProgressBar fielderStatsBar;
    public ProgressBar runnerStatsBar;
    public ProgressBar pitcherStatsBar;
    public ProgressBar staminaStatsBar;

    public Color ExcellentColor;
    public Color NormalColor;
    public Color PoorColor;

    public Image batterImage;
    public Image fielderImage;
    public Image runnerImage;
    public Image pitcherImage;
    public Image staminaImage;

    public TextMeshProUGUI totalStatsTextBox;

    private int level;


    public void LoadInfo(Player player)
    {
        int maxStat = player.GetMaxBaseScore();
        level = player.GetLevelStat();

        SetBar(batterStatsBar, 0, maxStat, player.GetBatterScore());
        SetBar(fielderStatsBar, 0, maxStat, player.GetFielderScore());
        SetBar(runnerStatsBar, 0, maxStat, player.GetRunningSpeed());
        SetBar(pitcherStatsBar, 0, maxStat, player.GetPitcherScore());
        SetBar(staminaStatsBar, 0, maxStat, player.GetStaminaScore());

        batterImage.color = FindColor(player.GetBatterScore());
        fielderImage.color = FindColor(player.GetFielderScore());
        runnerImage.color = FindColor(player.GetRunningSpeed());
        pitcherImage.color = FindColor(player.GetPitcherScore());
        staminaImage.color = FindColor(player.GetStaminaScore());

        playerName.text = player.GetNameOfPlayer();
        totalStatsTextBox.text = player.GetPlayerBaseStatsRating() + "%";
    }

    public void LoadInfo(Team team)
    {
        int maxStat = team.GetTeamLevel() * 10;
        level = team.GetTeamLevel();

        int batterScore = team.GetTeamAvg_Batting();
        int fieldScore = team.GetTeamAvg_Fielding();
        int runnerScore = team.GetTeamAvg_Running();
        int pitcherScore = team.GetTeamPitcherScore();
        int staminaScore = team.GetTeamAvg_Stamina();

        float avg = ((batterScore + fieldScore + runnerScore + pitcherScore + staminaScore) / (maxStat * 5f)) * 100f;
        SetBar(batterStatsBar, 0, maxStat, batterScore);
        SetBar(fielderStatsBar, 0, maxStat, fieldScore);
        SetBar(runnerStatsBar, 0, maxStat, runnerScore);
        SetBar(pitcherStatsBar, 0, maxStat, pitcherScore);
        SetBar(staminaStatsBar, 0, maxStat, staminaScore);

        batterImage.color = FindColor(batterScore);
        fielderImage.color = FindColor(fieldScore);
        runnerImage.color = FindColor(runnerScore);
        pitcherImage.color = FindColor(pitcherScore);
        staminaImage.color = FindColor(staminaScore);

        playerName.text = team.GetName();
        totalStatsTextBox.text = avg + "%";
    }

    public void SetBar(ProgressBar pg, float min, float max, float current)
    {
        pg.minimum = min;
        pg.maximum = max;
        pg.current = current;
        pg.color = FindColor(current);
    }

    public Color FindColor(float score)
    {
        Color theColor;
        if (score <= 3 * level)
        {
            theColor = PoorColor;
        }
        else if (score >= 7 * level)
        {
            theColor = ExcellentColor;
        }
        else
        {
            theColor = NormalColor;
        }
        return theColor;
    }

}
