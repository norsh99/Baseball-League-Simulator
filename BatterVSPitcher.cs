using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterVSPitcher
{
    private Player batter;
    private Player pitcher;

    private int strikes;
    private int balls;

    private bool stilBatting;
    private int finalBatterScore;

    private bool groundBall;

    public BatterVSPitcher(Player batter, Player pitcher)
    {
        this.batter = batter;
        this.pitcher = pitcher;

        stilBatting = true;
        groundBall = false;

        strikes = 0;
        balls = 0;
    }


    //Public Functions
    public void ExecuteAtBat()
    {
        while(stilBatting)
        {
            int pitcherScore = pitcher.Def_Pitching(batter);
            bool strikeBox = pitcher.Def_ThrowStrike();
            int batterScore = batter.Off_Batting(pitcherScore, strikeBox);

            pitcher.GetCurrentGameStats().totalPitches += 1;

            //Debug.Log("P: " + pitcherScore + " B: " + batter.GetNameOfPlayer() + ": " + batterScore);

            if (batterScore == 0)
            {
                strikes += 1;
            }
            else if(batterScore == 1)
            {
                balls += 1;
            }else
            {
                //Batter has hit the ball
                finalBatterScore = batterScore; //The batter has hit the ball
                stilBatting = false;
                groundBall = Groundball();
                break;
            }

            if (IsBatterOut())
            {
                stilBatting = false;
                finalBatterScore = 0; //If 0 the batter has struck out
            }
            else if(DidBatterWalk())
            {
                stilBatting = false;
                finalBatterScore = 1; //If 1 the batter has walked

            }
        }

    }

    public bool GetIsGroundball() { return groundBall; }
    public int GetFinalBatterScore() { return finalBatterScore; }







    //Private Functions

    private int RandomNum(int num1, int num2)
    {
        return Random.Range(num1, num2 + 1);
    }

    private bool Groundball()
    {
        int randNum = RandomNum(0, 1);
        if (randNum == 1)
        {
            return true;
        }
        return false;
    }


    private bool IsBatterOut()
    {
        if (strikes == 3)
        {
            return true;
        }
        return false;
    }

    private bool DidBatterWalk()
    {
        if (balls == 4)
        {
            return true;
        }
        return false;
    }

}
