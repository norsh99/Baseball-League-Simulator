using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeScreen : MonoBehaviour
{

    public Color[] selectionColors;

    private TradeBarPrefab yourTeamSelection;
    private TradeBarPrefab opponentTeamSelection;

    private bool teamSelected;

    public HomeCode homeCode;
    private User user;



    //Side Panel Vars
    public GameObject tradePlayerBarPrefab;
    public GameObject backButton;
    public GameObject tradeArrowButton;
    public GameObject requestArrowButton;
    public TextMeshProUGUI yourTeamNameText;
    public TextMeshProUGUI theirTeamNameText;




    public Transform leftPanelContainer;
    public Transform rightPanelContainer;

    private List<GameObject> leftPanelGOs;
    private List<GameObject> rightPanelGOs;




    //Center Vars
    public GameObject tradeCenterPrefab;
    public GameObject tradeButton;

    public Transform upCenterContainer;
    public Transform downCenterContainer;
    public TextMeshProUGUI yourValueText;
    public TextMeshProUGUI theirValueText;


    private List<GameObject> centerUpPanelGOs;
    private List<GameObject> centerDownPanelGOs;

    public GameObject infoSection;
    public GameObject tradeSection;
    public GameObject rightQuestionButton;
    public GameObject leftQuestionButton;




    //Trade Money Vars
    //Yours
    public GameObject[] moneyButtonMenus;
    public TMP_InputField inputAmountInputBox;
    public Image inputTextColor;
    public TextMeshProUGUI finalAmountTextBox;
    private int tradeMoneyAmount;
    public Color[] warningColors;
    //Theirs
    public GameObject[] moneyButtonMenus2;
    public TMP_InputField inputAmountInputBox2;
    public Image inputTextColor2;
    public TextMeshProUGUI finalAmountTextBox2;
    private int tradeMoneyAmount2;





    //Total Value Trade Vars
    private float yourTradeValue;
    private float theirTradeValue;
    private Team currentTeamSelected;

    //Accept/Decline Popup
    public GameObject popup;
    public TextMeshProUGUI popupTextbox;
    public Image popupBackgroundHighlight;


    //Player Info Vars
    public PlayerBaseStatsPanel playerBaseStatsPanel;
    public GeneralInfoUIBox generalInfoUIBox;
    public DataField dataField;




    public void LoadInfo()
    {
        ResetThePage();
    }

    private void ResetThePage()
    {
        teamSelected = false;
        user = homeCode.user;
        tradeArrowButton.SetActive(false);
        requestArrowButton.SetActive(false);
        tradeMoneyAmount = 0;
        yourTradeValue = 0;
        theirTradeValue = 0;
        currentTeamSelected = null;
        yourValueText.text = yourTradeValue.ToString("C0");
        theirValueText.text = theirTradeValue.ToString("C0");
        yourTeamNameText.text = user.GetTeam().GetName();
        UpdateTheirTeamNameTextBox();
        infoSection.SetActive(false);
        tradeSection.SetActive(true);
        rightQuestionButton.SetActive(false);
        leftQuestionButton.SetActive(false);


        popup.SetActive(false);


        tradeButton.SetActive(false);

        DestroyLeftBars();
        DestroyRightBars();
        DestroyCenterUpBars();
        DestroyCenterDownBars();
        PanelListLaodInPlayers(user.GetTeam().GetAllPlayers(), leftPanelGOs, leftPanelContainer);
        PanelListLaodInTeams(homeCode.allDivisions.GetAllTeamsExcludingUserTeam(), rightPanelGOs, rightPanelContainer);
    }

    private void PlayerInfoPage(Player player)
    {
        playerBaseStatsPanel.LoadInfo(player);
        generalInfoUIBox.LoadInfo(player);
        List<Stats> newList = new List<Stats>();
        newList.Add(player.GetSeasonStats());
        dataField.SetInfo(newList);
    }



    public void SelectionMade(TradeBarPrefab selection)
    {
        //You clicked left side, your team.
        if (selection.GetTeam() == user.GetTeam())
        {
            if (selection != yourTeamSelection && !selection.GetIsCommittedToTrade())
            {

                selection.ChamgeColorBackground(selectionColors[1]);
                tradeArrowButton.SetActive(true);
                leftQuestionButton.SetActive(true);

                if (yourTeamSelection != null)
                {
                    yourTeamSelection.ChamgeColorBackground(selectionColors[0]);

                }
                yourTeamSelection = selection;
            }
            else if (selection == yourTeamSelection)
            {
                selection.ChamgeColorBackground(selectionColors[0]);
                yourTeamSelection = null;
                tradeArrowButton.SetActive(false);
                leftQuestionButton.SetActive(false);


            }


        }
        else //You clicked right side
        {
            //Team has been selected already
            if (teamSelected)
            {
                //You are making a player selection
                if (selection != opponentTeamSelection && !selection.GetIsCommittedToTrade())
                {

                    selection.ChamgeColorBackground(selectionColors[2]);
                    requestArrowButton.SetActive(true);
                    rightQuestionButton.SetActive(true);


                    if (opponentTeamSelection != null)
                    {
                        opponentTeamSelection.ChamgeColorBackground(selectionColors[0]);
                    }
                    opponentTeamSelection = selection;
                }
                else if (selection == opponentTeamSelection && !selection.GetIsCommittedToTrade())
                {
                    selection.ChamgeColorBackground(selectionColors[0]);
                    opponentTeamSelection = null;
                    requestArrowButton.SetActive(false);
                    rightQuestionButton.SetActive(false);


                }


            }
            else //No team selected yet
            {
                teamSelected = true;
                currentTeamSelected = selection.GetTeam();
                UpdateTheirTeamNameTextBox();

                //Load in all players on that team.
                DestroyRightBars();
                PanelListLaodInPlayers(selection.GetTeam().GetAllPlayers(), rightPanelGOs, rightPanelContainer);

                //Enable back button
                backButton.SetActive(true);

            }
        }
        
    }

    private void UpdateTheirTeamNameTextBox()
    {
        if (teamSelected)
        {
            theirTeamNameText.text = currentTeamSelected.GetName();

        }
        else
        {
            theirTeamNameText.text = "All Teams";

        }
    }

    private void LoadInCenterBars(Player player, List<GameObject> panelObjects, Transform container, Color colorBackground, TradeBarPrefab tradeBar)
    {
            GameObject upgradeBox = Instantiate(tradeCenterPrefab, new Vector3(), Quaternion.identity, container);

            upgradeBox.GetComponent<TradeCenterBarPrefab>().LoadInfo(colorBackground, player, this, tradeBar);
            panelObjects.Add(upgradeBox);

    }


    private void PanelListLaodInPlayers(List<Player> allPlayers, List<GameObject> panelObjects, Transform container)
    {
        for (int i = 0; i < allPlayers.Count; i++)
        {
            GameObject upgradeBox = Instantiate(tradePlayerBarPrefab, new Vector3(), Quaternion.identity, container);

            upgradeBox.GetComponent<TradeBarPrefab>().LoadInfoPlayer(allPlayers[i], this);
            panelObjects.Add(upgradeBox);

        }
    }

    private void PanelListLaodInTeams(List<Team> allTeams, List<GameObject> panelObjects, Transform container)
    {
        for (int i = 0; i < allTeams.Count; i++)
        {
            GameObject upgradeBox = Instantiate(tradePlayerBarPrefab, new Vector3(), Quaternion.identity, container);

            upgradeBox.GetComponent<TradeBarPrefab>().LoadInfoTeam(allTeams[i], this);
            panelObjects.Add(upgradeBox);

        }
    }

    private void DestroyRightBars()
    {
        if (rightPanelGOs == null)
        {
            rightPanelGOs = new List<GameObject>();
        }

        if (rightPanelGOs.Count > 0)
        {
            for (int i = 0; i < rightPanelGOs.Count; i++)
            {
                Destroy(rightPanelGOs[i]);
            }
        }
        rightPanelGOs = new List<GameObject>();
    }
    private void DestroyLeftBars()
    {
        if (leftPanelGOs == null)
        {
            leftPanelGOs = new List<GameObject>();
        }

        if (leftPanelGOs.Count > 0)
        {
            for (int i = 0; i < leftPanelGOs.Count; i++)
            {
                Destroy(leftPanelGOs[i]);
            }
        }
        leftPanelGOs = new List<GameObject>();
    }
    private void DestroyCenterUpBars()
    {
        if (centerUpPanelGOs == null)
        {
            centerUpPanelGOs = new List<GameObject>();
        }

        if (centerUpPanelGOs.Count > 0)
        {
            for (int i = 0; i < centerUpPanelGOs.Count; i++)
            {
                Destroy(centerUpPanelGOs[i]);
            }
        }
        centerUpPanelGOs = new List<GameObject>();
    }
    private void DestroyCenterDownBars()
    {
        if (centerDownPanelGOs == null)
        {
            centerDownPanelGOs = new List<GameObject>();
        }

        if (centerDownPanelGOs.Count > 0)
        {
            for (int i = 0; i < centerDownPanelGOs.Count; i++)
            {
                Destroy(centerDownPanelGOs[i]);
            }
        }
        centerDownPanelGOs = new List<GameObject>();
    }

    public void RemoveBarFromList(TradeCenterBarPrefab centerBar, TradeBarPrefab tradeBar)
    {
        for (int i = 0; i < centerUpPanelGOs.Count; i++)
        {
            if (centerUpPanelGOs[i].GetComponent<TradeCenterBarPrefab>() == centerBar)
            {
                Destroy(centerUpPanelGOs[i]);
                centerUpPanelGOs.Remove(centerUpPanelGOs[i]);
                tradeBar.ChamgeColorBackground(selectionColors[0]);
                tradeBar.SetCommittedToTrade(false);
                CompareTradeToActivateTradeButton();

                return;
            }
        }
        for (int i = 0; i < centerDownPanelGOs.Count; i++)
        {
            if (centerDownPanelGOs[i].GetComponent<TradeCenterBarPrefab>() == centerBar)
            {
                Destroy(centerDownPanelGOs[i]);
                centerDownPanelGOs.Remove(centerDownPanelGOs[i]);
                tradeBar.ChamgeColorBackground(selectionColors[0]);
                tradeBar.SetCommittedToTrade(false);
                CompareTradeToActivateTradeButton();

                return;
            }
        }
    }

    IEnumerator FlashWarning(Image textBox)
    {
        textBox.color = warningColors[0];
        yield return new WaitForSeconds(.1f);
        textBox.color = warningColors[1];
        yield return new WaitForSeconds(.1f);
        textBox.color = warningColors[0];
        yield return new WaitForSeconds(.1f);
        textBox.color = warningColors[1];
        yield return new WaitForSeconds(.1f);
        textBox.color = warningColors[2];
    }

    private void CompareTradeToActivateTradeButton()
    {
        bool allPlayersMatchPostions = true;

        if (centerUpPanelGOs != null)
        {
            if (centerUpPanelGOs.Count == 0)
            {
                allPlayersMatchPostions = false;
            }
            //Debug.Log("CenterUp Length: " + centerUpPanelGOs.Count);
            for (int i = 0; i < centerUpPanelGOs.Count; i++)
            {
                if (!CompareToList(centerUpPanelGOs[i].GetComponent<TradeCenterBarPrefab>().GetPlayer().GetPosition()))
                {
                    allPlayersMatchPostions = false;
                }
            }
        }
        else
        {
            allPlayersMatchPostions = false;
        }

        if (allPlayersMatchPostions)
        {
            tradeButton.SetActive(true);
        }
        else
        {
            tradeButton.SetActive(false);
        }
        CalcualteTradeValue();
    }
    private bool CompareToList(int position)
    {
        if (centerDownPanelGOs == null)
        {
            return false;
        }
        //Debug.Log("CenterDown Length: " + centerDownPanelGOs.Count);

        for (int i = 0; i < centerDownPanelGOs.Count; i++)
        {
            if (centerDownPanelGOs[i].GetComponent<TradeCenterBarPrefab>().GetPlayer().GetPosition() == position)
            {
                return true;
            }
        }
        return false;
    }
    private int RandomNum(int num1, int num2)
    {
        return Random.Range(num1, num2 + 1);
    }

    private bool DoesOpponentAcceptTradeOffer()
    {
        float range = RandomNum(0, 1) * .1f;
        //Debug.Log("Your Trade Value: " + yourTradeValue);
        //Debug.Log("Their Trade Value: " + theirTradeValue);

        float theirPreferredTradeValue = theirTradeValue * (currentTeamSelected.GetTeamTradeLikelinessRatio() - range);
        //Debug.Log("Their Preferred Trade Value: " + theirPreferredTradeValue);
        //Debug.Log("Range: " + range + " Likeliness Ratio: " + currentTeamSelected.GetTeamTradeLikelinessRatio());


        if (yourTradeValue >= theirPreferredTradeValue)
        {
            return true;
        }
        return false;
    }

    private void CalcualteTradeValue()
    {
        theirTradeValue = 0;
        yourTradeValue = 0;
        if (centerUpPanelGOs != null)
        {
            for (int i = 0; i < centerUpPanelGOs.Count; i++)
            {
                yourTradeValue += centerUpPanelGOs[i].GetComponent<TradeCenterBarPrefab>().GetPlayer().GetValue();
            }
        }
        if (centerDownPanelGOs != null)
        {
            for (int i = 0; i < centerDownPanelGOs.Count; i++)
            {
                theirTradeValue += centerDownPanelGOs[i].GetComponent<TradeCenterBarPrefab>().GetPlayer().GetValue();
            }
        }
        yourTradeValue += tradeMoneyAmount;
        theirTradeValue += tradeMoneyAmount2;

        yourValueText.text = yourTradeValue.ToString("C0");
        theirValueText.text = theirTradeValue.ToString("C0");
    }

    private void PopupFormat(bool didAcceptTrade)
    {
        popup.SetActive(true);

        if (didAcceptTrade)
        {
            popupTextbox.text = "Your offer was accepted.";
            popupBackgroundHighlight.color = selectionColors[3];
        }
        else
        {
            popupTextbox.text = "Your offer was declined.";
            popupBackgroundHighlight.color = selectionColors[4];

        }
    }

    //Buttons
    public void BackButton()
    {
        DestroyRightBars();
        PanelListLaodInTeams(homeCode.allDivisions.GetAllTeamsExcludingUserTeam(), rightPanelGOs, rightPanelContainer);
        backButton.SetActive(false);
        teamSelected = false;
        currentTeamSelected = null;
        requestArrowButton.SetActive(false);
        DestroyCenterDownBars();
        CompareTradeToActivateTradeButton();
        UpdateTheirTeamNameTextBox();
    }

    public void TradePlayerButton(bool isLeftSideButton)
    {
        if (isLeftSideButton)
        {
            LoadInCenterBars(yourTeamSelection.GetPlayer(), centerUpPanelGOs, upCenterContainer, selectionColors[1], yourTeamSelection);
            yourTeamSelection.SetCommittedToTrade(true);
            yourTeamSelection.ChamgeColorBackground(selectionColors[4]);
            yourTeamSelection = null;
            tradeArrowButton.SetActive(false);
            leftQuestionButton.SetActive(false);


        }
        else
        {
            LoadInCenterBars(opponentTeamSelection.GetPlayer(), centerDownPanelGOs, downCenterContainer, selectionColors[2], opponentTeamSelection);
            opponentTeamSelection.SetCommittedToTrade(true);
            opponentTeamSelection.ChamgeColorBackground(selectionColors[4]);
            opponentTeamSelection = null;
            requestArrowButton.SetActive(false);
            rightQuestionButton.SetActive(false);

        }
        CompareTradeToActivateTradeButton();
    }

    private void TradePlayersBetweenTeams()
    {
        for (int i = 0; i < centerUpPanelGOs.Count; i++)
        {
            Player yourPlayer = centerUpPanelGOs[i].GetComponent<TradeCenterBarPrefab>().GetPlayer();
            Player theirPlayer = centerDownPanelGOs[i].GetComponent<TradeCenterBarPrefab>().GetPlayer();

            user.GetTeam().GetAllPlayers().Add(theirPlayer);
            currentTeamSelected.GetAllPlayers().Add(yourPlayer);

            user.GetTeam().GetAllPlayers().Remove(yourPlayer);
            currentTeamSelected.GetAllPlayers().Remove(theirPlayer);
        }
    }

    public void TradeMoneyButton()
    {
        moneyButtonMenus[1].SetActive(false);
        moneyButtonMenus[2].SetActive(true);
        inputAmountInputBox.text = "";
    }
    public void TradeMoneyButton2()
    {
        moneyButtonMenus2[1].SetActive(false);
        moneyButtonMenus2[2].SetActive(true);
        inputAmountInputBox2.text = "";
    }
    public void ExitButtonOnMoneyButton()
    {
        moneyButtonMenus[1].SetActive(true);
        moneyButtonMenus[2].SetActive(false);
        moneyButtonMenus[3].SetActive(false);

        tradeMoneyAmount = 0;
        CalcualteTradeValue();
    }
    public void ExitButtonOnMoneyButton2()
    {
        moneyButtonMenus2[1].SetActive(true);
        moneyButtonMenus2[2].SetActive(false);
        moneyButtonMenus2[3].SetActive(false);

        tradeMoneyAmount2 = 0;
        CalcualteTradeValue();
    }

    public void SubmitButton()
    {
        int amount = int.Parse(inputAmountInputBox.text);
        if (user.GetMoney() > amount && amount > 0)
        {
            tradeMoneyAmount = amount;
            moneyButtonMenus[1].SetActive(false);
            moneyButtonMenus[2].SetActive(false);
            moneyButtonMenus[3].SetActive(true);
            finalAmountTextBox.text = amount.ToString("C0");
            CalcualteTradeValue();
        }
        else
        {
            StartCoroutine(FlashWarning(inputTextColor));
        }
    }
    public void SubmitButton2()
    {
        int amount = int.Parse(inputAmountInputBox2.text);
        if (user.GetMoney() > amount && amount > 0)
        {
            tradeMoneyAmount2 = amount;
            moneyButtonMenus2[1].SetActive(false);
            moneyButtonMenus2[2].SetActive(false);
            moneyButtonMenus2[3].SetActive(true);
            finalAmountTextBox2.text = amount.ToString("C0");
            CalcualteTradeValue();
        }
        else
        {
            StartCoroutine(FlashWarning(inputTextColor));
        }
    }

    public void TradeButton()
    {
        if (DoesOpponentAcceptTradeOffer())
        {
            //Opponent accepted trade
            PopupFormat(true);

            //Commit trade
            TradePlayersBetweenTeams();

            //If paying money then pay money
            if (tradeMoneyAmount > 0)
            {
                user.SpendMoney(tradeMoneyAmount, true);
            }
            ExitButtonOnMoneyButton();
            ExitButtonOnMoneyButton2();
            ResetThePage();
        }
        else
        {
            //No trade made
            PopupFormat(false);

        }
    }

    public void PopupOkButton()
    {
        popup.SetActive(false);
    }

    public void ExitButton() //Exit the Trading screen
    {
        homeCode.GoToTradeScreen(gameObject);
    }
    public void QuitPlayerInfoArea() 
    {
        infoSection.SetActive(false);
        tradeSection.SetActive(true);
    }
    public void OpenPlayerInfo(bool isLeftSide)
    {
        if (isLeftSide)
        {
            PlayerInfoPage(yourTeamSelection.GetPlayer());
            
        }
        else
        {
            PlayerInfoPage(opponentTeamSelection.GetPlayer());

        }
        infoSection.SetActive(true);
        tradeSection.SetActive(false);
    }
}
