using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AthleticBadge : MonoBehaviour
{


    public List<Sprite> allBadge;
    public List<Sprite> allStars;
    public List<Color> colorRankings;
    public Image badge;
    public Image stars;

    public void UpdateBadgeAndStars(int levelRating, int athleticRating)
    {
        stars.sprite = allStars[levelRating-1];

        //0-3 = normal; 4-6 = bronze; 7-8 = silver; 9-10 = gold
        if (athleticRating <= 3)
        {
            badge.sprite = allBadge[0];
            stars.color = colorRankings[0];
        }
        else if(athleticRating >= 4 && athleticRating <= 6)
        {
            badge.sprite = allBadge[1];
            stars.color = colorRankings[1];
        }
        else if (athleticRating >= 7 && athleticRating <= 8)
        {
            badge.sprite = allBadge[2];
            stars.color = colorRankings[2];
        } else
        {
            badge.sprite = allBadge[3];
            stars.color = colorRankings[3];
        }

    }

    public void SetStarColor(Color newColor)
    {
        stars.color = newColor;
    }




}
