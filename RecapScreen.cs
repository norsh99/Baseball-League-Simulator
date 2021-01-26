using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecapScreen : MonoBehaviour
{
    public TextMeshProUGUI teamNamesTextbox;
    public TextMeshProUGUI scoreTextbox;
    public TextMeshProUGUI attendanceTextbox;
    public TextMeshProUGUI ticketPriceTextbox;
    public TextMeshProUGUI finalSalesTextbox;





    private Team teamRecap;
    public PlayerStatsRecapPanel recapPanel;

    public HomeCode homeCode;






    public void LoadInfo(Team teamToRecap, GameScoreCard scoreCard)
    {
        teamRecap = teamToRecap;
        recapPanel.LoadInfo(teamRecap.GetAllPlayers());
        UpdateTextFields(scoreCard);


        int attendance = homeCode.user.GetLatestAttendanceNumbers();
        int ticketPrice = homeCode.user.GetTicketPrice();
        int sales = attendance * ticketPrice;

        attendanceTextbox.text = attendance.ToString() + "/" + homeCode.user.GetCurrentStadium().maxAttendance;
        ticketPriceTextbox.text = ticketPrice.ToString("C0");
        finalSalesTextbox.text = sales.ToString("C0");
        homeCode.user.AddMoney(sales);

    }

    private void UpdateTextFields(GameScoreCard scoreCard)
    {
        if (scoreCard.GetScore(scoreCard.t1) > scoreCard.GetScore(scoreCard.t2))
        {
            teamNamesTextbox.text = scoreCard.t1.GetName() + " - " + scoreCard.t2.GetName();
            scoreTextbox.text = scoreCard.GetScore(scoreCard.t1) + " - " + scoreCard.GetScore(scoreCard.t2);
        }
        else
        {
            teamNamesTextbox.text = scoreCard.t2.GetName() + " - " + scoreCard.t1.GetName();
            scoreTextbox.text = scoreCard.GetScore(scoreCard.t2) + " - " + scoreCard.GetScore(scoreCard.t1);
        }
    }





    //BUTTONS

    public void GoToHomeScreen(GameObject menu)
    {
        homeCode.TransitionMenus(homeCode.currentOpenMenu, menu);
    }
}
