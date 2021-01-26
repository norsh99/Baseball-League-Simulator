using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

public class PlayGameScreen : MonoBehaviour
{

    public Scoreboard scoreboard;
    public HomeCode homeCode;

    public Team awayTeam;
    public Team homeTeam;

    public Gamev2 currentGame;

    public GameObject startGameGameObject;
    public GameObject leaveGameGameObject;
    public TextMeshProUGUI attendanceTextBox;

    private bool triggerGame;
    private bool topInning;
    private int timer;
    private int animateAttendance;
    private int tempAttendanceNum;
    private int finalAttendance;
    private int maxAttendance;


    public void LoadIn(Team awayT, Team homeT)
    {
        attendanceTextBox.text = "0";
        triggerGame = false;
        topInning = true;
        timer = 0;
        animateAttendance = 0;
        tempAttendanceNum = 0;
        finalAttendance = homeCode.user.DetermineAttendance();
        maxAttendance = homeCode.user.GetCurrentStadium().maxAttendance;
        homeCode.currentSeason.AddToTotalVisitors(finalAttendance);
        homeCode.currentSeason.AddToTotalTicketSales(homeCode.user.GetTicketPrice() * finalAttendance);


        awayTeam = awayT;
        homeTeam = homeT;

        currentGame = new Gamev2(awayTeam, homeTeam);

        scoreboard.GameLauncher(awayTeam.GetName(), homeTeam.GetName());
    }

    private void Update()
    {
        PlayGame();
        SimulateAttendanceStat();
    }

    private void SimulateAttendanceStat()
    {

        if (animateAttendance < tempAttendanceNum && animateAttendance < finalAttendance)
        {
            animateAttendance += 1;
            attendanceTextBox.text = animateAttendance + "/" + maxAttendance;
        }
        if (timer > 5)
        {
            tempAttendanceNum = tempAttendanceNum + RandomNum(1, 10);
            timer = 0;
        }
        timer += 1;
        if (currentGame.isGameOver)
        {
            animateAttendance = finalAttendance;
            attendanceTextBox.text = animateAttendance + "/" + maxAttendance;
        }
    }

    private int RandomNum(int num1, int num2)
    {
        return UnityEngine.Random.Range(num1, num2 + 1);
    }


    private void PlayGame()
    {
        if (!currentGame.isGameOver && triggerGame)
        {
            triggerGame = false;
            //Start Half Inning
            int inningNum = currentGame.gameScoreCard.GetInningCounter();
            CreateNewInningVisual(scoreboard.inningCell.Count, inningNum);
            currentGame.PlayHalfInning();
            int scoredPoints = 0;

            if (topInning)
            {
                scoredPoints = currentGame.gameScoreCard.GetScoreInningTeam(awayTeam, inningNum);
            } else
            {
                scoredPoints = currentGame.gameScoreCard.GetScoreInningTeam(homeTeam, inningNum);

            }
            float delayTime = 0.3f;
            float debugDelay = 0f; //DEBUG ONLY
            if (scoredPoints > 2) { delayTime = 0.5f; }
            if (scoredPoints > 4) { delayTime = 0.7f; }
            if (scoredPoints > 5) { delayTime = 1f; }

            StartCoroutine(Delay(delayTime + debugDelay, scoredPoints, inningNum));

        }
        
    }

    IEnumerator Delay(float time, int score, int inningNum)
    {
        yield return new WaitForSeconds(time);

        //Display UI Info
        scoreboard.UpdateTextInCell(topInning, inningNum, score.ToString());
        scoreboard.UpdateRunsText(true, currentGame.gameScoreCard.GetScore(awayTeam).ToString());
        scoreboard.UpdateRunsText(false, currentGame.gameScoreCard.GetScore(homeTeam).ToString());
        scoreboard.UpdateHitsText(true, currentGame.gameScoreCard.GetHits(awayTeam).ToString());
        scoreboard.UpdateHitsText(false,  currentGame.gameScoreCard.GetHits(homeTeam).ToString());
        scoreboard.UpdateErrorsText(true,  currentGame.gameScoreCard.GetErrors(awayTeam).ToString());
        scoreboard.UpdateErrorsText(false,  currentGame.gameScoreCard.GetErrors(homeTeam).ToString());
        triggerGame = true;
        topInning = !topInning;

        if (currentGame.isGameOver)
        {
            leaveGameGameObject.SetActive(true);
        }
    }

    private void CreateNewInningVisual(int currentInningLength, int inningNum)
    {
        if (currentInningLength < inningNum)
        {
            scoreboard.AddCell("", "", inningNum.ToString());
        }
    }

    //BUTTONS
    public void StartGameButton()
    {
        triggerGame = true;
        startGameGameObject.SetActive(false);
    }
    public void GomeToRecapScreen(GameObject menu)
    {
        startGameGameObject.SetActive(true);
        leaveGameGameObject.SetActive(false);
        scoreboard.ResetAllInningCells();
        homeCode.TransitionMenus(homeCode.currentOpenMenu, menu);
        menu.GetComponent<RecapScreen>().LoadInfo(homeCode.user.GetTeam(), currentGame.gameScoreCard);
    }

}
