using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsRecapPanel : MonoBehaviour
{
    public GameObject recapBarPrefab;
    public Transform container;

    public List<GameObject> allBars;
    public void LoadInfo(List<Player> listOfPlayersOnTeam)
    {
        DestroyBars();
        CreateBars(listOfPlayersOnTeam);

    }


    private void CreateBars(List<Player> theList)
    {
        if (allBars == null)
        {
            allBars = new List<GameObject>();
        }


        for (int i = 0; i < theList.Count; i++)
        {
            GameObject bar = Instantiate(recapBarPrefab, new Vector3(), Quaternion.identity, container);

            float timer = i * .3f;

            bar.GetComponent<StatsEndScreenBar>().LoadInfo(theList[i], timer);
            allBars.Add(bar);

        }
    }

    private void DestroyBars()
    {
        if (allBars == null)
        {
            allBars = new List<GameObject>();
        }

        if (allBars.Count > 0)
        {
            for (int i = 0; i < allBars.Count; i++)
            {
                Destroy(allBars[i]);
            }
        }
    }


}
