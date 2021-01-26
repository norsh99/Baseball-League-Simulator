using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TeamInfoCell : MonoBehaviour
{
    public TextMeshProUGUI cell1;
    public TextMeshProUGUI cell2;
    public TextMeshProUGUI cell3;

    private TeamStats teamStats;
    private StandingsPage standingPage;

    public Image background;
    public Color[] colorSelection;
    private bool isSelected;
    public void LoadInfo(TeamStats teamStats, StandingsPage standingPage)
    {
        this.teamStats = teamStats;
        this.standingPage = standingPage;
        isSelected = false;
        cell1.text = teamStats.team.GetName();
        cell2.text = teamStats.wins.ToString();
        cell3.text = teamStats.loses.ToString();
    }
    public TeamStats GetTeam()
    {
        return teamStats;
    }

    public void Selection(bool selectOn) {
        if (selectOn)
        {
            background.color = colorSelection[1];
            isSelected = true;
        }
        else
        {
            background.color = colorSelection[0];
            isSelected = false;

        }

    }
    public void ButtonClick()
    {
        standingPage.ViewTeamStats(teamStats.team, this);
    }
}
