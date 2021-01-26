using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Player(string playerName, Team teamName)
    {
        

        if (playerName == null)
        {
            name = "Rand Name " + RandomNum(0, 5000);
        }else
        {
            name = playerName;
        }
        
        CreateNewPlayer(teamName);
    }
    //Team
    private Team team;

    //Player Rating
    private string name;
    private float playerBaseStatsRating;
    private int playerPerformanceRating;


    //Base Scores
    private int batterScore; //The higher the better for collecting more home runs;
    private int fielderScore; //The highter the score the better for not making mistakes;
    private int pitcherScore; //The higher the score the better you will be against har hitting batters;
    private int runningSpeed; //A bonus stat to help the player perform better;
    private int staminaScore; //How long in a game a player can last before recieving a negative bonues stat when playing the game;
    private int athletics; //How quickly a player will train a grow to new levels;
    private int position; //1: C, 2: 3rd, 3: SS, 4: P, 5: 2nd, 6: 1st, 7: LF, 8: CF, 9: RF 
    private int battingStance;


    //Value
    private float value; //How much the player is valued
    private float boughtValue; //The value of the player when you bought him.

    //Stats
    List<Stats> SeasonStats;
    List<Stats> AllTimeStats;
    private Stats thisSeason;

    //Upgrades
    private int maxBaseScore; //The max base score for all the stats. The more the better so the player has room to grow. You get more each level increase.
    private int level; //Each level gives a free stat point when playing the game;
    private int xp; //Collect experience points to get you to the next level; By completing important plays in a game the xp will be increased. By winning games the xp will be incrased.
    private int maxXP; //The needed max xp in order to increase the level of the player. 
    private int minRange; //When a player upgrades in each he gets a new min range. Example: 1=0, 2=5, 3=10, 5=15. This will be used when evaluating defense or offense comparaisons only. Not for bar graph info.
    private int latestGameXP;
    private int winStreak;
    private int upgradeCoins;
    private int latestUpgradeCoins;

    private int prevLevel; //All previous variable store what happened before getting upgraded. This is important for animating stats and xp on the recap screen after a game is completed.
    private int prevXP;
    private int prevMinRange;
    private int prevMaxXp;




    //Current Game Stats
    private Stats currentGameStats;
    private int currentStamina;

    public void CreateNewPlayer(Team teamName)
    {

        //Establish Variables
        SeasonStats = new List<Stats>();
        AllTimeStats = new List<Stats>();
        thisSeason = new Stats(null);

        //Upgrades
        maxBaseScore = 10;
        minRange = 0;
        xp = RandomNum(0, 300);
        maxXP = 2000;
        level = 1;
        if (RandomNum(0, 10) >= 8) { UpgradeLevel(); }
        winStreak = 0;
        upgradeCoins = RandomNum(4, 10); //FOR TESTING, otherwise set to 0
        latestUpgradeCoins = 0;




        //Team
        team = teamName;

        //Base Scores
        batterScore = RandomNum(0,maxBaseScore);
        fielderScore = RandomNum(0, maxBaseScore);
        pitcherScore = RandomNum(0, maxBaseScore);
        runningSpeed = RandomNum(0, maxBaseScore);
        staminaScore = RandomNum(0, maxBaseScore);
        
        battingStance = 0;
        if (RandomNum(0,10) >= 8){ battingStance = 1; }


        //Value
        value = 0;
        boughtValue = 0;

        athletics = RandomNum(0, 10); //Doesn't get a maxBaseScore stat comparaison. You are either good at baseball or not. 
        position = RandomNum(1, 9);


        //Player Rating
        CalculateBaseStatsRating();

        playerPerformanceRating = 0;
    }

    public void NewSeasonStatsReset()
    {
        SeasonStats.Add(thisSeason);
        thisSeason = new Stats(null);

    }
    public int CalculateNewUpgradeCoins()
    {
        winStreak += 1;
        latestUpgradeCoins = 0;
        if (level == 1 && winStreak == 6)
        {
            upgradeCoins += 1;
            latestUpgradeCoins += 1;
            winStreak = 0;
            return 1;
        }
        else if (level == 2 && winStreak == 5)
        {
            upgradeCoins += 1;
            latestUpgradeCoins += 1;

            winStreak = 0;
            return 1;
        }
        else if (level == 3 && winStreak == 4)
        {
            upgradeCoins += 1;
            latestUpgradeCoins += 1;

            winStreak = 0;
            return 1;
        }
        else if (level == 4 && winStreak == 3)
        {
            upgradeCoins += 1;
            latestUpgradeCoins += 1;

            winStreak = 0;
            return 1;
        }
        return 0;
    }

    //GETS CALCULATED AFTER EVERYGAME
    public int CalculateNewXPFromStats(Stats stats)
    {
        prevLevel = level;
        prevMaxXp = GetMaxXP();
        prevMinRange = 0;
        prevXP = GetXP();


        int totalXP = 0;
        //Off
        totalXP += stats.hits * 5;
        totalXP += stats.homeruns * 50;
        totalXP += stats.runs * 10;
        totalXP += stats.doubles * 20;
        totalXP += stats.triples * 30;
        totalXP += stats.RBI * 10;

        //Def
        totalXP += stats.pitcherStrikeouts * 5;
        totalXP += stats.pitcherWins * 50;

        latestGameXP = totalXP;

        SetXP(totalXP);
        CalculateValue();
        return totalXP;
    }
    public int[] GetPrevStats() { int[] allPrevStats = new int[] {prevLevel, prevMaxXp, prevMinRange, prevXP }; return allPrevStats; }


    public float CalculateBaseStatsRating()
    {
        playerBaseStatsRating = ((batterScore + fielderScore + pitcherScore + runningSpeed + staminaScore) / (maxBaseScore * 5f)) * 100f;
        return playerBaseStatsRating;
    }

    public void UpgradeLevel()
    {
        if (level <= 3)
        {
            level += 1;
        }
        
        if (level == 2)
        {
            maxBaseScore = 20;
            maxXP = 3000;
            xp = 0;
            minRange = 5;
        }
        if (level == 3)
        {
            maxBaseScore = 30;
            maxXP = 4000;
            xp = 0;
            minRange = 10;


        }
        if (level == 4)
        {
            maxBaseScore = 40;
            maxXP = 6000;
            xp = 0;
            minRange = 15;

        }
    }

    private int RandomNum(int num1, int num2)
    {
        return Random.Range(num1, num2+1);
    }

    public void CreateNewCurrentGameStats(string teamName)
    {
        currentGameStats = new Stats(teamName);
    }
        
    public void SetPosition(int newPosition) { position = newPosition; }
    public void SetTeam(Team newTeam) { team = newTeam; }
    public Team GetTeam() { return team; }

    public int GetPitcherScore() { return pitcherScore; }
    public int GetBatterScore() { return batterScore; }
    public int GetFielderScore() { return fielderScore; }
    public string GetNameOfPlayer() { return name; }
    public int GetRunningSpeed() { return runningSpeed; }
    public int GetStaminaScore() { return staminaScore; }
    public int GetCurrentStamina() { return currentStamina; }
    public void SetCurrentStamina(bool resetStamina = false) 
    {
        if (resetStamina)
        {
            currentStamina = staminaScore;
        }
        else
        {
            currentStamina -= 1;
        }
    }

    public void SetXP(int xpPoints)
    {
        xp = xp + xpPoints;
        CalculateLevelFromXP();
    }
    public int GetXP() { return xp; }
    public int GetMaxXP() { return maxXP; }
    public int GetLatestGameXP() { return latestGameXP; }
    public int GetLatestUpgradeCoins() { return latestUpgradeCoins; }


    public int GetMaxBaseScore() { return maxBaseScore; }


    public float GetPlayerBaseStatsRating() { CalculateBaseStatsRating(); CalculateValue(); return playerBaseStatsRating; }

    private void CalculateLevelFromXP()
    {
        if (xp >= maxXP)
        {
            UpgradeLevel();
        }
    }
    public int GetLevelStat() { return level; }
    public int GetAthleticsStat() { return athletics; }
    public float GetValue()
    {
        CalculateValue();
        return value;
    }
    public int GetPosition(){ return position; }
    public string GetPositionString()
    {
        switch(position)
        {
            default:
                return "Error";
            case 1:
                return "C";
            case 2:
                return "3rd";
            case 3:
                return "SS";
            case 4:
                return "P";
            case 5:
                return "2nd";
            case 6:
                return "1st";
            case 7:
                return "LF";
            case 8:
                return "CF";
            case 9:
                return "RF";
        }
    }

    public int GetBatterStance() { return battingStance; } //0 = Right handed , 1 = Left handed;
    public void CalculateValue()
    {
        int[] lvlPayMax = new int[] { 10000, 50000, 100000, 5000000 };
        float newAthleticsNum = athletics;
        if (athletics <= 0f)
        {
            newAthleticsNum = 1f;
        }

        float newPayThreshold = lvlPayMax[level-1] * ((float)newAthleticsNum / 10f);
        float newValue = newPayThreshold * ((float)CalculateBaseStatsRating() / 100f);


        value = newValue;
    }

    public Stats GetCurrentGameStats() { return currentGameStats; }

    public void AddStatsToThisSeason()
    {
        thisSeason = CombineStats(thisSeason, currentGameStats);
    }

    public Stats GetSeasonStats() { return thisSeason; }

    private Stats CombineStats(Stats stat1, Stats stat2)
    {
        Stats newStat = new Stats(null);
        newStat.atBats = stat1.atBats + stat2.atBats;
        newStat.hits = stat1.hits + stat2.hits;
        newStat.homeruns = stat1.homeruns + stat2.homeruns;
        newStat.runs = stat1.runs + stat2.runs;
        newStat.doubles = stat1.doubles + stat2.doubles;
        newStat.triples = stat1.triples + stat2.triples;
        newStat.RBI = stat1.RBI + stat2.RBI;
        newStat.battingStrikeouts = stat1.battingStrikeouts + stat2.battingStrikeouts;

        return newStat;
    }

    public float GetBoughtValue() { return boughtValue; }
    public void SetBoughtValue(float newValue) { boughtValue = newValue; }



    //UPGRADE FUNCTION CALLS ----------------------------------------------------------------------------------------
    //Base Scores

    //batterScore
    //fielderScore
    //pitcherScore
    //runningSpeed
    //staminaScore

    public int GetUpgradeCoins() { return upgradeCoins; }
    public void SpendUpgradeCoins(int amount) { upgradeCoins -= amount; }

    public bool CheckCanPurchaseStat_Batting(int purchasePoints)
    {
        if (batterScore + purchasePoints <= maxBaseScore)
        {
            return true;
        }
        return false;
    }
    public void AddStat_Batting(int amount) { batterScore += amount; }

    public bool CheckCanPurchaseStat_Fielding(int purchasePoints)
    {
        if (fielderScore + purchasePoints <= maxBaseScore)
        {
            return true;
        }
        return false;
    }
    public void AddStat_Fielding(int amount) { fielderScore += amount; }

    public bool CheckCanPurchaseStat_Pitching(int purchasePoints)
    {
        if (pitcherScore + purchasePoints <= maxBaseScore)
        {
            return true;
        }
        return false;
    }
    public void AddStat_Pitching(int amount) { pitcherScore += amount; }

    public bool CheckCanPurchaseStat_Running(int purchasePoints)
    {
        if (runningSpeed + purchasePoints <= maxBaseScore)
        {
            return true;
        }
        return false;
    }
    public void AddStat_Running(int amount) { runningSpeed += amount; }

    public bool CheckCanPurchaseStat_Stamina(int purchasePoints)
    {
        if (staminaScore + purchasePoints <= maxBaseScore)
        {
            return true;
        }
        return false;
    }
    public void AddStat_Stamina(int amount) { staminaScore += amount; }










    //Defense and Offense Comparaison ---------------------------------------------------------------------------------
    public bool Def_FieldBall(int difficulty)
    {
        int randomEffect = RandomNum(0, 3 * level);
        int defRoll = RandomNum(minRange, fielderScore);
        int speedRoll = RandomNum(minRange, runningSpeed);
        int totalDef = defRoll + speedRoll;

        if (totalDef > difficulty + randomEffect)
        {
            return true;
        }else
        {
            return false;
        }
    }

    public bool Def_ErrorRoll() //True mean no error committed. Ball was hit to open space.
    {
        if (RandomNum(0,maxXP) > fielderScore)
        {
            return true;
        }
        return false;
    }

    public bool Def_MakePlayAtBase(int difficulty)
    {
        int defRoll = RandomNum(minRange, fielderScore);
        int bonusHelp = RandomNum(0, 3 * level);
        int totalDef = defRoll + bonusHelp;

        if (totalDef > difficulty)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Def_CatchFlyBall(int difficulty)
    {
        if (!Def_FieldBall(difficulty))
        {
            if (RandomNum(0,10) >= 5)
            {
                return true; //Failed the field score, however, extra 50/50 shot of making the catch.
            }
        }else
        {
            return true; //Succeeded the field score.
        }
        return false; //Failed everything.
    }

    public int Def_Pitching(Player batter)
    {
        int pitchPower = RandomNum(minRange, pitcherScore);
        int pitchBonus = 0;

        if (batter.GetBatterStance() == battingStance)
        {
            pitchBonus = RandomNum(0, 5 * level);
        }

        if (currentStamina > 0)
        {
            pitchBonus = RandomNum(0, 5);
        }
        return pitchPower + pitchBonus;
    }
    public bool Def_ThrowStrike()
    {
        int randNum = RandomNum(0,1);
        if (randNum == 1)
        {
            return true;
        }else
        {
            return false;
        }
    }
    public int Off_Batting(int difficulty, bool isInStrikeBox)
    {
        int takeSwing = RandomNum(0, 1); //50/50 shot of swinging.
        //if (currentStamina <= 0 && takeSwing == 1 && !isInStrikeBox)
        //{
            //return 0;//0 = recieve strike. Player is drained and swinging at crap. 
        //}

        int batBonus = 0;

        if (currentStamina > staminaScore)
        {
            batBonus = RandomNum(0, 5);
        }
        else if(currentStamina > 0)
        {
            batBonus = RandomNum(0, currentStamina);
        }

        if (takeSwing == 1)
        {
            int battingScore = RandomNum(minRange, batterScore);
            //Debug.Log("P: " + difficulty + " B: " + GetNameOfPlayer() + ": " + battingScore);

            if (battingScore > difficulty)
            {
                int newBattingScore = 0;
                if (isInStrikeBox)
                {
                    newBattingScore = (batterScore - difficulty) * 2; //Double power when crushing a hit in the strike box
                }else
                {
                    newBattingScore = batterScore - difficulty;
                }
                return newBattingScore + batBonus; //This score is the power difficulty
            }else
            {
                return 0; //0 = recieve strike
            }
        }
        else if(isInStrikeBox)
        {
            return 0; //0 = recieve strike
        }
        else
        {
            return 1; //1 = recieve ball
        }
    }

    public bool Off_CheckForDouble(Player defender, bool errorMade)
    {
        if (errorMade)
        {
            if (RandomNum(minRange, runningSpeed) > RandomNum(0, defender.GetRunningSpeed()-5))
            {
                return true;
            }
            return false;
        }
        else 
        {
            if (RandomNum(minRange, runningSpeed) > RandomNum(0, defender.GetRunningSpeed()))
            {
                return true;
            }
            return false;
        }
    }
    public bool Off_CheckForTriple(Player defender)
    {
        if (RandomNum(minRange, runningSpeed - 1) > RandomNum(0, defender.GetRunningSpeed()))
        {
            return true;
        }
        return false;
    }

    public bool Off_CheckForHomeRun()
    {
        if (RandomNum(0,batterScore) > 5 * level)
        {
            return true;
        }else
        {
            if (RandomNum(0,10) >= 7)
            {
                return true; //Last 1/3 chance to hit homerun
            }
            return false;
        }
    }


    public int Off_HitLocation()
    {
        return RandomNum(2, 9); //1: C, 2: 3rd, 3: SS, 4: P, 5: 2nd, 6: 1st, 7: LF, 8: CF, 9: RF 
    }
}
