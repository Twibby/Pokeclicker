using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChainBloc : MonoBehaviour
{
    public TMPro.TMP_Text ProfitLabel;

    public Transform DealLinesParent;
    public GameObject DealLinePrefab;

    private DealChain myDealChain;
    
    public void Init(DealChain chain)
    {
        string numberOfDays = "";
        if (chain.Deals.Count > 1)
        {
            numberOfDays = " over " + ((chain.Deals[chain.Deals.Count - 1].date - chain.Deals[0].date).TotalDays +1).ToString() + " days";
        }        

        ProfitLabel.text = "Profit of <b>" + chain.Profit + " " + chain.Deals[chain.Deals.Count-1].item2.DisplayName + "</b> in " + chain.Deals.Count + " deal(s)" + numberOfDays;

        myDealChain = chain;

        displayLines();
    }

    public void ShowDeals(bool value)
    {
        DealLinesParent.gameObject.SetActive(value);
    }

    private void displayLines()
    {
        foreach (DealLine line in DealLinesParent.GetComponentsInChildren<DealLine>(true))
        {
            GameObject.Destroy(line.gameObject);
        }

        bool zebra = false;
        foreach (DailyDeal deal in myDealChain.Deals)
        {
            GameObject line = GameObject.Instantiate(DealLinePrefab, DealLinesParent);
            line.GetComponent<DealLine>().Init(deal, zebra, true);

            zebra = !zebra;
        }
    }
}
