using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    //Team
    public string teamName;

    //Offense
    public int atBats;
    public int hits;
    private int battingAvg;
    public int homeruns;
    public int runs;
    public int doubles;
    public int triples;
    public int RBI;
    public int battingStrikeouts;

    //Defense
    public int error;
    public int pitcherStrikeouts;
    public int pitcherWalks;
    public int runsAllowed;
    public int totalPitches;
    public int pitcherWins;
    public int pitcherLoses;


    public Stats(string team)
    {
        teamName = team;

        //Offense
        atBats = 0;
        hits = 0;
        battingAvg = 0;
        homeruns = 0;
        runs = 0;
        doubles = 0;
        triples = 0;
        RBI = 0;
        battingStrikeouts = 0;

        //Defense
        error = 0;
        pitcherStrikeouts = 0;
        runsAllowed = 0;
        pitcherWalks = 0;
        totalPitches = 0;
        pitcherWins = 0;
        pitcherLoses = 0;
    }

    public float GetBattingAverage()
    {
        float math = (float)hits / (float)atBats;
        if (hits == 0) { math = 0; }
        return math;
    }

    public List<string> ReturnOffenseHeaders()
    {
        List<string> newList = new List<string>();
        newList.Add("AB");
        newList.Add("Hits");
        newList.Add("AVG");
        newList.Add("HR");
        newList.Add("Runs");
        newList.Add("2B");
        newList.Add("3B");
        newList.Add("RBI");
        newList.Add("SO");


        return newList;
    }
    public List<string> ReturnOffenseStats()
    {

        List<string> newList = new List<string>();
        newList.Add(atBats.ToString());
        newList.Add(hits.ToString());
        newList.Add(GetBattingAverage().ToString("F3").TrimStart('0'));
        newList.Add(homeruns.ToString());
        newList.Add(runs.ToString());
        newList.Add(doubles.ToString());
        newList.Add(triples.ToString());
        newList.Add(RBI.ToString());
        newList.Add(battingStrikeouts.ToString());

        return newList;
    }

    
}
