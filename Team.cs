using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Team
{
    private string teamName;

    List<Player> allPlayers;
    List<Player> startingLineup;
    //List<Player> defLineup;


    //Current Season Stats and Schedule
    private TeamStats currentSeasonStats;
    private List<TeamVsTeam> currentSeasonSchedule;

    private int level; //What the level is at. - i.e. a, aa, aaa, mlb
    private HomeCode hc;
    public Team(string name, HomeCode hc)
    {
        this.hc = hc;
        allPlayers = new List<Player>();
        currentSeasonStats = new TeamStats(this);
        level = 1;
        if (name == null)
        {
            teamName = "Team " + RandomNum(0,5000);
        }
        else
        {
            teamName = name;
        }
        
    }

    public void NewSeasonStats()
    {
        currentSeasonStats = new TeamStats(this);

    }

    public void UpgradeLevel()
    {
        level += 1;
    }

    public float GetTeamTradeLikelinessRatio()
    {
        float wins = currentSeasonStats.wins;
        float totalGames = currentSeasonStats.totalGames;
        if (wins == 0)
        {
            wins = 1;
        }
        if (totalGames == 0)
        {
            totalGames = 1;
        }

        float ratio = wins / totalGames;

        ratio += .3f;

        return ratio;
    }


    public int GetTeamLevel() { return level; }
    public void ResetSchedule()
    {
        currentSeasonSchedule = new List<TeamVsTeam>();
    }

    public void SetNewMatchupToSchedule(TeamVsTeam newMatchup) 
    {
        if (currentSeasonSchedule == null)
        {
            currentSeasonSchedule = new List<TeamVsTeam>();
        }
        currentSeasonSchedule.Add(newMatchup);
    }
    public List<TeamVsTeam> GetCurrentSeasonEntireSchedule() { return currentSeasonSchedule; }
    public TeamVsTeam GetCurrentSeasonWeeklyMatchup(int week) { return currentSeasonSchedule[week]; }


    public void SetCurrentSeasonTeamStats(TeamStats teamstats) { currentSeasonStats = teamstats; }
    public TeamStats GetCurrentSeasonTeamStats() { return currentSeasonStats; }

    public void SetPlayersToDefPositions()
    {
        Player[] arrayList = new Player[] { null, null, null, null, null, null, null, null, null };
        for (int i = 0; i < allPlayers.Count; i++)
        {
            int index = allPlayers[i].GetPosition();
            arrayList[index-1] = allPlayers[i];
        }
        allPlayers = arrayList.ToList();
    }

    public void SetBattingLineUp()
    {
        List<Player> bbList = new List<Player>();
        List<Player> currentList = new List<Player>(allPlayers);
        Player currentHighestPlayer = null;

        for (int i = 0; i < 9; i++)
        {
            for (int p = 0; p < currentList.Count; p++)
            {
                if (p == 0)
                {
                    currentHighestPlayer = currentList[p];

                }
                else
                {
                    if (currentList[p].GetBatterScore() > currentHighestPlayer.GetBatterScore())
                    {
                        currentHighestPlayer = currentList[p];
                    }
                }
                
            }
            currentList.Remove(currentHighestPlayer);
            bbList.Add(currentHighestPlayer);
        }
        Player[] arrayList = new Player[] { bbList[2], bbList[1], bbList[0], bbList[3], bbList[4], bbList[5], bbList[6], bbList[7], bbList[8] };
        List<Player> newBatterList = arrayList.ToList();
        SetStartingLineUp(newBatterList);
    }

    private string CreateNewPlayerName()
    {
        int fname = RandomNum(1, hc.allNames.Count);
        int lname = RandomNum(1, hc.allNames.Count);
        string finalName = hc.allNames[fname-1].firstName + " " + hc.allNames[lname-1].lastName;
        return finalName;
    }


    public void CreateBrandNewTeam()
    {
        allPlayers = new List<Player>();
        for (int i = 0; i < 9; i++)
        {
            Player newPlayer = CreateNewPlayer(CreateNewPlayerName());
            allPlayers.Add(newPlayer);
        }
    }
    public void CreateBrandNewTeamAllDefensePositionsFilled()
    {
        allPlayers = new List<Player>();
        for (int i = 0; i < 9; i++)
        {
            Player newPlayer = CreateNewPlayer(CreateNewPlayerName());
            newPlayer.SetPosition(i+1);
            allPlayers.Add(newPlayer);
        }
    }

    public void AddPlayerToTeam(Player player)
    {
        allPlayers.Add(player);
    }

    public string GetName() { return teamName; }

    public Player CreateNewPlayer(string playerName)
    {
        Player newPlayer = new Player(playerName, this);
        return newPlayer;
    }

    public List<Player> GetAllPlayers()
    {
        return allPlayers;
    }
    private int RandomNum(int num1, int num2)
    {
        return UnityEngine.Random.Range(num1, num2 + 1);
    }
    public List<Player> GetStartingLineUp() { return startingLineup; }
    public void SetStartingLineUp(List<Player> newLineUp) { startingLineup = newLineUp; }

    public int GetTeamAvg_Batting()
    {
        float total = 0;
        int newNum = 0;

        float count = 0;
        for (int i = 0; i < allPlayers.Count; i++)
        {
            count += 1;
            total += allPlayers[i].GetBatterScore();
        }
        newNum = (int)Math.Round(total / count);
        return newNum;
    }
    public int GetTeamAvg_Running()
    {
        float total = 0;
        int newNum = 0;

        float count = 0;
        for (int i = 0; i < allPlayers.Count; i++)
        {
            count += 1;
            total += allPlayers[i].GetRunningSpeed();
        }
        newNum = (int)Math.Round(total / count);
        return newNum;
    }
    public int GetTeamAvg_Fielding()
    {
        float total = 0;
        int newNum = 0;

        float count = 0;
        for (int i = 0; i < allPlayers.Count; i++)
        {
            count += 1;
            total += allPlayers[i].GetFielderScore();
        }
        newNum = (int)Math.Round(total / count);
        return newNum;
    }
    public int GetTeamAvg_Stamina()
    {
        float total = 0;
        int newNum = 0;

        float count = 0;
        for (int i = 0; i < allPlayers.Count; i++)
        {
            count += 1;
            total += allPlayers[i].GetStaminaScore();
        }
        newNum = (int)Math.Round(total / count);
        return newNum;
    }
    public int GetTeamPitcherScore() { return allPlayers[3].GetPitcherScore(); }
}
