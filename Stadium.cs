using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Stadium")]
public class Stadium : ScriptableObject
{
    public string stadiumName;
    public int maxAttendance;
    public int levelType;
    public int priceToPurchase;
    public int recurringCosts;
    public int suggestedTicketPrice;


    [TextArea(5, 10)]
    public string benifitsDef;

    public bool hasBeenPurchased;

    public void PurchaseStadium()
    {
        hasBeenPurchased = true;
    }
    public bool GetHasBeenPurchased() { return hasBeenPurchased; }

    public Stadium(Stadium oldStadium)
    {
        stadiumName = oldStadium.stadiumName;
        maxAttendance = oldStadium.maxAttendance;
        levelType = oldStadium.levelType;
        priceToPurchase = oldStadium.priceToPurchase;
        recurringCosts = oldStadium.recurringCosts;
        benifitsDef = oldStadium.benifitsDef;
        hasBeenPurchased = oldStadium.hasBeenPurchased;
        suggestedTicketPrice = oldStadium.suggestedTicketPrice;
    }
}
