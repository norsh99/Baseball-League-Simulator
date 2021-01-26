using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamStats
{
    public Team team;
    public int wins;
    public int loses;
    public int totalGames;


   public TeamStats(Team team)
    {
        this.team = team;
        wins = 0;
        loses = 0;
        totalGames = 0;
    }
}
