using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StadiumUpgradePrefab : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI attendanceText;

    public Image selectionBorder;
    public Image stadiumIcon;
    public GameObject lockOverlay;
    public GameObject purchasedOverlay;




    private User user;
    public Stadium stadium;
    private bool canPurchase;
    private StadiumSelectScreen stadiumSelectScreen;


    public void LoadInfo(Stadium stadium, User user, StadiumSelectScreen stadiumSelectScreen)
    {
        this.stadiumSelectScreen = stadiumSelectScreen;
        this.user = user;
        this.stadium = stadium;
        levelText.text = "LVL " + stadium.levelType.ToString();
        nameText.text = stadium.stadiumName;
        costText.text = stadium.priceToPurchase.ToString("C0");
        attendanceText.text = stadium.maxAttendance.ToString();

        canPurchase = CheckIfCanPurchaseAndLevel();
        lockOverlay.SetActive(!canPurchase);

        purchasedOverlay.SetActive(stadium.GetHasBeenPurchased());



        CheckIfAlreadyPurchased();

    }

    public bool CheckIfCanPurchaseAndLevel()
    {
        if (user.GetMoney() > stadium.priceToPurchase && stadium.levelType <= user.GetLevel())
        {
            return true;
        }
        return false;
    }
    private void CheckIfAlreadyPurchased()
    {
        if (user.GetCurrentStadium() == stadium)
        {
            purchasedOverlay.SetActive(true);
            lockOverlay.SetActive(false);
        }
    }

    public void PurchaseStadium()
    {
        if (canPurchase)
        {
            user.PurchaseNewStadium(stadium);

            purchasedOverlay.SetActive(true);
            lockOverlay.SetActive(false);
        }
    }

    public void HighlightSelection(Color colorSelection)
    {
        selectionBorder.color = colorSelection;
    }

    //BUTTONS
    public void SelectButton()
    {
        stadiumSelectScreen.UpgradeSelected(this);
    }

}
