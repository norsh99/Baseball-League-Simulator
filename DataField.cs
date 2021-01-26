using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataField : MonoBehaviour
{
    private List<GameObject> allLines;
    public GameObject lineGameObjectPrefab;
    private List<Stats> allStats;

    public void SetInfo(List<Stats> allStats)
    {
        this.allStats = allStats;
        DestroyTabs();
        CreateTabs();
    }

    private void CreateTabs()
    {
        if (allLines == null)
        {
            allLines = new List<GameObject>();
        }


        for (int i = 0; i < allStats.Count+1; i++)
        {
            GameObject line = Instantiate(lineGameObjectPrefab, new Vector3(), Quaternion.identity, this.transform);
            allLines.Add(line);
            InfoSingleLine8Prefab newLine = line.GetComponent<InfoSingleLine8Prefab>();
           
            if (i == 0)
            {
                newLine.LoadInfo(allStats[0].ReturnOffenseHeaders());
            }
            else
            {
                newLine.LoadInfo(allStats[i-1].ReturnOffenseStats());
            }
            
        }
    }
    private void DestroyTabs()
    {
        if (allLines == null)
        {
            allLines = new List<GameObject>();
        }

        if (allLines.Count > 0)
        {
            for (int i = 0; i < allLines.Count; i++)
            {
                Destroy(allLines[i]);
            }
        }
    }
}
