using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HomePage : MonoBehaviour
{
    public HomeCode homeCode;

    public List<GameObject> homeMenus;
    public List<GameObject> introPages;
    public List<GameObject> fullGameMenus;


    public TextMeshProUGUI congratsTeamNameTextBox;
    public GameObject submitNameButton;
    public GameObject errorMessage;
    public TextMeshProUGUI weekTitleHeader;



    public TMP_InputField inputField;


    private int continueButtonIndex = 0;

    //New Season
    public GameObject newSeasonMessage;
    public TextMeshProUGUI newSeasonCostText;
    public Color[] warningColors; 



    private void Update()
    {
        if (inputField.text.Length > 0)
        {
            submitNameButton.SetActive(true);
        }
        else
        {
            submitNameButton.SetActive(false);

        }
    }

    public void CheckIfWholeTeamIsDrafted()
    {
        if (homeCode.user.GetTeam().GetAllPlayers().Count != 9)
        {
            errorMessage.SetActive(true);
        }
        else
        {
            errorMessage.SetActive(false);

        }
    }

    public void EnableFullGameMenu()
    {
        fullGameMenus[0].SetActive(true);
        fullGameMenus[1].SetActive(true);
        fullGameMenus[2].SetActive(true);
        fullGameMenus[3].SetActive(false);

    }

    public void ActivateNewSeasonWarning()
    {
        float totalAmount = homeCode.user.GetTotalAmountDueEachNewSeason();
        newSeasonCostText.text = "It will cost " + totalAmount.ToString("C0") + " to progress to the next season.";
        newSeasonMessage.SetActive(true);
    }

    IEnumerator FlashWarning(TextMeshProUGUI textBox, Color yellowColor, Color redColor)
    {
        textBox.color = yellowColor;
        yield return new WaitForSeconds(.1f);
        textBox.color = redColor;
        yield return new WaitForSeconds(.1f);
        textBox.color = yellowColor;
        yield return new WaitForSeconds(.1f);
        textBox.color = redColor;
    }

    public void UpdateHeader()
    {
        int currentWeek = homeCode.currentSeason.currentSeasonSchedule.GetWeekIndex();
        int maxWeek = homeCode.currentSeason.currentSeasonSchedule.GetMaxWeeksInSeason();

        if (currentWeek <= maxWeek)
        {
            weekTitleHeader.text = "Week " + currentWeek;

        }
        if (currentWeek > maxWeek)
        {
            weekTitleHeader.text = "Current Season is Over";

        }
    }


    //Buttons--------------------------------------

    public void CreateNewTeamButton()
    {
        homeMenus[0].SetActive(false);
        homeMenus[1].SetActive(true);
        homeCode.backButton.SetActive(false);


    }
    public void SubmitNameButton()
    {
        homeCode.CreateUserTeamsAndSeason(inputField.text);


        congratsTeamNameTextBox.text = "Congrats " + homeCode.user.GetTeam().GetName() + "!";
        homeMenus[1].SetActive(false);
        homeMenus[2].SetActive(true);
        
    }
    public void ContinueButton()
    {
        if (continueButtonIndex == 0)
        {
            homeCode.user.AddMoney(homeCode.startingMoney);
            introPages[0].SetActive(false);
            introPages[1].SetActive(true);
            continueButtonIndex += 1;

        }
        else if (continueButtonIndex == 1)
        {
            
            homeMenus[2].SetActive(false);
            homeMenus[3].SetActive(true);
            UpdateHeader();

        }

    }

    public void YesButton()
    {
        if (homeCode.user.GetMoney() > homeCode.user.GetTotalAmountDueEachNewSeason())
        {
            newSeasonMessage.SetActive(false);
            homeCode.user.SpendMoney(homeCode.user.GetTotalAmountDueEachNewSeason(), true);
            //CREATE NEW SEASON!!!!!!!
            homeCode.CreateNewSeason();
            weekTitleHeader.text = "Week " + homeCode.currentSeason.currentSeasonSchedule.GetWeekIndex();

        }
        else
        {
            //Display Warning - Not enough funds.
            StartCoroutine(FlashWarning(newSeasonCostText, warningColors[0], warningColors[1]));
        }

    }
    public void NoButton()
    {
        newSeasonMessage.SetActive(false);

    }
}
