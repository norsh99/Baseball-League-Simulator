using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TradeBarPrefab : MonoBehaviour
{

    public TextMeshProUGUI number;

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI position;

    public Image background;

    private Player player;
    private Team team;

    private TradeScreen tradeScreen;
    private bool committedToTrade;

    //Player player, int lineupNumber, ManageTeamScreen mts, GameObject gameOnject
    public void LoadInfoPlayer(Player player, TradeScreen tradeScreen)
    {
        this.player = player;
        this.team = player.GetTeam();
        this.tradeScreen = tradeScreen;
        committedToTrade = false;

        playerName.text = player.GetNameOfPlayer();
        position.text = player.GetPositionString();
        number.text = player.GetValue().ToString("C0");

    }
    public void LoadInfoTeam(Team team, TradeScreen tradeScreen)
    {
 
        this.team = team;
        this.tradeScreen = tradeScreen;


        playerName.text = team.GetName();
        position.text = "";
        number.text = "";

    }
    public Player GetPlayer() { return player; }
    public Team GetTeam() { return team; }


    public void ChamgeColorBackground(Color color)
    {
            background.color = color;
    }

    public void ButtonClick()
    {
        tradeScreen.SelectionMade(this);
    }

    public void SetCommittedToTrade(bool status) { committedToTrade = status; }
    public bool GetIsCommittedToTrade() { return committedToTrade; }




}
