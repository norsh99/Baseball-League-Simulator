using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamsAllDivisions
{
    private User user;
    public Team userTeam;

    public List<Team> division_A1;
    public List<Team> division_A2; 

    private List<string> divisionNames_A1;
    private List<string> divisionNames_A2;

    private HomeCode hc;

    public TeamsAllDivisions(User user, HomeCode hc)
    {
        this.hc = hc;
        this.user = user;
        userTeam = user.GetTeam();

        divisionNames_A1 = new List<string>() { "Astros", "Expos", "Giants", "Marlins" };
        divisionNames_A2 = new List<string>() { "Cubs", "White Sox", "Red Sox", "Yankees", "Braves" };


        //Create all teams and assign to divisions
        division_A1 = GenerateNewDivisionTeams(divisionNames_A1);
        division_A2 = GenerateNewDivisionTeams(divisionNames_A2);
        division_A1.Add(userTeam);
    }

    private List<Team> GenerateNewDivisionTeams(List<string> names)
    {
        List<Team> newList = new List<Team>();
        for (int i = 0; i < names.Count; i++)
        {
            Team newTeam = new Team(names[i], hc);
            newTeam.CreateBrandNewTeamAllDefensePositionsFilled();
            newTeam.SetPlayersToDefPositions();
            newTeam.SetBattingLineUp();
            newList.Add(newTeam);
        }
        return newList;
    }
    public List<Team> GetAllTeamsExcludingUserTeam()
    {
        List<List<Team>> newList = new List<List<Team>>() {division_A1, division_A2 };
        return CollectAllTeams(newList, false);
    }
    public List<Team> GetAllTeamsIncludeUserTeam()
    {
        List<List<Team>> newList = new List<List<Team>>() { division_A1, division_A2 };
        return CollectAllTeams(newList, true);
    }

    private List<Team> CollectAllTeams(List<List<Team>> allDivisions, bool includeUserTeam)
    {
        List<Team> newList = new List<Team>();

        for (int i = 0; i < allDivisions.Count; i++)
        {
            for (int m = 0; m < allDivisions[i].Count; m++)
            {
                Team selectedTeam = allDivisions[i][m];
                if (!includeUserTeam)
                {
                    if (selectedTeam != user.GetTeam())
                    {
                        newList.Add(selectedTeam);

                    }
                }
                else
                {
                    newList.Add(selectedTeam);
                }
                
            }
        }
        return newList;
    }
}
