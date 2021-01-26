using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamStandingsPanel : MonoBehaviour
{
    private List<TeamInfoCell> allCells;
    public Transform containerForCells;
    public TextMeshProUGUI headerButtonText;

    public GameObject teamInfoCellPrefab;

    private List<GameObject> spawnedCells;
    private StandingsPage standingsPage;
    public void LoadInfo(List<Team> divisionTeams, string divisionHeaderText, StandingsPage standingsPage)
    {
        this.standingsPage = standingsPage;
        DestroyTabs();
        headerButtonText.text = divisionHeaderText;
        spawnedCells = new List<GameObject>();
        allCells = new List<TeamInfoCell>();
        CreateCells(divisionTeams);
    }

    private void CreateCells(List<Team> theList)
    {
        if (spawnedCells == null)
        {
            spawnedCells = new List<GameObject>();
        }


        for (int i = 0; i < theList.Count; i++)
        {
            GameObject cell = Instantiate(teamInfoCellPrefab, new Vector3(), Quaternion.identity, containerForCells);
            spawnedCells.Add(cell);
            TeamInfoCell newCell = cell.GetComponent<TeamInfoCell>();
            allCells.Add(newCell);
            newCell.LoadInfo(theList[i].GetCurrentSeasonTeamStats(), standingsPage);

        }
    }

    private void DestroyTabs()
    {
        if (spawnedCells == null)
        {
            spawnedCells = new List<GameObject>();
        }

        if (spawnedCells.Count > 0)
        {
            for (int i = 0; i < spawnedCells.Count; i++)
            {
                Destroy(spawnedCells[i]);
            }
        }
    }
}
