using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HomeCode : MonoBehaviour
{
    //1: P, 2: C, 3: 3rd, 4: SS, 5: 2nd, 6: 1st, 7: LF, 8: CF, 9: RF 
    public User user;
    public GameObject currentOpenMenu;
    public UserBarUI userBarUI;
    public GameObject backButton;
    public GameObject stadiumButton;

    public Season currentSeason;

    public HomePage homePage;



    //All Teams Stored Here
    public TeamsAllDivisions allDivisions;


    //Stadiums
    private bool stadiumScreenActive = false;
    private List<Stadium> allStadiums;
    public List<Stadium> sciptObjectStadiums;


    //Team Info
    private List<Team> allTeams;
    List<Player> allPlayers;


    //Player Names
    public List<PlayerNames> allNames;



    //Trade Screen
    private bool isTradeScreenOpen;
    public GameObject tradeButton;

    //Starting Values
    public float startingMoney = 100000;



    void Start()
    {
        allNames = ImportNames();
        StadiumButtonSwitch(false);
        allStadiums = LoadInStadiums();
        isTradeScreenOpen = false;
        tradeButton.SetActive(false);
    }

    private List<PlayerNames> ImportNames()
    {
        List<PlayerNames> finalList = new List<PlayerNames>();
        TextAsset firstNameData = Resources.Load<TextAsset>("DataFields/MaleNameLastName");
        string[] data = firstNameData.text.Split(new char[] { '\n' });
        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            PlayerNames newName = new PlayerNames();
            newName.firstName = row[0];
            newName.lastName = row[1];
            finalList.Add(newName);
        }
        return finalList;
    }

    public void CreateUserTeamsAndSeason(string pickName)
    {
        user = new User(pickName, this, allStadiums[0]);
        allDivisions = new TeamsAllDivisions(user, this);
        CreateNewSeason();

        user.onStatsChange += UpdateUserBarUI;
        backButton.SetActive(false);
        UpdateUserBarUI();
        GenerateNewTeams(6);
        FirstRoundGameStats(); //Temp for now
        allPlayers = CollectAllPlayersFromTeams(allTeams);
    }

    public void CreateNewSeason()
    {
        currentSeason = new Season(user, allDivisions, this);

    }
    public List<Stadium> GetAllStadiums() { return allStadiums; }
    private List<Stadium> LoadInStadiums()
    {
        List<Stadium> newStadiumList = new List<Stadium>();
        for (int i = 0; i < sciptObjectStadiums.Count; i++)
        {
            Stadium newStadium = new Stadium(sciptObjectStadiums[i]);
            newStadiumList.Add(newStadium);
        }
        return newStadiumList;
    }

    public void TransitionMenus(GameObject closeMenu, GameObject openMenu)
    {
        closeMenu.SetActive(false);
        openMenu.SetActive(true);
        currentOpenMenu = openMenu;
    }


    private List<Player> CollectAllPlayersFromTeams(List<Team> allTeams)
    {
        List<Player> allPlayers = new List<Player>();
        for (int i = 0; i < allTeams.Count; i++)
        {
            allPlayers.AddRange(allTeams[i].GetAllPlayers());
        }
        return allPlayers;
    }

    private void FirstRoundGameStats()
    {
        TeamVSTeam(allTeams[0], allTeams[1]);
        TeamVSTeam(allTeams[2], allTeams[3]);
        TeamVSTeam(allTeams[4], allTeams[5]);
    }

    private void TeamVSTeam(Team team1, Team team2)
    {
        team1.SetStartingLineUp(team1.GetAllPlayers());
        team2.SetStartingLineUp(team2.GetAllPlayers());

        Game game = new Game(team1, team2);
        game.PlayEntireGame();
    }

    private void GenerateNewTeams(int amount)
    {
        allTeams = new List<Team>();
        for (int i = 0; i < amount; i++)
        {
            Team newTeam = new Team(null, this);
            newTeam.CreateBrandNewTeam();
            allTeams.Add(newTeam);
        }
    }

    public void UpdateUserBarUI()
    {
        userBarUI.UpdateUI(user);
    }

    private void PrintPlayerList(List<Player> theList)
    {
        for (int i = 0; i < theList.Count; i++)
        {
            Debug.Log("Player: " + theList[i].GetNameOfPlayer());
        }
    }

    
    private void SimulateWeekOfGames()
    {
        List<WeeklyMatchups> weekOfGames = currentSeason.currentSeasonSchedule.GetWeeklyMatchups();

        List<TeamVsTeam> thisWeek = weekOfGames[currentSeason.currentSeasonSchedule.GetWeekIndex()-1].GetWeekMatchup();
        for (int i = 0; i < thisWeek.Count; i++)
        {
            if (!thisWeek[i].isGameComplete)
            {
            Team awayTeam = thisWeek[i].awayTeam;
            Team homeTeam = thisWeek[i].homeTeam;

            Gamev2 newGame = new Gamev2(awayTeam, homeTeam);
            newGame.PlayEntireGame();
            }
        }
    }






    public void StadiumButtonSwitch(bool turnOn)
    {
        stadiumButton.SetActive(turnOn);

    }




    //Buttons
    public void GotoDraftScreenButton(GameObject menu)
    {
        TransitionMenus(currentOpenMenu, menu);
        
        menu.GetComponent<PickPlayersScreen>().LoadSceen(user.GetTeam(), allPlayers);
        backButton.SetActive(false);
    }

    public void GoToManageTeamScreen(GameObject menu)
    {
        TransitionMenus(currentOpenMenu, menu);
        menu.GetComponent<ManageTeamScreen>().LoadIn();
        backButton.SetActive(true);

    }

    public void GoHomeButton(GameObject menu)
    {
        TransitionMenus(currentOpenMenu, menu);
    }

    public void GoStartGame(GameObject menu)
    {
        if (currentSeason.currentSeasonSchedule.GetWeekIndex() <= currentSeason.currentSeasonSchedule.GetMaxWeeksInSeason())
        {
            TransitionMenus(currentOpenMenu, menu);

            TeamVsTeam currentMatchup = user.GetTeam().GetCurrentSeasonWeeklyMatchup(currentSeason.currentSeasonSchedule.GetWeekIndex() - 1);
            Team awayTeam = currentMatchup.awayTeam;
            Team homeTeam = currentMatchup.homeTeam;
            currentMatchup.isGameComplete = true;

            if (user.GetTeam().GetStartingLineUp() == null)
            {
                user.GetTeam().SetBattingLineUp();
            }

            menu.GetComponent<PlayGameScreen>().LoadIn(awayTeam, homeTeam);
            SimulateWeekOfGames();
            currentSeason.currentSeasonSchedule.AddWeek();
        }
        else
        {
            homePage.ActivateNewSeasonWarning();
        }
        homePage.UpdateHeader();
    }
    public void TestButton(GameObject menu)
    {
        TransitionMenus(currentOpenMenu, menu);
        Team testTeam1 = new Team("Giants", this);
        Team testTeam2 = new Team("Cardinals", this);
        testTeam1.CreateBrandNewTeamAllDefensePositionsFilled();
        testTeam2.CreateBrandNewTeamAllDefensePositionsFilled();

        testTeam1.SetPlayersToDefPositions();
        testTeam2.SetPlayersToDefPositions();


        testTeam1.SetBattingLineUp();
        testTeam2.SetBattingLineUp();

        menu.GetComponent<PlayGameScreen>().LoadIn(testTeam1, testTeam2);

    }

    public void GoToStandingsPage(GameObject menu)
    {
        TransitionMenus(currentOpenMenu, menu);
        backButton.SetActive(true);
        menu.GetComponent<StandingsPage>().LoadInfo();
    }

    public void GoToStadiumScreen(GameObject menu)
    {
        if (user != null && !isTradeScreenOpen)
        {
            if (stadiumScreenActive)
            {
                menu.SetActive(false);
                stadiumScreenActive = false;
            }
            else
            {
                menu.SetActive(true);
                menu.GetComponent<StadiumSelectScreen>().LoadInfo();
                stadiumScreenActive = true;
            }
        }
    }
    public void GoToTradeScreen(GameObject menu)
    {
        if (user != null)
        {
            if (isTradeScreenOpen)
            {
                menu.SetActive(false);
                isTradeScreenOpen = false;
                backButton.SetActive(true);
            }
            else
            {
                menu.SetActive(true);
                menu.GetComponent<TradeScreen>().LoadInfo();
                isTradeScreenOpen = true;
                backButton.SetActive(false);

            }
        }
    }
}
