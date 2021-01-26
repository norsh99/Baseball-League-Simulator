using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoPanel : MonoBehaviour
{


    private Player currentPlayer;
    public PlayerBaseStatsPanel playerBaseStatsPanel;
    public DataField dataField;

    public void LoadStats(Player player)
    {
        currentPlayer = player;
        playerBaseStatsPanel.LoadInfo(player);

        List<Stats> newList = new List<Stats>();
        newList.Add(player.GetCurrentGameStats());
        dataField.SetInfo(newList);
    }

    public void ExitPanelButton()
    {
        this.gameObject.SetActive(false);
    }
}
