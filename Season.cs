using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Season
{

    private int totalVisitors;
    private int totalTicketSales;

    public SeasonSchedule currentSeasonSchedule;

    public User user;
    public HomeCode homeCode;

    public Season(User user, TeamsAllDivisions allDivisions ,HomeCode homeCode)
    {
        this.user = user;
        this.homeCode = homeCode;
        user.IncreaseSeasonsPlayed();


        currentSeasonSchedule = new SeasonSchedule(allDivisions);

        //Variables
        totalVisitors = 0;
        totalTicketSales = 0;

        NewStatsForAllTeams();


    }

    private void NewStatsForAllTeams()
    {
        List<Team> allTeams = homeCode.allDivisions.GetAllTeamsIncludeUserTeam();
        for (int i = 0; i < allTeams.Count; i++)
        {
            allTeams[i].NewSeasonStats();
            for (int m = 0; m < allTeams[i].GetAllPlayers().Count; m++)
            {
                allTeams[i].GetAllPlayers()[m].NewSeasonStatsReset();
            }
        }
    }

    public void AddToTotalVisitors(int amount) { totalVisitors += amount; }
    public int GetTotalVisitors() { return totalVisitors; }

    public void AddToTotalTicketSales(int amount) { totalTicketSales += amount; }
    public int GetTotalTicketSales() { return totalTicketSales; }


}
