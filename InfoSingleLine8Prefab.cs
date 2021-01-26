using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoSingleLine8Prefab : MonoBehaviour
{
    public TextMeshProUGUI[] textBoxes;

    public void LoadInfo(List<string> info)
    {
        for (int i = 0; i < textBoxes.Length; i++)
        {
            if (i < info.Count)
            {
                textBoxes[i].gameObject.SetActive(true);
                textBoxes[i].text = info[i].ToString();
            }
            else
            {
                textBoxes[i].gameObject.SetActive(false);
                textBoxes[i] = null;
            }

        }
    }
}
