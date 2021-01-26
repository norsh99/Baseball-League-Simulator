using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameScoreCard
{
    //Teams
    public Team t1;
    public Team t2;

    //Score
    private int[] score;


    public int outs;
    private int inningCounter;
    public bool halfInning;

    private int[] hits;
    private int[] errors;

    private List<Inning> allInnings;


    public GameScoreCard(Team team1, Team team2)
    {
        t1 = team1;
        t2 = team2;

        allInnings = new List<Inning>();

        score = new int[] { 0, 0 };
        hits = new int[] { 0, 0 };
        errors = new int[] { 0, 0 };

        outs = 0;
        inningCounter = 0;
        halfInning = false;

        StartNewInning();

    }

    public void StartNewInning()
    {
        Inning inning = new Inning();
        allInnings.Add(inning);
        inningCounter += 1;
    }

    public void AddScore(int amount, Team team)
    {
        if (team == t1)
        {
            score[0] += amount;
            allInnings[inningCounter - 1].score[0] += amount;
        }
        if (team == t2)
        {
            score[1] += amount;
            allInnings[inningCounter - 1].score[1] += amount;

        }
    }
    public int GetScore(Team team)
    {
        if (team == t1)
        {
            return score[0];
        }
        else
        {
            return score[1];

        }
    }

    public void AddHits(int amount, Team team)
    {
        if (team == t1)
        {
            hits[0] += amount;
        }
        else
        {
            hits[1] += amount;
        }
    }
    public int GetHits(Team team)
    {
        int num = 0;
        if (team == t1)
        {
            num = hits[0];
        }
        else
        {
            num = hits[1];
        }
        return num;
    }

    public void AddErrors(int amount, Team team)
    {
        if (team == t1)
        {
            errors[0] += amount;
        }
        else
        {
            errors[1] += amount;
        }
    }
    public int GetErrors(Team team)
    {
        int num = 0;
        if (team == t1)
        {
            num = errors[0];
        }
        else
        {
            num = errors[1];
        }
        return num;
    }

    public int GetScoreInningTeam(Team team, int inningNum)
    {
        if (team == t1)
        {
            return allInnings[inningNum - 1].score[0];
        }
        else
        {
            return allInnings[inningNum - 1].score[1];
        }
    }
    public int GetLatestScore()
    {
        if (halfInning)
        {
            return allInnings[allInnings.Count-1].score[1];
        }
        else
        {
            return allInnings[allInnings.Count-1].score[0];
        }
    }

    public int GetInningCounter() { return inningCounter; }
}


public class Inning
{
    public int[] score;
    public Inning()
    {
        score = new int[] {0, 0};
    }




} 