using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSchedulePanel : MonoBehaviour
{
    public GameObject verticalLayoutPanel;
    public GameObject singleCell;

    private List<GameObject> colList;
    private List<GameObject> nameColCells;


    public Transform nameCol;
    public Transform container;

    private List<Team> allTeam;

    public Color[] colorBackground;

    public void LoadInfo(List<Team> allTeam, int weekNum, int maxWeeks)
    {
        if (weekNum > maxWeeks)
        {
            weekNum = maxWeeks;
        }
        this.allTeam = allTeam;

        DestroyCol();
        DestroyNameColCells();

        CreateCol(allTeam[0].GetCurrentSeasonEntireSchedule().Count);
        CreateCells(weekNum);

    }


    private void CreateCells(int weekNum)
    {
        nameColCells = new List<GameObject>();


        //Instantiating the name col. Add one black cell first
        GameObject blankCell = Instantiate(singleCell, new Vector3(), Quaternion.identity, nameCol);
        blankCell.GetComponent<ScheduleCellTemplate>().LoadInfo("", colorBackground[1]);
        nameColCells.Add(blankCell);
        for (int i = 0; i < allTeam.Count; i++)
        {
            GameObject newCell = Instantiate(singleCell, new Vector3(), Quaternion.identity, nameCol);
            newCell.GetComponent<ScheduleCellTemplate>().LoadInfo(allTeam[i].GetName(), colorBackground[1]);

            nameColCells.Add(newCell);
        }



        //Instantiate all the other cells


        for (int col = 0; col < colList.Count; col++)
        {
            GameObject weekCell = Instantiate(singleCell, new Vector3(), Quaternion.identity, colList[col].transform);
            if (weekNum == col+1)
            {
                weekCell.GetComponent<ScheduleCellTemplate>().LoadInfo((col + 1).ToString(), colorBackground[0]);
            }
            else
            {
                weekCell.GetComponent<ScheduleCellTemplate>().LoadInfo((col + 1).ToString(), colorBackground[1]);

            }
            for (int i = 0; i < allTeam.Count; i++)
            {
                TeamVsTeam matchup = allTeam[i].GetCurrentSeasonWeeklyMatchup(col);


                GameObject newCell = Instantiate(singleCell, new Vector3(), Quaternion.identity, colList[col].transform);
                if (weekNum == col + 1)
                {
                    newCell.GetComponent<ScheduleCellTemplate>().LoadInfo(matchup.awayTeam.GetName() + "\n" + matchup.homeTeam.GetName(), colorBackground[0]);

                }
                else
                {
                    newCell.GetComponent<ScheduleCellTemplate>().LoadInfo(matchup.awayTeam.GetName() + "\n" + matchup.homeTeam.GetName(), colorBackground[1]);

                }
            }
        }
    }



    private void CreateCol(int amount)
    {
        if (colList == null)
        {
            colList = new List<GameObject>();
        }


        for (int i = 0; i < amount; i++)
        {
            GameObject col = Instantiate(verticalLayoutPanel, new Vector3(), Quaternion.identity, container);
            colList.Add(col);
        }
    }

    private void DestroyCol()
    {
        if (colList == null)
        {
            colList = new List<GameObject>();
        }

        if (colList.Count > 0)
        {
            for (int i = 0; i < colList.Count; i++)
            {
                Destroy(colList[i]);
            }
        }
        colList = new List<GameObject>();
    }

    private void DestroyNameColCells()
    {
        if (nameColCells == null)
        {
            nameColCells = new List<GameObject>();
        }

        if (nameColCells.Count > 0)
        {
            for (int i = 0; i < nameColCells.Count; i++)
            {
                Destroy(nameColCells[i]);
            }
        }
        nameColCells = new List<GameObject>();
    }

}
