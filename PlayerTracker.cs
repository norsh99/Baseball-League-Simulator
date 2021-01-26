using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTracker : MonoBehaviour
{
    public List<Player> playerInfoList;
    public List<GameObject> listOfTabs;
    public GameObject playerTrackerTabPrefab;
    public GameObject windowGameObject;
    public TextMeshProUGUI teamNameText;

    public Color selectColor;
    public Color defautColor;

    public PlayerTrackerPrefab selected;
    public PlayerTrackerPrefab prevSelected;
    public PlayerInfoPanel playerInfoPanel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadInfo(string teamName, List<Player> theTeam)
    {
        playerInfoList = theTeam;
        teamNameText.text = teamName;


        CreateTabs();

    }

    private void CreateTabs()
    {
        for (int i = 0; i < playerInfoList.Count; i++)
        {
            GameObject tab = Instantiate(playerTrackerTabPrefab, new Vector3(), Quaternion.identity, windowGameObject.transform);
            listOfTabs.Add(tab);
            Player currentPlayer = playerInfoList[i];
            tab.GetComponent<PlayerTrackerPrefab>().AsignInfo(i+1, currentPlayer.GetNameOfPlayer(), this, currentPlayer, playerInfoPanel);
        }
    }

    public void ChangeSelection(PlayerTrackerPrefab thePrefab)
    {
        selected = thePrefab;
        prevSelected?.DeSelect();
        prevSelected = thePrefab;
    }
}
