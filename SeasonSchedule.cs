using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonSchedule
{

    private List<WeeklyMatchups> seasonSchedule;

    private List<Team> div1;
    private List<Team> div2;

    private int weekIndex;
    private TeamsAllDivisions teamsAllDivisions;
    public SeasonSchedule(TeamsAllDivisions teamsAllDivisions)
    {
        this.teamsAllDivisions = teamsAllDivisions;
        weekIndex = 1;

        div1 = teamsAllDivisions.division_A1;
        div2 = teamsAllDivisions.division_A2;

        seasonSchedule = GenerateWeeklySchedulev2(div1, div2, 3);
    }

    
    public void AddWeek()
    {
        weekIndex += 1;
    }
    public int GetWeekIndex() { return weekIndex; }
    public int GetMaxWeeksInSeason() { return seasonSchedule.Count; }


    public TeamVsTeam GetMatchup(Team team, int week)
    {
        List<TeamVsTeam> theList = seasonSchedule[week].GetWeekMatchup();
        for (int i = 0; i < theList.Count; i++)
        {
            if (theList[i].awayTeam == team)
            {
                return theList[i];
            }
            else if (theList[i].homeTeam == team)
            {
                return theList[i];

            }
        }
        return null;
    }
    //Round Robin style scheduled matchups
    private List<WeeklyMatchups> GenerateWeeklySchedulev2(List<Team> d1, List<Team> d2, int playEachTeamHowManyTimes = 1)
    {
        List<WeeklyMatchups> newSeason = new List<WeeklyMatchups>();

        List<Team> row = new List<Team>(d1);
        List<Team> row2 = new List<Team>(d2);

        ResetScheduleForEachTeam();

        for (int i = 0; i < row.Count + row2.Count - 1; i++)
        {

            List<TeamVsTeam> week = new List<TeamVsTeam>();
            for (int m = 0; m < row2.Count; m++) 
            {
                Team h = row[m];
                Team a = row2[m];
                //To flip around the home team now and then so the same team isn't always the home team
                if (m % 2 == 0)
                {
                    h = row2[m];
                    a = row[m];
                }
                TeamVsTeam newMatch = new TeamVsTeam(a, h);

                for (int l = 0; l < playEachTeamHowManyTimes; l++)
                {
                    a.SetNewMatchupToSchedule(newMatch);
                    h.SetNewMatchupToSchedule(newMatch);
                }
                


                week.Add(newMatch);
            }

            Team holdTeam = row[row.Count-1];
            Team holdTeam2 = row2[0];

            row.RemoveAt(row.Count - 1);
            row2.RemoveAt(0);

            row.Insert(1, holdTeam2);
            row2.Add(holdTeam);


            WeeklyMatchups wm = new WeeklyMatchups(week);
            for (int j = 0; j < playEachTeamHowManyTimes; j++)
            {
                newSeason.Add(wm);
            }
        }
        return newSeason;
    }

    public List<WeeklyMatchups> GetWeeklyMatchups() { return seasonSchedule; }

    private void ResetScheduleForEachTeam()
    {
        List<Team> allTeams = teamsAllDivisions.GetAllTeamsIncludeUserTeam();
        for (int i = 0; i < allTeams.Count; i++)
        {
            allTeams[i].ResetSchedule();
        }
    }
}



public class WeeklyMatchups
{
    private List<TeamVsTeam> weekMatchup;
    public WeeklyMatchups(List<TeamVsTeam> matchups)
    {
        weekMatchup = matchups;
    }
    public List<TeamVsTeam> GetWeekMatchup()
    {
        return weekMatchup;
    }
}


public class TeamVsTeam
{

    public Team homeTeam;
    public Team awayTeam;
    public GameScoreCard gameScoreCard;
    public bool isGameComplete;

    public TeamVsTeam(Team awayTeam, Team homeTeam)
    {
        this.awayTeam = awayTeam;
        this.homeTeam = homeTeam;
        isGameComplete = false;
    }
    public Team GetWinningTeam()
    {
        int awayScore = gameScoreCard.GetScore(awayTeam);
        int homeScore = gameScoreCard.GetScore(homeTeam);
        if (awayScore > homeScore)
        {
            return awayTeam;
        }
        else
        {
            return homeTeam;

        }
    }

    public void SetGameScoreCard(GameScoreCard gameScoreCard) { this.gameScoreCard = gameScoreCard; }
}
