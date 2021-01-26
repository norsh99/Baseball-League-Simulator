using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TradeCenterBarPrefab : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public Image backgroundImage;





    private Player player;
    private TradeScreen tradeScreen;
    private TradeBarPrefab tradeBar;

    public void LoadInfo(Color setColorBackground, Player player, TradeScreen tradeScreen, TradeBarPrefab tradeBar)
    {
        this.player = player;
        this.tradeScreen = tradeScreen;
        this.tradeBar = tradeBar;

        nameText.text = player.GetNameOfPlayer();
        backgroundImage.color = setColorBackground;
    }





    public Player GetPlayer() { return player; }

    //BUTTON
    public void RemoveButton()
    {
        tradeScreen.RemoveBarFromList(this, tradeBar);
    }
}
