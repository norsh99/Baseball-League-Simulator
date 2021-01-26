using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerTrackerPrefab : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public TextMeshProUGUI playerText;
    public Image background;

    public PlayerTracker playerTracker;
    public Player player;
    public PlayerInfoPanel playerInfoPanel;

    public void AsignInfo(int number, string name, PlayerTracker playerTrack, Player importPLayer, PlayerInfoPanel pip)
    {
        numberText.text = number.ToString();
        playerText.text = name;
        playerTracker = playerTrack;
        player = importPLayer;
        playerInfoPanel = pip;
    }

    public void Select()
    {
        background.color = playerTracker.selectColor;
        playerTracker.ChangeSelection(this);
        playerInfoPanel.gameObject.SetActive(true);
        playerInfoPanel.LoadStats(player);
    }

    public void DeSelect()
    {
        background.color = playerTracker.defautColor;
    }

}
