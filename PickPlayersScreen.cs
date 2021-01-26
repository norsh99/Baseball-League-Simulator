using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPlayersScreen : MonoBehaviour
{

    public HomeCode homeCode;
    public GameObject menuPlayerInfoCell; //Prefab for the cells
    private List<Player> allPlayers;
    private Team playerTeam;

    public Transform list1;
    public Transform list2;

    private List<GameObject> allGOList1;
    private List<GameObject> allGOList2;

    private Player selectedPlayer;
    public GameObject playerInfoScreen;
    public GameObject diamondScreen;
    public PlayerBaseStatsPanel playerBaseStatsPanel;
    public DataField dataField;
    public DiamondFieldUI diamondFieldUI;
    public GeneralInfoUIBox generalInfoUIBox;

    private MenuPlayerInfoCell currentSelection;
    public DraftPlayerButton draftPlayerButton;

    private bool[] pickedPositions;

    public GameObject submitTeamButton;
    public GameObject autoPickPlayersButton;


    public void LoadSceen(Team playerTeam, List<Player> allPlayersList)
    {
        allPlayers = allPlayersList;
        this.playerTeam = playerTeam;

        DestroyTabs(allGOList1);
        DestroyTabs(allGOList2);

        allGOList1 = CreateTabs(playerTeam.GetAllPlayers(), list1);
        allGOList2 = CreateTabs(allPlayers, list2);

        pickedPositions = AnalyzeTeamPositions(playerTeam);
        diamondFieldUI.SetAllPositionsEmpty();
        if (playerTeam.GetAllPlayers() != null)
        {
            UpdateDiamondUI(playerTeam.GetAllPlayers());
        }
    }

    private bool[] AnalyzeTeamPositions(Team team)
    {
        bool[] positions = new bool[] { false, false, false, false, false, false, false, false, false };
        List<Player> thePlayers = team.GetAllPlayers();
        if (thePlayers != null)
        {
            for (int i = 0; i < thePlayers.Count; i++)
            {
                int index = thePlayers[i].GetPosition()-1;
                positions[index] = true;
            }
        }
        
        return positions;
    }

    private void UpdateDiamondUI(List<Player> thePlayers)
    {
        for (int i = 0; i < thePlayers.Count; i++)
        {
            diamondFieldUI.UpdatePosition(thePlayers[i].GetPosition(), thePlayers[i]);

        }
    }

    private List<GameObject> CreateTabs(List<Player> newList, Transform theListDesignation)
    {
        List<GameObject> allGOList = new List<GameObject>();
        for (int i = 0; i < newList.Count; i++)
        {
            GameObject tab = Instantiate(menuPlayerInfoCell, new Vector3(), Quaternion.identity, theListDesignation);
            allGOList.Add(tab);
            Player currentPlayer = newList[i];
            tab.GetComponent<MenuPlayerInfoCell>().LoadInfo(currentPlayer, this);
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

    public void ActivatePlayerInfoScreen(Player player, MenuPlayerInfoCell mps)
    {
        selectedPlayer = player;
        currentSelection = mps;
        DraftButtonColorChange();
        playerInfoScreen.SetActive(true);
        diamondScreen.SetActive(false);

        playerBaseStatsPanel.LoadInfo(player);
        generalInfoUIBox.LoadInfo(player);

        List<Stats> newList = new List<Stats>();
        newList.Add(player.GetSeasonStats());
        dataField.SetInfo(newList);


        //Update all fields of info
    }

    private void DraftButtonColorChange()
    {
        if (selectedPlayer.GetTeam() != homeCode.user.GetTeam())
        {

            if (pickedPositions[selectedPlayer.GetPosition() - 1])
            {
                //Can't purchase, position already filled
                draftPlayerButton.SetInfo(false, "Position already filled");

            }
            else if (homeCode.user.GetMoney() >= selectedPlayer.GetValue())
            {
                //Can Purchase
                draftPlayerButton.SetInfo(true, selectedPlayer.GetValue().ToString("C0"));
            }
            else 
            {
                //Can't purchase, not enough money
                draftPlayerButton.SetInfo(false, "No Enough Funds");
            }

            
        }
        else
        {
            draftPlayerButton.SetForSale(selectedPlayer.GetValue().ToString("C0"));
        }
       
    }

    private void TransitionPlayerToTeam(Player player)
    {
        //Transition to team
        FillOrReleasePositionTracker(true, player.GetPosition());

        homeCode.user.GetTeam().AddPlayerToTeam(player);
        player.SetBoughtValue(player.GetValue());
        DestroyTabs(allGOList1);
        allGOList1 = CreateTabs(playerTeam.GetAllPlayers(), list1);
        diamondFieldUI.UpdatePosition(player.GetPosition(), player);
        player.SetTeam(homeCode.user.GetTeam());

        //Remove from all players list
        allPlayers.Remove(selectedPlayer);
        if (selectedPlayer != null)
        {
            currentSelection.DraftPlayer();

        }
        selectedPlayer = null;
        homeCode.user.GetTeam().SetStartingLineUp(null);
        ExitButton();
    }

    private void TradePlayerToEmpty(Player player)
    {
        FillOrReleasePositionTracker(false, player.GetPosition());
        playerTeam.GetAllPlayers().Remove(player);
        DestroyTabs(allGOList1);
        allGOList1 = CreateTabs(playerTeam.GetAllPlayers(), list1);
        diamondFieldUI.RemovePosition(player.GetPosition());
        selectedPlayer = null;
        homeCode.user.GetTeam().SetStartingLineUp(null);
        ExitButton();
    }

    private void FillOrReleasePositionTracker(bool fill, int position)
    {
        if (fill)
        {
            pickedPositions[position - 1] = true;
        }
        else
        {
            pickedPositions[position - 1] = false;

        }
    }

    private void CheckIfTeamIsComplete()
    {
        if (playerTeam.GetAllPlayers().Count == 9)
        {
            submitTeamButton.SetActive(true);
        }
        else
        {
            submitTeamButton.SetActive(false);
        }
    }

    //BUTTONS
    public void ExitButton()
    {
        playerInfoScreen.SetActive(false);
        diamondScreen.SetActive(true);
    }

    public void BuyAndSellButton()
    {
        if (selectedPlayer.GetTeam() != homeCode.user.GetTeam())
        {
            if (!pickedPositions[selectedPlayer.GetPosition()-1])
            {
                if (homeCode.user.SpendMoney(selectedPlayer.GetValue(), true))
                {
                    //DRAFTED
                    //Move player over
                    TransitionPlayerToTeam(selectedPlayer);
                    CheckIfTeamIsComplete();
                }
                else
                {
                    //Not enough money
                    Debug.Log("Not Enought Money To Draft");
                }
            }
            else
            {
                Debug.Log("Position already filled.");
            }
            
        }
        else
        {
            //Sold Player
            homeCode.user.AddMoney(selectedPlayer.GetValue());
            TradePlayerToEmpty(selectedPlayer);
            CheckIfTeamIsComplete();
        }
    }
    public void SubmitTeamButton(GameObject destinationGameObject)
    {
        homeCode.GoHomeButton(destinationGameObject);
        homeCode.homePage.EnableFullGameMenu();
        homeCode.StadiumButtonSwitch(true);
        homeCode.tradeButton.SetActive(true);
    }

    public void AutoPickTeamButton()
    {
        autoPickPlayersButton.SetActive(false);
        for (int i = 0; i < allPlayers.Count; i++)
        {
            
            Player pickedPlayer = allPlayers[i];
            if (pickedPlayer.GetValue() <= 10000 && !pickedPositions[pickedPlayer.GetPosition()-1])
            {
                homeCode.user.SpendMoney(pickedPlayer.GetValue(), true);
                currentSelection = allGOList2[i].GetComponent<MenuPlayerInfoCell>();
                TransitionPlayerToTeam(pickedPlayer);
                CheckIfTeamIsComplete();

            }

            if (playerTeam.GetAllPlayers().Count == 9)
            {
                break;
            }
        }
    }
}
