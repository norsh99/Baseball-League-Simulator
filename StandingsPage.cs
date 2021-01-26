using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StandingsPage : MonoBehaviour
{
    public TeamStandingsPanel teamStandingsPanel;
    public HomeCode homeCode;
    public TextMeshProUGUI nextGameTextbox;
    public TeamSchedulePanel teamSchedulePanel;
    public PlayerBaseStatsPanel playerBaseStatsPanel;

    public GameObject statsPage;
    public GameObject schedulePage;

    private bool divisionIndex;
    private bool statsPageOpen;
    private TeamInfoCell currentSelection;
    private TeamInfoCell prevSelection;

    public void LoadInfo()
    {
        divisionIndex = false;
        divisionIndex = false;

        List<Team> sortedList = SortByMostWin(homeCode.allDivisions.division_A1);
        teamStandingsPanel.LoadInfo(sortedList, "Division 1", this);

        //PrintWeekOfMatchups(homeCode.currentSeason.currentSeasonSchedule.GetWeeklyMatchups(), 1);

        CalculateNextMatchup(homeCode.user.GetTeam(), homeCode.currentSeason.currentSeasonSchedule.GetWeekIndex());
        teamSchedulePanel.LoadInfo(homeCode.allDivisions.division_A1, homeCode.currentSeason.currentSeasonSchedule.GetWeekIndex(), homeCode.currentSeason.currentSeasonSchedule.GetMaxWeeksInSeason());
    }


    private List<Team> SortByMostWin(List<Team> theList)
    {
        List<Team> pickList = new List<Team>(theList);

        List<Team> newList = new List<Team>();
        Team highest = null;
        for (int i = 0; i < theList.Count; i++)
        {
            for (int m = 0; m < pickList.Count; m++)
            {
                if (m == 0)
                {
                    highest = pickList[m];
                }
                else
                {
                    if (pickList[m].GetCurrentSeasonTeamStats().wins > highest.GetCurrentSeasonTeamStats().wins)
                    {
                        highest = pickList[m];
                    }
                }
                
            }
            pickList.Remove(highest);
            newList.Add(highest);
        }




        return newList;
    }


    private void CalculateNextMatchup(Team team, int week)
    {
        TeamVsTeam nextMatchup = team.GetCurrentSeasonWeeklyMatchup(week-1);
        if (team == nextMatchup.homeTeam)
        {
            nextGameTextbox.text = nextMatchup.awayTeam.GetName();
        }
        else
        {
            nextGameTextbox.text = nextMatchup.homeTeam.GetName();
        }
    }



    private void PrintWeekOfMatchups(List<WeeklyMatchups> weeklyMatchups, int weekNum)
    {
        List<TeamVsTeam> weekOfMatchups = weeklyMatchups[weekNum + 1].GetWeekMatchup();
        for (int i = 0; i < weekOfMatchups.Count; i++)
        {
            Debug.Log("Home Team: " + weekOfMatchups[i].homeTeam.GetName() + " | Away Team: " + weekOfMatchups[i].awayTeam.GetName());
        }
    }




    //Buttons
    public void ChangeDivisionButton()
    {
        if (!divisionIndex)
        {
            List<Team> sortedList = SortByMostWin(homeCode.allDivisions.division_A2);

            teamStandingsPanel.LoadInfo(sortedList, "Division 2", this);
            teamSchedulePanel.LoadInfo(homeCode.allDivisions.division_A2, homeCode.currentSeason.currentSeasonSchedule.GetWeekIndex(), homeCode.currentSeason.currentSeasonSchedule.GetMaxWeeksInSeason());

            divisionIndex = true;
        }
        else
        {
            List<Team> sortedList = SortByMostWin(homeCode.allDivisions.division_A1);

            teamStandingsPanel.LoadInfo(sortedList, "Division 1", this);
            teamSchedulePanel.LoadInfo(homeCode.allDivisions.division_A1, homeCode.currentSeason.currentSeasonSchedule.GetWeekIndex(), homeCode.currentSeason.currentSeasonSchedule.GetMaxWeeksInSeason());

            divisionIndex = false;
        }
        //Changing the visual to the schedule page;
        SchedulePageOff();
    }

    //Button Selection when someone clicks a cell
    public void ViewTeamStats(Team team, TeamInfoCell selection)
    {
        currentSelection = selection;

        //Tab Selection Visuals
        currentSelection.Selection(true);
        if (prevSelection != currentSelection && !statsPageOpen)
        {
            if (prevSelection != null)
            {
                prevSelection.Selection(false);
            }
            SchedulePageOn(team);
        }
        else if(prevSelection != currentSelection)
        {
            if (prevSelection != null)
            {
                prevSelection.Selection(false);
            }
            SchedulePageOn(team);
        }
        else if(prevSelection == currentSelection && !statsPageOpen)
        {
            currentSelection.Selection(true);

            SchedulePageOn(team);
        }
        else if (prevSelection == currentSelection && statsPageOpen)
        {
            currentSelection.Selection(false);

            SchedulePageOff();
        }
        prevSelection = currentSelection;
        
    }
    private void SchedulePageOn(Team team)
    {
        statsPage.SetActive(true);
        schedulePage.SetActive(false);
        playerBaseStatsPanel.LoadInfo(team);
        statsPageOpen = true;
    }

    private void SchedulePageOff()
    {
        statsPage.SetActive(false);
        schedulePage.SetActive(true);

        statsPageOpen = false;

        if (currentSelection != null)
        {
            currentSelection.Selection(false);
        }
    }

}
