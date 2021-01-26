using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public GameObject sCell;

    public ScoreboardCell nameCells;
    public ScoreboardCell runsCell;
    public ScoreboardCell hitsCell;

    public ScoreboardCell errorsCell;


    public List<ScoreboardCell> inningCell;
    public List<GameObject> allCells;


    public Transform holder;

    public void AddCell(string cell1, string cell2, string info)
    {
        GameObject cell = Instantiate(sCell, new Vector3(), Quaternion.identity, holder);
        allCells.Add(cell);
        cell.transform.SetSiblingIndex(cell.transform.GetSiblingIndex() - 3); //2 becasue of the runs and erros cells at the end
        ScoreboardCell newCell = cell.GetComponent<ScoreboardCell>();
        newCell.LoadInfo(info, cell1, cell2);
        inningCell.Add(newCell);
    }

    public void GameLauncher(string teamName1, string teamName2)
    {
        nameCells.LoadInfo("Team", teamName1, teamName2);
        runsCell.LoadInfo("R", "0", "0");
        hitsCell.LoadInfo("H", "0", "0");
        errorsCell.LoadInfo("E", "0", "0");



        inningCell = new List<ScoreboardCell>();
        for (int i = 0; i < 9; i++)
        {
            AddCell("", "", (i+1).ToString());
        }
    }

    public void ResetAllInningCells()
    {
        inningCell = new List<ScoreboardCell>();
        DestroyCells(allCells);
    }

    private void DestroyCells(List<GameObject> theList)
    {
        if (theList != null)
        {
            for (int i = 0; i < theList.Count; i++)
            {
                Destroy(theList[i]);
            }
        }

    }

    public void UpdateTextInCell(bool isTeam1, int inningNum, string score)
    {
        if (isTeam1)
        {
            inningCell[inningNum - 1].cellTextBox1.text = score;
        }
        else
        {
            inningCell[inningNum - 1].cellTextBox2.text = score;
        }
    }
    public void UpdateRunsText(bool isTeam1, string num)
    {
        if (isTeam1)
        {
            runsCell.cellTextBox1.text = num;
        }
        else
        {
            runsCell.cellTextBox2.text = num;
        }
    }
    public void UpdateErrorsText(bool isTeam1, string num)
    {
        if (isTeam1)
        {
            errorsCell.cellTextBox1.text = num;
        }
        else
        {
            errorsCell.cellTextBox2.text = num;
        }
    }
    public void UpdateHitsText(bool isTeam1, string num)
    {
        if (isTeam1)
        {
            hitsCell.cellTextBox1.text = num;
        }
        else
        {
            hitsCell.cellTextBox2.text = num;
        }
    }

    public List<ScoreboardCell> GetListScoreboardCells() { return inningCell; }

}
