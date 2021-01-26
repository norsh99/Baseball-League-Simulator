using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_SimplePlayerInfoBar : MonoBehaviour
{
    public TextMeshProUGUI number;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI position;

    public Image background;
    public List<Color> selectionColors;

    private Player player;
    private ManageTeamScreen mts;
    private bool isSelected;

    private int lineupPosition;

    private GameObject go;

    public void LoadInfo(Player player, int lineupNumber, ManageTeamScreen mts, GameObject gameOnject)
    {
        this.player = player;
        this.mts = mts;
        lineupPosition = lineupNumber;
        this.go = gameObject;

        number.text = lineupNumber.ToString();
        playerName.text = player.GetNameOfPlayer();
        position.text = player.GetPositionString();


    }
    public Player GetPlayer() { return player; }

    public void SelectOn()
    {
        if (!isSelected)
        {
            background.color = selectionColors[1];
            mts.ClickedOnPlayer(this);
            isSelected = true;
        }
        else
        {
            SelectOff();
            mts.ExitButton();
        }
       
    }
    public void SelectOff()
    {
        background.color = selectionColors[0];
        mts.DeSelectPlayer();
        isSelected = false;
    }

    public int GetLineupPosition() { return lineupPosition; }
    public void SetLineupPosition(int newPos) 
    { 
        lineupPosition = newPos;
        number.text = lineupPosition.ToString();
    }
    public GameObject GetGameObject() { return go; }
}
