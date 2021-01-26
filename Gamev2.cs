using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamev2
{
    List<Player> defPositions;
    List<Player> offenseLineUp;
    private Team awayTeam;
    private Team homeTeam;

    public int team1OffensePos;
    public int team2OffensePos;
    public Player[] bases; //0 = batter, 1 = 1st, 2 = 2nd, 3 = 3rd, 4 = homeplate

    public Player currentBatter;

    public GameScoreCard gameScoreCard;
    public bool isGameOver;
    public bool isHalfInningOver;


    private bool turnOnDialogue;

    public Gamev2(Team awayTeam, Team homeTeam)
    {
        this.awayTeam = awayTeam;
        this.homeTeam = homeTeam;
        isGameOver = false;
        isHalfInningOver = false;

        gameScoreCard = new GameScoreCard(awayTeam, homeTeam);
        gameScoreCard = new GameScoreCard(awayTeam, homeTeam);
        team1OffensePos = 0;
        team2OffensePos = 0;

        CreateNewGameStats(awayTeam);
        CreateNewGameStats(homeTeam);

        offenseLineUp = awayTeam.GetStartingLineUp();
        defPositions = homeTeam.GetAllPlayers();


        SetStaminaPointsFromTeam(offenseLineUp, true);
        SetStaminaPointsFromTeam(defPositions, true);

        bases = new Player[] { null, null, null, null, null };
    }

    private void CreateNewGameStats(Team team)
    {
        List<Player> allPlayer = team.GetAllPlayers();
        for (int i = 0; i < allPlayer.Count; i++)
        {
            allPlayer[i].CreateNewCurrentGameStats(team.GetName());
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

    public bool PlayEntireGame()
    {
        if (isGameOver) { return false; }
        while (!isGameOver)
        {
            SingleAtBat();
        }


        return true;
    }
    public bool PlayHalfInning()
    {
        turnOnDialogue = true;

        if (isGameOver) { return false; }
        isHalfInningOver = false;
        while (!isHalfInningOver)
        {
            if (isGameOver)
            {
                break;
            }
            SingleAtBat();
        }

        return true;
    }

    private void SingleAtBat()
    {
        if (CheckGameStatus()) //Before next at bat check if game is over.
        {
            return;
        }
       
        if (!isGameOver)
        {
            if (gameScoreCard.halfInning)
            {
                BatterUp(offenseLineUp[team2OffensePos], defPositions[3]);

                team2OffensePos += 1;

                if (team2OffensePos >= offenseLineUp.Count)
                {
                    team2OffensePos = 0;
                }
            }
            else
            {
                BatterUp(offenseLineUp[team1OffensePos], defPositions[3]);

                team1OffensePos += 1;

                if (team1OffensePos >= offenseLineUp.Count)
                {
                    team1OffensePos = 0;
                }
            }
        }
    }

    private void BatterUp(Player batter, Player pitcher)
    {
        //Load In
        currentBatter = batter;
        currentBatter.GetCurrentGameStats().atBats += 1;
        bases[0] = batter;


        //Batter vs Pitcher
        //Debug.Log(" Batter Base Stat: " + batter.GetBatterScore());

        BatterVSPitcher theDuel = new BatterVSPitcher(batter, pitcher);
        theDuel.ExecuteAtBat();
        int batterScore = theDuel.GetFinalBatterScore();

        bool isGroundball = theDuel.GetIsGroundball();
        if (batterScore == 0) //STRIKEOUT
        {
            DialogueBox("Batter strikes out!");
            RemovePlayerFromBase(0);

            AddOut(1);
            currentBatter.GetCurrentGameStats().battingStrikeouts += 1;
            defPositions[4].GetCurrentGameStats().pitcherStrikeouts += 1;
            return;
        }
        else if(batterScore == 1) //WALKED
        {
            pitcher.GetCurrentGameStats().pitcherWalks += 1;
            MovePlayersToNextBase(1);
            return;
        }


        int hitLocation = batter.Off_HitLocation();
        Player playerFielder = defPositions[hitLocation - 1];

        //Is flyball
        if (!isGroundball)
        {
            DialogueBox("Flyball to " + playerFielder.GetPositionString() + ".");
            if (hitLocation < 7)
            {
                if (playerFielder.Def_CatchFlyBall(batterScore))
                {
                    //Made the catch
                    AddOut(1);
                    return;
                }
                else //Failed catch. Check if error.
                {
                    if (!playerFielder.Def_ErrorRoll())
                    {
                        //Error made
                        ErrorMade(playerFielder);
                    }
                    //Single hit made by batter
                    DialogueBox("Batter hit a single. All players advance");

                    MovePlayersToNextBase(1);
                    return;
                }
            }
            else //Hit to outfield
            {
                if (playerFielder.Def_CatchFlyBall(batterScore))
                {
                    //Made the catch
                    AddOut(1);
                    return;
                }
                else //Failed catch. Check if error.
                {
                    if (batter.Off_CheckForHomeRun())//Check Homerun
                    {
                        //HOMERUN!!!!
                        int howManyOnBase = CheckForAmountOfPlayersOnBases(bases) + 1;
                        DialogueBox("HOMERUN!!!!! " + howManyOnBase + " runs scored!");
                        currentBatter.GetCurrentGameStats().homeruns += 1;
                        //Move all players around the diamond

                        MovePlayersToNextBase(4);
                        return;
                    }
                    else //No Homerun 
                    {
                        bool errorMade = playerFielder.Def_ErrorRoll();
                        if (errorMade)
                        {
                            //Error made
                            ErrorMade(playerFielder);
                            if (batter.Off_CheckForTriple(playerFielder))
                            {
                                //Made Triple
                                DialogueBox("Hit to the outfield! IT'S A TRIPLE! All players advance 3 bases.");
                                currentBatter.GetCurrentGameStats().triples += 1;
                                MovePlayersToNextBase(3);
                                return;
                            }
                            else if (batter.Off_CheckForDouble(playerFielder, true))
                            {
                                //Made Double
                                DialogueBox("Hit to the outfield! IT'S A DOUBLEE! All players advance 2 bases.");
                                currentBatter.GetCurrentGameStats().doubles += 1;
                                MovePlayersToNextBase(2);
                                return;
                            }
                            else
                            {
                                //Made it to first base
                                DialogueBox("Hit to the outfield! It's a single! All players advance 1 bases.");
                                MovePlayersToNextBase(1);
                                return;
                            }

                        }
                        else
                        {
                            if (batter.Off_CheckForTriple(playerFielder))
                            {
                                //Made Triple
                                DialogueBox("Hit to the outfield! IT'S A TRIPLE! All players advance 3 bases.");
                                currentBatter.GetCurrentGameStats().triples += 1;
                                MovePlayersToNextBase(3);
                                return;
                            }
                            else if (batter.Off_CheckForDouble(playerFielder, false))
                            {
                                //Made Double
                                DialogueBox("Hit to the outfield! IT'S A DOUBLEE! All players advance 2 bases.");
                                currentBatter.GetCurrentGameStats().doubles += 1;
                                MovePlayersToNextBase(2);
                                return;
                            }
                            else
                            {
                                //Made it to first base
                                DialogueBox("Hit to the outfield! It's a single! All players advance 1 bases.");
                                MovePlayersToNextBase(1);
                                return;
                            }
                        }
                    }
                }
            }
        }
        else //Is Groundball
        {
            DialogueBox("Groundball to " + playerFielder.GetPositionString() + ".");

            if (!playerFielder.Def_FieldBall(batterScore))
            {
                bool errorMade = playerFielder.Def_ErrorRoll();
                if (errorMade)
                {
                    //Made error. All players adavance
                    ErrorMade(playerFielder);
                }
                DialogueBox("It's a single! All players advance 1 bases.");
                MovePlayersToNextBase(1);
            }
            else if (hitLocation >= 7) //Grounded to Outfield
            {
                DialogueBox("It's a single! All players advance 1 bases.");
                MovePlayersToNextBase(1);
            }
            else if (hitLocation < 7) //Grounded to infield
            {
                //1: C, 2: 3rd, 3: SS, 4: P, 5: 2nd, 6: 1st, 7: LF, 8: CF, 9: RF 
                Player thirdBase = defPositions[2 - 1];
                Player secondBase = defPositions[5 - 1];
                Player firstBase = defPositions[6 - 1];

                //If no outs and is triple play an option?
                if (gameScoreCard.outs == 0 && CheckForAmountOfPlayersOnBases(bases) >= 2)
                {
                    if (thirdBase.Def_MakePlayAtBase(batterScore) && secondBase.Def_MakePlayAtBase(batterScore) && firstBase.Def_MakePlayAtBase(batterScore))
                    {
                        DialogueBox("TRIPLE PLAY MADE!!");
                        AddOut(3);
                        
                    }
                    else
                    {
                        RemovePlayerFromBase(FurthestAlongPlayerOnBases());
                        
                        DialogueBox("Triple play opportunity but only one out made.");
                        AddOut(1);
                        MovePlayersToNextBase(1);
                    }

                }
                else if (gameScoreCard.outs <= 1 && CheckForAmountOfPlayersOnBases(bases) >= 1) //If 1 or less outs and is double play an option?
                {
                    if (secondBase.Def_MakePlayAtBase(batterScore) && firstBase.Def_MakePlayAtBase(batterScore))
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
                    }
                    else
                    {
                        DialogueBox("Double play opportunity but only one out made.");
                        RemovePlayerFromBase(FurthestAlongPlayerOnBases());
                        MovePlayersToNextBase(1);
                        AddOut(1);
                    }
                }
                //Else throw to first (Remove player that would be moving to first)
                else if (firstBase.Def_MakePlayAtBase(batterScore))
                {
                    DialogueBox("Batter out at first.");
                    RemovePlayerFromBase(0);
                    AddOut(1);
                }
                else
                {
                    DialogueBox("Batter made it to first base.");
                    MovePlayersToNextBase(1);

                }
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

    private bool CheckGameStatus()
    {
        if (gameScoreCard.outs == 3)
        {
            //Is the game over
            if (gameScoreCard.GetScore(awayTeam) != gameScoreCard.GetScore(homeTeam) && gameScoreCard.GetInningCounter() >= 9 && gameScoreCard.halfInning == true)
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
            return true;
        }
        return false;
    }

    private void EndGameCalculations()
    {
        if (gameScoreCard.GetScore(awayTeam) > gameScoreCard.GetScore(homeTeam))
        {
            awayTeam.GetAllPlayers()[3].GetCurrentGameStats().pitcherWins += 1;
            homeTeam.GetAllPlayers()[3].GetCurrentGameStats().pitcherLoses += 1;
            AddToWinStreakUpgradeCoins(awayTeam);
            awayTeam.GetCurrentSeasonTeamStats().wins += 1;
            homeTeam.GetCurrentSeasonTeamStats().loses += 1;


        }
        else
        {
            awayTeam.GetAllPlayers()[3].GetCurrentGameStats().pitcherLoses += 1;
            homeTeam.GetAllPlayers()[3].GetCurrentGameStats().pitcherWins += 1;
            AddToWinStreakUpgradeCoins(homeTeam);
            homeTeam.GetCurrentSeasonTeamStats().wins += 1;
            awayTeam.GetCurrentSeasonTeamStats().loses += 1;

        }
        awayTeam.GetCurrentSeasonTeamStats().totalGames += 1;
        homeTeam.GetCurrentSeasonTeamStats().totalGames += 1;

        SaveStatsPerPlayer(awayTeam);
        SaveStatsPerPlayer(homeTeam);

    }

    private void AddToWinStreakUpgradeCoins(Team team)
    {
        List<Player> allPlayers = team.GetAllPlayers();

        for (int i = 0; i < allPlayers.Count; i++)
        {
            allPlayers[i].CalculateNewUpgradeCoins();
        }
    }

    private void SaveStatsPerPlayer(Team team)
    {
        List<Player> allPlayers = team.GetAllPlayers();

        for (int i = 0; i < allPlayers.Count; i++)
        {
            allPlayers[i].CalculateNewXPFromStats(allPlayers[i].GetCurrentGameStats());
            allPlayers[i].AddStatsToThisSeason();
        }
    }

    private void EndHalfInning()
    {
        isHalfInningOver = true;
        if (gameScoreCard.halfInning == true)
        {

            gameScoreCard.halfInning = false;
            gameScoreCard.StartNewInning();
            offenseLineUp = awayTeam.GetAllPlayers();
            defPositions = homeTeam.GetAllPlayers();
            SetStaminaPointsFromTeam(offenseLineUp);
            SetStaminaPointsFromTeam(defPositions);
            DialogueBox("End of an inning!");
        }
        else
        {
            DialogueBox("End of the half.");
            gameScoreCard.halfInning = true;
            offenseLineUp = homeTeam.GetAllPlayers();
            defPositions = awayTeam.GetAllPlayers();
        }

        //Clear all bases
        bases = new Player[] { null, null, null, null, null };
        gameScoreCard.outs = 0;

    }

    private void MovePlayersToNextBase(int runHowManyTimes) //This also calculates score if a player is on the 5th base (homeplate).
    {
        DialogueBox("Runners move up " + runHowManyTimes + " bases.");
        for (int i = 0; i < runHowManyTimes; i++)
        {
            for (int m = 3; m >= 0; m--)
            {
                if (bases[m] != null)
                {
                    if (m == 0)
                    {
                        currentBatter.GetCurrentGameStats().hits += 1; //Add hits to the stat sheet.
                        gameScoreCard.AddHits(1, currentBatter.GetTeam());
                    }

                    bases[m + 1] = bases[m];
                    RemovePlayerFromBase(m);
                }
            }
            if (bases[4] != null) //Check for players on the home plate, if so, remove and add score.
            {
                AddScore(bases[4]);
                RemovePlayerFromBase(4);
            }
        }
    }

    private void AddScore(Player playerThatScored)
    {
        Stats playerStatCard = playerThatScored.GetCurrentGameStats();

        if (playerStatCard.teamName == gameScoreCard.t1.GetName()) //Add to runs stat on player
        {
            //Add to team 1 score.
            gameScoreCard.AddScore(1, awayTeam);
        }
        else
        {
            //Add to team 2 score.
            gameScoreCard.AddScore(1, homeTeam);
        }
        currentBatter.GetCurrentGameStats().RBI += 1;
        defPositions[4].GetCurrentGameStats().runsAllowed += 1;
        playerStatCard.runs += 1;
    }


    private void AddOut(int numOfOuts)
    { 
        DialogueBox(numOfOuts + " Outs Recorded.");
        gameScoreCard.outs += numOfOuts;
    }

    private void RemovePlayerFromBase(int whichBase)
    {
        bases[whichBase] = null;
    }

    private void ErrorMade(Player player)
    {
        DialogueBox("Error made by: " + player.GetPositionString());

        gameScoreCard.AddErrors(1, player.GetTeam());
        player.GetCurrentGameStats().error += 1;
    }




    private void DialogueBox(string dialogue)
    {
        if (turnOnDialogue)
        {
            Debug.Log(dialogue);
        }
    }

    public string HitBallTranslate(int num)
    {
        switch (num)
        {
            case 1:
                return ("Catcher");
            case 2:
                return ("3rd Base");
            case 3:
                return ("Shortstop");
            case 4:
                return ("Pitcher");
            case 5:
                return ("2nd Base");
            case 6:
                return ("1st Base");
            case 7:
                return ("Left Field");
            case 8:
                return ("Center Field");
            case 9:
                return ("Right Field");
            case 10:
                return ("Left Field Homerun!");
            case 11:
                return ("Center Field Homerun!");
            case 12:
                return ("Right Field Homerun!");
            default:
                return ("Error");

        }

    }
}