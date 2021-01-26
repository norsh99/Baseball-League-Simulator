using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StadiumSelectScreen : MonoBehaviour
{
    public TextMeshProUGUI stadiumTypeTextbox;
    public TextMeshProUGUI priceTextbox;
    public TextMeshProUGUI infoTextbox;
    public TextMeshProUGUI stadiumTitleTextbox;

    //BUDGET INFO
    public TextMeshProUGUI budgetTitle;
    public TextMeshProUGUI playerCostText;
    public TextMeshProUGUI stadiumCostText;
    public TextMeshProUGUI totalVisitorsText;
    public TextMeshProUGUI totalTicketSalesText;

    public Color[] goodBadColors;



    private bool infoButtonON;
    public GameObject infoGameobject;
    public GameObject ticketPriceGameobject;
    public Image infoIconImage;

    public Sprite infoIcon;
    public Sprite ticketIcon;


    //UPGRADE AREA
    public GameObject upgradeStadiumPrefab;
    private StadiumUpgradePrefab selectedUpgrade;
    public Color[] upgradeSelectionColors;
    public Transform upgradeContainer;
    private List<GameObject> allUpgradePrefabs;
    public TextMeshProUGUI upgradeDescriptionBox;
    public GameObject purchaseButton;
    public GameObject descriptionGameobject;
    public Image upgradeIconButton;
    public List<Sprite> upgradeBudgetIcon;
    public GameObject[] upgradeBudgetScreens;
    private bool isUpgradeScreenOpen;

    



    public HomeCode homeCode;
    private User user;
    private Season currentSeason;

    void OnEnable()
    {
        //InfoButtonSelect(true);
    }

   
    public void LoadInfo()
    {
        user = homeCode.user;
        currentSeason = homeCode.currentSeason;
        budgetTitle.text = "Season " + user.GetTotalSeasonsPlayed() + " Budget";

        
        UpdatePriceBox();
        UpdateInfoSide();
        UpdateBudgetInfo();

        DestroyBars();
        UpgradeLoadIn(homeCode.GetAllStadiums());


        descriptionGameobject.SetActive(false);
        purchaseButton.SetActive(false);
        isUpgradeScreenOpen = false;
        upgradeBudgetScreens[0].SetActive(false);
        upgradeBudgetScreens[1].SetActive(true);
        upgradeIconButton.sprite = upgradeBudgetIcon[0];


    }

    private void UpdateInfoSide()
    {
        stadiumTitleTextbox.text = user.GetCurrentStadium().stadiumName;
        infoTextbox.text = user.GetCurrentStadium().benifitsDef;

    }

    private void UpdatePriceBox()
    {
        priceTextbox.text = user.GetTicketPrice().ToString("C0");
    }
    private void UpdateBudgetInfo()
    {
        playerCostText.text = user.GetTotalValueOfPlayers().ToString("C0");
        stadiumCostText.text = user.GetCurrentStadium().recurringCosts.ToString("C0");
        totalVisitorsText.text = currentSeason.GetTotalVisitors().ToString();
        totalTicketSalesText.text = currentSeason.GetTotalTicketSales().ToString("C0");
    }



    //UPGRADE AREA

    public void UpgradeSelected(StadiumUpgradePrefab selectedUp)
    {
        if (selectedUp != selectedUpgrade)
        {
            purchaseButton.SetActive(false);

            selectedUp.HighlightSelection(upgradeSelectionColors[0]);
            if (selectedUpgrade != null)
            {
                selectedUpgrade.HighlightSelection(upgradeSelectionColors[1]);

            }
            selectedUpgrade = selectedUp;

            //Update and enable info
            //Activate button to buy
            descriptionGameobject.SetActive(true);
            upgradeDescriptionBox.text = selectedUp.stadium.benifitsDef;

            if (selectedUp.CheckIfCanPurchaseAndLevel() && !selectedUp.stadium.GetHasBeenPurchased())
            {
                purchaseButton.SetActive(true);
            }
        }
        else if(selectedUp == selectedUpgrade)
        {
            selectedUp.HighlightSelection(upgradeSelectionColors[1]);
            selectedUpgrade = null;

            //Disable info and buy button
            descriptionGameobject.SetActive(false);
            purchaseButton.SetActive(false);



        }
    }


    private void UpgradeLoadIn(List<Stadium> allStadiums)
    {
        if (allUpgradePrefabs == null)
        {
            allUpgradePrefabs = new List<GameObject>();
        }


        for (int i = 0; i < allStadiums.Count; i++)
        {
            GameObject upgradeBox = Instantiate(upgradeStadiumPrefab, new Vector3(), Quaternion.identity, upgradeContainer);

            upgradeBox.GetComponent<StadiumUpgradePrefab>().LoadInfo(allStadiums[i], user, this);
            allUpgradePrefabs.Add(upgradeBox);

        }
    }

    private void DestroyBars()
    {
        if (allUpgradePrefabs == null)
        {
            allUpgradePrefabs = new List<GameObject>();
        }

        if (allUpgradePrefabs.Count > 0)
        {
            for (int i = 0; i < allUpgradePrefabs.Count; i++)
            {
                Destroy(allUpgradePrefabs[i]);
            }
        }
    }













    //BUTTON
    public void UpPriceArrow()
    {
        if (user.GetTicketPrice() < 5000)
        {
            user.SetTicketPrice(user.GetTicketPrice() + 1);
            UpdatePriceBox();
        }
        
    }
    public void DownPriceArrow()
    {
        if (user.GetTicketPrice() > 1)
        {
            user.SetTicketPrice(user.GetTicketPrice() - 1);
            UpdatePriceBox();
        }
        
    }
    public void CloseButton()
    {
        homeCode.GoToStadiumScreen(gameObject);
    }
    public void InfoButtonSelect(bool turnoff = false)
    {
        if (turnoff)
        {
            infoGameobject.SetActive(false);
            ticketPriceGameobject.SetActive(true);
            infoIconImage.sprite = infoIcon;
            infoButtonON = false;

            return;
        }

        if (infoButtonON)
        {
            infoButtonON = false;

            infoGameobject.SetActive(false);
            ticketPriceGameobject.SetActive(true);
            infoIconImage.sprite = infoIcon;
        }
        else
        {
            //CLICK TURN ON
            infoButtonON = true;

            infoTextbox.text = user.GetCurrentStadium().benifitsDef;
            infoGameobject.SetActive(true);
            ticketPriceGameobject.SetActive(false);
            infoIconImage.sprite = ticketIcon;

        }
    }

    public void UpgradeButton()
    {
        selectedUpgrade.PurchaseStadium();
        UpdateInfoSide();
        UpgradeSelected(selectedUpgrade);
    }

    public void UpgradeToBudgetButton()
    {
        if (isUpgradeScreenOpen)
        {
            isUpgradeScreenOpen = false;
            upgradeBudgetScreens[0].SetActive(false);
            upgradeBudgetScreens[1].SetActive(true);
            upgradeIconButton.sprite = upgradeBudgetIcon[0];
        }
        else
        {
            isUpgradeScreenOpen = true;
            upgradeBudgetScreens[0].SetActive(true);
            upgradeBudgetScreens[1].SetActive(false);
            upgradeIconButton.sprite = upgradeBudgetIcon[1];

        }
    }
}
