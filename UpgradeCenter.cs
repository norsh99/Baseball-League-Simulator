using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeCenter : MonoBehaviour
{
    public List<UpgradeTabs> listOfTabs;
    public Color greyColor;
    public Color selectedColor;
    public Color cantUpgradeColor;


    public GameObject tabsPanel;
    public Image bannerImage;
    public TextMeshProUGUI bannerText;

    private Player selectedPlayer;

    public Image resetImage;
    public Image commitImage;

    private int totalCoinsSpent;
    private int battingCoinsSpent;
    private int fieldingCoinsSpent;
    private int runningCoinsSpent;
    private int pitchingCoinsSpent;
    private int staminaCoinsSpent;

    private bool isPanelOpen;

    public VerticalLayoutGroup masterVerticalLayoutGroup; //Enable this on and off to reset the layout group so things fit nicely
    public PlayerBaseStatsPanel playerBaseStatsPanel;
    public GeneralInfoUIBox generalInfoUIBox;

    


    public void LoadInfo(Player player)
    {
        selectedPlayer = player;

        totalCoinsSpent = 0;
        battingCoinsSpent = 0;
        fieldingCoinsSpent = 0;
        runningCoinsSpent = 0;
        pitchingCoinsSpent = 0;
        staminaCoinsSpent = 0;


        tabsPanel.SetActive(false);
        isPanelOpen = false;
        ResetTabs();
        BannerInfo();
    }

    private void UpdatePlayerInforPanel()
    {
        if (playerBaseStatsPanel != null)
        {
            playerBaseStatsPanel.LoadInfo(selectedPlayer);
        }
        if (generalInfoUIBox != null)
        {
            generalInfoUIBox.LoadInfo(selectedPlayer);
        }
    }
    private void TurnOnOffReset()
    {
        masterVerticalLayoutGroup.enabled = false;
        StartCoroutine(DelayReset(.01f));

    }

    IEnumerator DelayReset(float delayNum)
    {
        yield return new WaitForSeconds(delayNum);
        masterVerticalLayoutGroup.enabled = true;
    }


    private void BannerInfo()
    {
        if (selectedPlayer.GetUpgradeCoins() > 0)
        {
            bannerImage.color = selectedColor;
            bannerText.gameObject.SetActive(true);
            bannerText.text = selectedPlayer.GetUpgradeCoins().ToString();

        }
        else
        {
            bannerImage.color = greyColor;
            bannerText.gameObject.SetActive(false);
        }
    }

    public void ResetTabs()
    {
        for (int i = 0; i < listOfTabs.Count; i++)
        {
            listOfTabs[i].SetInfo(greyColor, "0");
        }
    }

   
    IEnumerator CantUpgradeColor(UpgradeTabs currentTab)
    {
        Color prevColor = currentTab.GetCurrentColor();
        currentTab.ChangeColor(cantUpgradeColor);
        yield return new WaitForSeconds(.1f);
        currentTab.ChangeColor(greyColor);
        yield return new WaitForSeconds(.1f);
        currentTab.ChangeColor(cantUpgradeColor);
        yield return new WaitForSeconds(.1f);
        currentTab.ChangeColor(prevColor);
    }
    IEnumerator CantUpgradeBannerColor(Image image)
    {
        Color prevColor = image.color;
        image.color = cantUpgradeColor;
        yield return new WaitForSeconds(.1f);
        image.color = prevColor;
        yield return new WaitForSeconds(.1f);
        image.color = cantUpgradeColor;
        yield return new WaitForSeconds(.1f);
        image.color = prevColor;

    }

    private void OpenPanel()
    {
        tabsPanel.SetActive(true);
        isPanelOpen = true;
        TurnOnOffReset();
    }
    private void ClosePanel()
    {
        tabsPanel.SetActive(false);
        isPanelOpen = false;
        TurnOnOffReset();
    }


    //BUTTONS
    public void OpenCloseButton()
    {
        if (isPanelOpen)
        {
            ClosePanel();
        }
        else
        {
            OpenPanel();
        }
    }


    public void CommitUpgradesButton()
    {
        if (totalCoinsSpent > 0)
        {
            selectedPlayer.AddStat_Batting(battingCoinsSpent);
            selectedPlayer.AddStat_Fielding(fieldingCoinsSpent);
            selectedPlayer.AddStat_Running(runningCoinsSpent);
            selectedPlayer.AddStat_Pitching(pitchingCoinsSpent);
            selectedPlayer.AddStat_Stamina(staminaCoinsSpent);

            selectedPlayer.SpendUpgradeCoins(totalCoinsSpent);

            UpdatePlayerInforPanel();
            ResetUpgradesButton();

        }
        else
        {
            StartCoroutine(CantUpgradeBannerColor(commitImage));
        }

    }

    public void ResetUpgradesButton()
    {
        if (totalCoinsSpent > 0)
        {
            BannerInfo();

            totalCoinsSpent = 0;
            battingCoinsSpent = 0;
            fieldingCoinsSpent = 0;
            runningCoinsSpent = 0;
            pitchingCoinsSpent = 0;
            staminaCoinsSpent = 0;

            resetImage.color = greyColor;
            commitImage.color = greyColor;


            ResetTabs();
        }
        else
        {
            StartCoroutine(CantUpgradeBannerColor(resetImage));
        }

    }

    public void TabClick(int numID)
    {
        UpgradeTabs currentTab = listOfTabs[numID-1];
        bool sendCantUpgradeColor = true;

        if (totalCoinsSpent < selectedPlayer.GetUpgradeCoins())
        {
            if (numID == 1)
            {
                if (selectedPlayer.CheckCanPurchaseStat_Batting(battingCoinsSpent + 1))
                {
                    battingCoinsSpent += 1;
                    totalCoinsSpent += 1;
                    sendCantUpgradeColor = false;
                    currentTab.SetInfo(selectedColor, battingCoinsSpent.ToString());

                    resetImage.color = selectedColor;
                    commitImage.color = selectedColor;
                }

            }
            else if (numID == 2)
            {
                if (selectedPlayer.CheckCanPurchaseStat_Fielding(fieldingCoinsSpent + 1))
                {
                    fieldingCoinsSpent += 1;
                    totalCoinsSpent += 1;
                    sendCantUpgradeColor = false;
                    currentTab.SetInfo(selectedColor, fieldingCoinsSpent.ToString());

                    resetImage.color = selectedColor;
                    commitImage.color = selectedColor;
                }
            }
            
            else if (numID == 3)
            {
                if (selectedPlayer.CheckCanPurchaseStat_Running(runningCoinsSpent + 1))
                {
                    runningCoinsSpent += 1;
                    totalCoinsSpent += 1;
                    sendCantUpgradeColor = false;
                    currentTab.SetInfo(selectedColor, runningCoinsSpent.ToString());

                    resetImage.color = selectedColor;
                    commitImage.color = selectedColor;
                }
            }
            else if (numID == 4)
            {
                if (selectedPlayer.CheckCanPurchaseStat_Pitching(pitchingCoinsSpent + 1))
                {
                    pitchingCoinsSpent += 1;
                    totalCoinsSpent += 1;
                    sendCantUpgradeColor = false;
                    currentTab.SetInfo(selectedColor, pitchingCoinsSpent.ToString());

                    resetImage.color = selectedColor;
                    commitImage.color = selectedColor;
                }
            }
            else if (numID == 5)
            {
                if (selectedPlayer.CheckCanPurchaseStat_Stamina(staminaCoinsSpent + 1))
                {
                    staminaCoinsSpent += 1;
                    totalCoinsSpent += 1;
                    sendCantUpgradeColor = false;
                    currentTab.SetInfo(selectedColor, staminaCoinsSpent.ToString());

                    resetImage.color = selectedColor;
                    commitImage.color = selectedColor;
                }
            }
        }
        else
        {
            StartCoroutine(CantUpgradeBannerColor(bannerImage));
        }
        

        if (sendCantUpgradeColor)
        {
            StartCoroutine(CantUpgradeColor(currentTab));
        }
    }

}
