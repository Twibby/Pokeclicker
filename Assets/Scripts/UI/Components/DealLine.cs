using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealLine : MonoBehaviour
{
    public TMPro.TMP_Text Amount1Label, Item1Label;
    public TMPro.TMP_Text Amount2Label, Item2Label;
    
    private Color zebraColor = GlobalConstants.WhiteZebra;

    public void Init(DailyDeal deal, bool zebra = false)
    {
        this.Amount1Label.text = deal.amount1.ToString();
        this.Item1Label.text = deal.item1.ExtendedName;

        this.Amount2Label.text = deal.amount2.ToString();
        this.Item2Label.text = deal.item2.ExtendedName;

        if (PlayerSettings.ColorizeLabel)
        {
            this.Item2Label.color = deal.item2.GetItemColor();
            this.Item1Label.color = deal.item1.GetItemColor();
        }

        if (zebra)
            this.GetComponent<Image>().color = zebraColor;
    }
}
