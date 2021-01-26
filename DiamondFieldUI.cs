using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiamondFieldUI : MonoBehaviour
{

    public List<Image> baseIconList;
    public List<DiamondTextScript> textList;
    public List<Color> baseSelectionColors;

    //1: P, 2: C, 3: 3rd, 4: SS, 5: 2nd, 6: 1st, 7: LF, 8: CF, 9: RF OLD!!!!
    //1: C, 2: 3rd, 3: P, 4: SS, 5: 2nd, 6: 1st, 7: LF, 8: CF, 9: RF NEW


    public void UpdatePosition(int position, Player player)
    {
        textList[position - 1].UpdateText(player.GetNameOfPlayer());
        baseIconList[position - 1].color = baseSelectionColors[0];
    }
    public void RemovePosition(int position)
    {
        textList[position - 1].UpdateText("");
        baseIconList[position - 1].color = baseSelectionColors[1];
    }

    public void SetAllPositionsEmpty()
    {
        for (int i = 0; i < 9; i++)
        {
            RemovePosition(i+1);
        }
    }
}
