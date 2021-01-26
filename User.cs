using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class User
{
    private Team yourTeam;
    private float money;
    private int ticketPrice;
    private int level;
    private int latestAttendanceNumbers;
    private int totalSeasonsPlayed;

    private Stadium stadium;

    public event Action onStatsChange;
    private HomeCode hc;
    public User(string pickName, HomeCode hc, Stadium starterStadium)
    {
        this.hc = hc;
        yourTeam = CreateNewTeam(pickName);
        money = 0;
        ticketPrice = 1;
        level = 1;
        totalSeasonsPlayed = 0;

        stadium = starterStadium;
    }
    public int DetermineAttendance()
    {
        int attendance = 0;
        int playerTotalScore = 0;

        //Work in the current price and if its fair do nothing, if its high then less people should come, if low then more people should come
        //Is the stadium nice? Work that in.
        //Is the team winning alot? Increase attendance. If losing a lot then less attendance.



        List<Player> allPlayers = yourTeam.GetAllPlayers();
        for (int i = 0; i < allPlayers.Count; i++)
        {
            int tempScore  = allPlayers[i].GetAthleticsStat();
            int levelScore = allPlayers[i].GetLevelStat();
            playerTotalScore += tempScore * levelScore;
        }
        attendance = playerTotalScore * level;

        int fluxNum = RandomNum(0, 500 * level);
        int attendanceFlux = 0;
        if (fluxNum < attendance)
        {
            attendanceFlux = attendance - fluxNum;

        }

        attendance = RandomNum(attendanceFlux, attendance);

        attendance = Attendace_EvaluatePriceToValue(attendance, ticketPrice, stadium);

        if (attendance >= stadium.maxAttendance)
        {
            attendance = stadium.maxAttendance;
        }

        latestAttendanceNumbers = attendance;

        return attendance;
    }


    //Consideres the price of the stadium, how good the team is and how far into the season you are. Compare all that to the asking price of the stadium
    private int Attendace_EvaluatePriceToValue(float currentAttendance, float currentPrice, Stadium stadium)
    {

        float wins = yourTeam.GetCurrentSeasonTeamStats().wins;
        float totalGames = yourTeam.GetCurrentSeasonTeamStats().totalGames;
        if (wins == 0)
        {
            wins = 1;
        }
        if (totalGames == 0)
        {
            totalGames = 1;
        }

        float winPertentage = (wins) / totalGames;
        float newSuggestedTicketValue = (winPertentage + .5f) * stadium.suggestedTicketPrice;
        float adjustmentPercentage = newSuggestedTicketValue / currentPrice;

        
        int newAttendance = Mathf.FloorToInt(currentAttendance * adjustmentPercentage);

        Debug.Log("Win Percentage: " + winPertentage);
        Debug.Log("New Suggested Val: " + newSuggestedTicketValue);
        Debug.Log("Adjustment Perc: " + adjustmentPercentage);
        Debug.Log("New Attendance Val: " + newAttendance);

        return newAttendance;
    }


    public float GetTotalAmountDueEachNewSeason()
    {
        float total = 0;
        total = stadium.recurringCosts + GetTotalValueOfPlayers();
        return total;
    }

    public float GetTotalValueOfPlayers()
    {
        float total = 0;
        List<Player> allPlayers = yourTeam.GetAllPlayers();
        for (int i = 0; i < allPlayers.Count; i++)
        {
            total += allPlayers[i].GetValue();
        }
        return total;
    }

    public int GetLevel() { return level; }
    public void IncreaseLevel() { level += 1; }

    private int RandomNum(int num1, int num2)
    {
        return UnityEngine.Random.Range(num1, num2 + 1);
    }

    public void IncreaseSeasonsPlayed() { totalSeasonsPlayed += 1; }
    public int GetTotalSeasonsPlayed() { return totalSeasonsPlayed; }

    public int GetLatestAttendanceNumbers() { return latestAttendanceNumbers; }

    public Stadium GetCurrentStadium() { return stadium; }
    public void StatsChange()
    {
        if (onStatsChange != null)
        {
            onStatsChange();
        }
    }

    private Team CreateNewTeam(string teamName)
    {
        Team newTeam = new Team(teamName, hc);
        return newTeam;
    }

    public Team GetTeam()
    {
        return yourTeam;
    }

    public float GetMoney() { return money; }

    public bool SpendMoney(float amount, bool spendMoney = false)
    {
        if (money - amount >= 0)
        {
            if (spendMoney)
            {
                AddMoney(-amount);
            }
            return true;
        }
        return false;
    }

    public void AddMoney(float addAmount)
    {
        money = money + addAmount;
        onStatsChange();
    }

    public void PurchaseNewStadium(Stadium newStadium)
    {
        SpendMoney(stadium.priceToPurchase, true);
        stadium = newStadium;
        newStadium.PurchaseStadium();

    }

    public int GetTicketPrice() { return ticketPrice; }
    public void SetTicketPrice(int newPrice) { ticketPrice = newPrice; onStatsChange(); }

    public int AddMoneyFromTicketSales(int attendance) 
    {
        int newMoney = attendance * ticketPrice;
        AddMoney(newMoney);
        return newMoney;
    }
}
