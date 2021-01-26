using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Diagnostics;
using UnityEngine;

public class ManageTeamScreen : MonoBehaviour
{
    public GameObject infoLinePrefab;
    private UI_SimplePlayerInfoBar selectedPlayer;
    public ChangeLineup changeLineUp;
    public HomeCode homeCode;
    public Transform listTransform;
    public DiamondFieldUI diamondFieldUI;

    public GameObject diamondUI;
    public GameObject infoScreen;

    public PlayerBaseStatsPanel playerBaseStatsPanel;
    public GeneralInfoUIBox generalInfoUIBox;
    public DataField dataField;
    public UpgradeCenter upgradeCenter;

    List<GameObject> tabList;

    List<Player> lineUp;
    public void LoadIn()
    {
        diamondFieldUI.SetAllPositionsEmpty();
        if (homeCode.user.GetTeam().GetStartingLineUp() == null)
        {
            lineUp = homeCode.user.GetTeam().GetAllPlayers();

        }
        else
        {
            lineUp = homeCode.user.GetTeam().GetStartingLineUp();

        }

        if (lineUp != null)
        {
            DestroyTabs(tabList);
            tabList = CreateTabs(lineUp, listTransform);
            UpdateDiamondUI();
        }
    }
    private List<GameObject> CreateTabs(List<Player> newList, Transform theListDesignation)
    {
        List<GameObject> allGOList = new List<GameObject>();

        for (int i = 0; i < newList.Count; i++)
        {
            GameObject tab = Instantiate(infoLinePrefab, new Vector3(), Quaternion.identity, theListDesignation);
            allGOList.Add(tab);
            Player currentPlayer = newList[i];
            tab.GetComponent<UI_SimplePlayerInfoBar>().LoadInfo(currentPlayer, i + 1, this, tab);
        }
        return allGOList;
    }

    private void DestroyTabs(List<GameObject> theList)
    {
        if (theList != null)
        {
            for (int i = 0; i < theList.Count; i++)
            {
                Destroy(theList[i]);
            }
        }

    }

    private void UpdateDiamondUI()
    {
        for (int i = 0; i < lineUp.Count; i++)
        {
            diamondFieldUI.UpdatePosition(lineUp[i].GetPosition(), lineUp[i]);

        }
    }

    public void ClickedOnPlayer(UI_SimplePlayerInfoBar selectPlayer)
    {
        if (selectedPlayer != null && selectedPlayer != selectPlayer)
        {
            selectedPlayer.SelectOff();
        }

        selectedPlayer = selectPlayer;
        DetermineChangeLineUpButtons(selectPlayer);

        infoScreen.SetActive(true);
        diamondUI.SetActive(false);

        playerBaseStatsPanel.LoadInfo(selectPlayer.GetPlayer());
        generalInfoUIBox.LoadInfo(selectPlayer.GetPlayer());

        List<Stats> newList = new List<Stats>();
        newList.Add(selectPlayer.GetPlayer().GetSeasonStats());
        dataField.SetInfo(newList);
        upgradeCenter.LoadInfo(selectPlayer.GetPlayer());

    }

    private void DetermineChangeLineUpButtons(UI_SimplePlayerInfoBar selectPlayer)
    {
        changeLineUp.TurnOffAll();
        if (selectPlayer.GetLineupPosition() >= 1 && selectPlayer.GetLineupPosition() < lineUp.Count)
        {
            changeLineUp.TurnOnDown();
        }
        if (selectPlayer.GetLineupPosition() <= lineUp.Count && selectPlayer.GetLineupPosition() > 1)
        {
            selectPlayer.GetLineupPosition();
            changeLineUp.TurnOnUp();
        }
    }
    public void DeSelectPlayer()
    {
        selectedPlayer = null;
        changeLineUp.TurnOffAll();

    }

    //BUTTONS
    public void ExitButton()
    {
        if (selectedPlayer != null)
        {
            selectedPlayer.SelectOff();
            selectedPlayer = null;
        }

        infoScreen.SetActive(false);
        diamondUI.SetActive(true);
    }

    public void MovePlayerUpInLineup()
    {

        GameObject selectedGo = selectedPlayer.GetGameObject();
        selectedGo.transform.SetSiblingIndex(selectedGo.transform.GetSiblingIndex() - 1);
        lineUp = GetNewPlayerOrder(tabList);
        DetermineChangeLineUpButtons(selectedPlayer);
        homeCode.user.GetTeam().SetStartingLineUp(lineUp); //Saving the new order to the team


    }
    public void MovePlayerDownpInLineup()
    {
        GameObject selectedGo = selectedPlayer.GetGameObject();
        selectedGo.transform.SetSiblingIndex(selectedGo.transform.GetSiblingIndex() + 1);
        lineUp = GetNewPlayerOrder(tabList);
        DetermineChangeLineUpButtons(selectedPlayer);
        homeCode.user.GetTeam().SetStartingLineUp(lineUp); //Saving the new order to the team

    }

    private List<Player> GetNewPlayerOrder( List<GameObject> objectList) 
    {
        List<Player> newList = new List<Player>();

        Player[] array = new Player[objectList.Count];
        for (int i = 0; i < objectList.Count; i++)
        {
            UI_SimplePlayerInfoBar bar = objectList[i].GetComponent<UI_SimplePlayerInfoBar>();
            int index = bar.transform.GetSiblingIndex();
            bar.SetLineupPosition(index + 1);
            array[index] = bar.GetPlayer();
        }

        newList = array.ToList<Player>();

        return newList;
    }

    private void PrintPostionsInList(List<Player> theList) //For Debugging
    {
        Debug.Log("--------------------------------------");
        for (int i = 0; i < theList.Count; i++)
        {
            Debug.Log(i + 1 + ". " + theList[i].GetNameOfPlayer());
        }
    }
}
