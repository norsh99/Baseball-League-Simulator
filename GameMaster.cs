using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public string team1Name;
    public string team2Name;

    List<Player> defPositions;
    List<Player> offenseLineUp;
    Team team1;
    Team team2;

    public int team1OffensePos;
    public int team2OffensePos;
    public Player[] bases; //0 = batter, 1 = 1st, 2 = 2nd, 3 = 3rd, 4 = homeplate

    public Player currentBatter;

    public GameScoreCard gameScoreCard;
    public bool isGameOver;



    //UI
    public TextMeshProUGUI scorebox1;
    public TextMeshProUGUI scorebox2;
    public TextMeshProUGUI inningTextBox;
    public TextMeshProUGUI outsTextBox;
    public List<Image> diamondUI;
    public List<TextMeshProUGUI> diamondTextBoxes;
    public Color onBaseColor;
    public Color defaultColor;

    public PlayerTracker trackerTeam1;
    public PlayerTracker trackerTeam2;

    private bool turnOnDialogue;



    void Start()
    {

        //LoadInNewGame();

    }

    public void LoadInNewGame(Team t1, Team t2)
    {
        //Setup New Game Settings
        team1Name = t1.GetName();
        team2Name = t2.GetName();
        isGameOver = false;

        gameScoreCard = new GameScoreCard(team1, team2);
        scorebox1.text = gameScoreCard.t1 + ": " + 0;
        scorebox2.text = gameScoreCard.t2 + ": " + 0;
        inningTextBox.text = "Inning: " + gameScoreCard.GetInningCounter();
        team1OffensePos = 0;
        team2OffensePos = 0;

        //Load in teams

        //team1 = TempBuildTeams(team1Name);
        //team2 = TempBuildTeams(team2Name);
        team1 = t1;
        team2 = t2;
        CreateNewGameStats(t1);
        CreateNewGameStats(t2);



        offenseLineUp = team1.GetAllPlayers();
        defPositions = team2.GetAllPlayers();
        SetStaminaPointsFromTeam(offenseLineUp, true);
        SetStaminaPointsFromTeam(defPositions, true);



        bases = new Player[] { null, null, null, null, null };


        trackerTeam1.LoadInfo(team1Name ,team1.GetAllPlayers()); //Temp 
        trackerTeam2.LoadInfo(team2Name, team2.GetAllPlayers()); //Temp 


        //Execute game loop
        GameLoop();
    }

    //0 = Strikeout, 1 = Catcher, 2 = 3rd, 3 = Shortstop, 4 = Pitcher, 5 = 2nd, 6 = 1st,
    //7 = Left Field, 8 = Center Field, 9 = Right Field, 10 = Left HR, 11 = Center HR, 12 = Right HR.
    public int BatterVsPitcher(Player batter, Player pitcher)  
    {
        currentBatter = batter;
        currentBatter.GetCurrentGameStats().atBats += 1;
        bases[0] = batter;
        int batterBonusStat = 0;
        int pitcherBonusStat = 0;


        if (batter.GetCurrentGameStats().atBats > batter.GetStaminaScore()) //I'm comparing to the stamina score on purpose as at bats will increase to compare.
        {
            batterBonusStat -= 1;
        }

        if (pitcher.GetCurrentStamina() <= 0) { pitcherBonusStat -= 1; }
        if (pitcher.GetCurrentStamina() <= -1) { pitcherBonusStat -= 1; }


        int playerBat = RandomNum(0, batter.GetBatterScore()) + batterBonusStat;
        int pitcherPitch = RandomNum(0, pitcher.GetPitcherScore()) + pitcherBonusStat;
        //Debug.Log("Batter Score: " + playerBat + " | Pitcher Score: " + pitcherPitch);
        if (playerBat >= pitcherPitch)
        {
            int batBonus = 0;
            if (batter.GetCurrentStamina() > RandomNum(0, 10)) { batBonus += 1; }
            if(playerBat - pitcherPitch > 2) { batBonus += 1; }
            if (playerBat - pitcherPitch > 3) { batBonus += 1; }
            if (playerBat - pitcherPitch > 4) { batBonus += 1; }


            int result = RandomNum(1, 12);
            result = result + batBonus;
            if (result > 12)
            {
                result = 12;
            }
            return result;
        }
        return 0;

    }
    private void CreateNewGameStats(Team team)
    {
        List<Player> allPlayer = team.GetAllPlayers();
        for (int i = 0; i < allPlayer.Count; i++)
        {
            allPlayer[i].CreateNewCurrentGameStats(team.GetName());
        }
    }

    private int CheckStamina(Player player)
    {
        int staminaStat = 0;
        if (player.GetCurrentStamina() <= 0)
        {
            staminaStat -= 1;
        }
        return staminaStat;
    }

    private int CheckSpeedBonus(Player player)
    {
        int speedStat = 0;
        if (player.GetRunningSpeed() >= RandomNum(0, 10))
        {
            speedStat += 1;
        }
        return speedStat;
    }

    private bool FielderMakesPlay(Player fielder)
    {
        int bonusStat = 0;
        bonusStat += CheckStamina(fielder);
        bonusStat += CheckSpeedBonus(fielder);

        if (fielder.GetFielderScore() + bonusStat >= RandomNum(0, 10))
        {
            return true;
        }
        return false;
    }

    private void DialogueBox(string dialogue)
    {
        if (turnOnDialogue)
        {
            Debug.Log(dialogue);
        }
    }

 
    private void BatterUp(Player batter, Player pitcher)
    {
        int atBatResult = BatterVsPitcher(batter, pitcher);
        if (atBatResult > 0 && atBatResult < 10)
        {
            Player playerFielder = defPositions[atBatResult - 1];
            int flyBall = RandomNum(0, 1); //50/50 chance its a flyball. Default

            bool fielderCaughtBall = false;
            bool fielderFieldsBall = false;
            if (flyBall == 1) //If 1 that means it's a flyball.
            {
                fielderCaughtBall = FielderMakesPlay(playerFielder);
                DialogueBox("Flyball to " + HitBallTranslate(atBatResult) + ".");
            } else
            {
                fielderFieldsBall = FielderMakesPlay(playerFielder);
                DialogueBox("Groundball to " + HitBallTranslate(atBatResult) + ".");
            }

            if (fielderCaughtBall)
            {
                //Add to the Outs
                AddOut(1);
                RemovePlayerFromBase(0);
                DialogueBox(HitBallTranslate(atBatResult) + " catches the ball for an out.");
            }
            else if(fielderFieldsBall) {
                //Checking to make sure these are infield players fielding the ball
                if (atBatResult < 7)
                {
                    //If no outs and is triple play an option?
                    if (gameScoreCard.outs == 0 && CheckForAmountOfPlayersOnBases(bases) >= 2)
                    {
                        if (RandomNum(0,3) == 3) //I could impliment checks for each base to see if each baseman makes the play in the future. 
                        {
                            AddOut(3);
                            DialogueBox("TRIPLE PLAY MADE!!");
                        }
                        else
                        {
                            RemovePlayerFromBase(FurthestAlongPlayerOnBases());
                            AddOut(1);
                            DialogueBox("Triple play opportunity but only one out made.");
                            MovePlayersToNextBase(1);
                        }
                    }
                    
                    else if (gameScoreCard.outs <= 1 && CheckForAmountOfPlayersOnBases(bases) >= 1) //If 1 or less outs and is double play an option?
                    {
                        if (RandomNum(0, 2) == 2)
                        {
                            DialogueBox("DOUBLE PLAY MADE!!");
                            RemovePlayerFromBase(FurthestAlongPlayerOnBases());
                            RemovePlayerFromBase(FurthestAlongPlayerOnBases());
                            AddOut(2);
                            if (gameScoreCard.outs < 3 && CheckForAmountOfPlayersOnBases(bases) == 1)
                            {
                                DialogueBox("Batter makes it to first.");
                                MovePlayersToNextBase(1);
                            }
                        } else
                        {
                            DialogueBox("Double play opportunity but only one out made.");
                            RemovePlayerFromBase(FurthestAlongPlayerOnBases());
                            MovePlayersToNextBase(1);
                            AddOut(1);
                        }
                    }

                    //Else throw to first (Remove player that would be moving to first)
                    else if (defPositions[6].GetFielderScore() >= RandomNum(0, 10) + CheckSpeedBonus(currentBatter))
                    {
                        DialogueBox("Batter out at first.");
                        RemovePlayerFromBase(0);
                        AddOut(1);
                    }
                    else if(RandomNum(0,1) == 1)
                    {
                        DialogueBox("Batter makes it to first.");
                        MovePlayersToNextBase(1);
                    }
                    else
                    {
                        //Error was made
                        DialogueBox("Error made by first base, batter makes it to first.");
                        defPositions[6].GetCurrentGameStats().error += 1;
                        MovePlayersToNextBase(1);
                    }
                } 
                else if(atBatResult < 10) //If Outfield players field the ball
                {
                    if (RandomNum(0, 3) + CheckSpeedBonus(defPositions[atBatResult - 1]) <= 1 && currentBatter.GetCurrentStamina() >= RandomNum(0,10)) //TRIPLE
                    {
                        DialogueBox("Hit to the outfield! IT'S A TRIPLE! All players advance 3 bases.");
                        currentBatter.GetCurrentGameStats().triples += 1;
                        MovePlayersToNextBase(3);

                    }
                    else if(RandomNum(0, 2) + CheckSpeedBonus(defPositions[atBatResult - 1]) <= 1 && currentBatter.GetCurrentStamina() >= RandomNum(0, 10)) //DOUBLE
                    {
                        DialogueBox("Hit to the outfield! It's a double! All players advance 2 bases.");
                        currentBatter.GetCurrentGameStats().doubles += 1;
                        MovePlayersToNextBase(2);
                    }
                    else //SINGLE
                    {
                        DialogueBox("Hit to the outfield! It's a single. All players advance.");
                        MovePlayersToNextBase(1);
                    }
                    
                }
            }
            else
            {
                //Error made on defense
                DialogueBox("Error made by " + HitBallTranslate(atBatResult) + ". All Players advance.");
                playerFielder.GetCurrentGameStats().error += 1;

                //All baserunners move up 1;
                MovePlayersToNextBase(1);
            }
        }
        else if(atBatResult >= 10) //Hit Homerun
        {
            int howManyOnBase = CheckForAmountOfPlayersOnBases(bases) + 1;
            DialogueBox("HOMERUN!!!!! " + howManyOnBase + " runs scored!");
            currentBatter.GetCurrentGameStats().homeruns += 1;
            //Move all players around the diamond
            
            MovePlayersToNextBase(4);

        }
        else //Batter just struck out
        {
            //Batter is out. Add to the Outs in the Inning
            DialogueBox("Batter strikes out!");
            RemovePlayerFromBase(0);
            
            AddOut(1);
            currentBatter.GetCurrentGameStats().battingStrikeouts += 1;
            defPositions[4].GetCurrentGameStats().pitcherStrikeouts += 1;
        }
        CheckGameStatus();
    }

    private void SetDiamondColor(int baseNumber, bool onBase)
    {
        if (baseNumber > 0)
        {
            if (onBase)
            {
                diamondUI[baseNumber - 1].color = onBaseColor;
            }
            else
            {
                diamondUI[baseNumber - 1].color = defaultColor;
            }
        }
    }

    

    private int FurthestAlongPlayerOnBases()
    {
        for (int m = bases.Length - 2; m <= 0; m--)
        {
            if (bases[m] != null)
            {
                return m;
            }

        }
         return 0;
    }

    private void RemovePlayerFromBase(int whichBase)
    {
        bases[whichBase] = null;
        SetDiamondColor(whichBase, false);
        if (whichBase != 0)
        {
            diamondTextBoxes[whichBase].text = "";
        }
        
    }
    private void AddOut(int numOfOuts)
    {
        gameScoreCard.outs += numOfOuts;
        outsTextBox.text = "Outs: " + gameScoreCard.outs;
    }

    private void MovePlayersToNextBase(int runHowManyTimes) //This also calculates score if a player is on the 5th base (homeplate).
    {
        for (int i = 0; i < runHowManyTimes; i++)
        {
            for (int m = 3; m >= 0; m--)
            {
                if (bases[m] != null)
                {
                    if (m == 0)
                    {
                        currentBatter.GetCurrentGameStats().hits += 1; //Add hits to the stat sheet.
                    }

                    bases[m + 1] = bases[m];
                    SetDiamondColor(m + 1, true);
                    diamondTextBoxes[m + 1].text = bases[m].GetNameOfPlayer();
                    RemovePlayerFromBase(m);
                }
            }
            if (bases[4] != null) //Check for players on the home plate, if so, remove and add score.
            {
                AddScore(bases[4]);
                diamondTextBoxes[4].text = bases[4].GetNameOfPlayer();
                RemovePlayerFromBase(4);
                SetDiamondColor(4, false);
            }
        }
    }

    private void AddScore(Player playerThatScored)
    {
        Stats playerStatCard = playerThatScored.GetCurrentGameStats();

        if (playerStatCard.teamName == gameScoreCard.t1.ToString()) //Add to runs stat on player
        {
            //Add to team 1 score.
            gameScoreCard.AddScore(1, team1);
            scorebox1.text = gameScoreCard.t1 + ": " + gameScoreCard.GetScore(team1).ToString();
        }
        else
        {
            //Add to team 2 score.
            gameScoreCard.AddScore(1, team2);
            scorebox2.text = gameScoreCard.t2 + ": " + gameScoreCard.GetScore(team2).ToString();
        }
        currentBatter.GetCurrentGameStats().RBI += 1;
        defPositions[4].GetCurrentGameStats().runsAllowed += 1;
        playerStatCard.runs += 1;
    }

    private int CheckForAmountOfPlayersOnBases(Player[] basesCheck)
    {
        int count = 0;
        for (int i = 1; i < 4; i++)
        {
            if (basesCheck[i] != null)
            {
                count += 1;
            }
        }
        return count;
    }

    private void CheckGameStatus()
    {
        if (gameScoreCard.outs == 3)
        {
            //Is the game over
            if (gameScoreCard.GetScore(team1) != gameScoreCard.GetScore(team2) && gameScoreCard.GetInningCounter() >= 9 && gameScoreCard.halfInning == true)
            {
                //The game is over
                DialogueBox("!!!!!!!!!!!GAME OVER!!!!!!!!!!");
                isGameOver = true;
                EndGameCalculations();
            }
            else
            {
                EndHalfInning();
            }
        } 
    }

    private void SetStaminaPointsFromTeam(List<Player> team, bool resetStamina = false)
    {
        for (int i = 0; i < team.Count; i++)
        {
            if (resetStamina)
            {
                team[i].SetCurrentStamina(true); //Resets the stat for the start of the beginning of the game.
            }
            else
            {
                team[i].SetCurrentStamina(); //Removes one point fromt the current stamina stat.
            }
        }
    }

    private void EndHalfInning()
    {
        if (gameScoreCard.halfInning == true)
        {
            gameScoreCard.halfInning = false;
            gameScoreCard.StartNewInning();
            inningTextBox.text = "Inning: " + gameScoreCard.GetInningCounter();
            offenseLineUp = team1.GetAllPlayers();
            defPositions = team2.GetAllPlayers();
            SetStaminaPointsFromTeam(offenseLineUp);
            SetStaminaPointsFromTeam(defPositions);
            DialogueBox("End of an inning!");
        }
        else
        {
            DialogueBox("End of the half.");
            gameScoreCard.halfInning = true;
            offenseLineUp = team2.GetAllPlayers();
            defPositions = team1.GetAllPlayers();
        }

        //Clear all bases
        bases = new Player[] { null, null, null, null, null };
        gameScoreCard.outs = 0;
        outsTextBox.text = "Outs: " + gameScoreCard.outs;


        //Start Batter Loop
    }

    private void EndGameCalculations()
    {
        SaveStatsPerPlayer(team1);
        SaveStatsPerPlayer(team2);

    }

    private void SaveStatsPerPlayer(Team team)
    {
        List<Player> allPlayers = team.GetAllPlayers();

        for (int i = 0; i < allPlayers.Count; i++)
        {
            allPlayers[i].AddStatsToThisSeason();
        }
    }

    private void GameLoop(bool dialogueOn = false) //Call this to simulate a whole game. Must Load in teams first.
    {
        turnOnDialogue = dialogueOn;


        //BatterUp Function
        while (!isGameOver)
        {
            BatterUpButton();
        }

    }

    private int RandomNum(int num1, int num2)
    {
        return Random.Range(num1, num2+1);
    }


    public string HitBallTranslate(int num)
    {
        switch (num)
        {
            case 0:
                return("Strikeout!");
            case 1:
                return("Catcher");
            case 2:
                return("3rd Base");
            case 3:
                return("Shortstop");
            case 4:
                return("Pitcher");
            case 5:
                return("2nd Base");
            case 6:
                return("1st Base");
            case 7:
                return("Left Field");
            case 8:
                return("Center Field");
            case 9:
                return("Right Field");
            case 10:
                return("Left Field Homerun!");
            case 11:
                return("Center Field Homerun!");
            case 12:
                return("Right Field Homerun!");
            default:
                return ("Error");

        }


        
    }







    //BUTTONS ------------------------------------------------------------------------------------
    public void BatterUpButton()
    {
        if (gameScoreCard.halfInning)
        {
            BatterUp(offenseLineUp[team2OffensePos], defPositions[4]);

            team2OffensePos += 1;

            if (team2OffensePos >= offenseLineUp.Count)
            {
                team2OffensePos = 0;
            }
        }
        else
        {
            BatterUp(offenseLineUp[team1OffensePos], defPositions[4]);

            team1OffensePos += 1;

            if (team1OffensePos >= offenseLineUp.Count)
            {
                team1OffensePos = 0;
            }
        }
    }
    public void ExecuteGameLoopButton()
    {
        GameLoop();
    }

}
