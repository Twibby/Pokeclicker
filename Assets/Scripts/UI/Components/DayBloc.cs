using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DayBloc : MonoBehaviour
{
    public TMPro.TMP_Text DateLabel;

    public Transform DealLinesParent;
    public GameObject DealLinePrefab;

    public GameObject NoDealPanel;

    private List<DailyDeal> dayDeals;
    
    public void Init(DateTime date, List<DailyDeal> deals, List<UndergroundItem> filtersList = null, bool hideEmptyDays = false)
    {
        DateLabel.text = date.ToShortDateString();

        dayDeals = new List<DailyDeal>(deals);

        displayLines(filtersList, hideEmptyDays);
    }
    
    public void ApplyFilters(List<UndergroundItem> filtersList, bool hideEmptyDays = false)
    {
        displayLines(filtersList, hideEmptyDays);
    }

    public void ShowDeals(bool value)
    {
        DealLinesParent.gameObject.SetActive(value);
    }

    public void MaskEmptyDay(bool isActive)
    {
        if (isActive && NoDealPanel.activeInHierarchy)
            this.gameObject.SetActive(false);
        else
            this.gameObject.SetActive(true);
    }

    private void displayLines(List<UndergroundItem> filtersList, bool maskEmpty = false)
    {
        foreach (DealLine line in DealLinesParent.GetComponentsInChildren<DealLine>(true))
        {
            GameObject.Destroy(line.gameObject);
        }

        List<DailyDeal> displayedDeals = new List<DailyDeal>();
        if (filtersList == null || filtersList.Count == 0)
        {
            displayedDeals = new List<DailyDeal>(dayDeals);
        }
        else
        {
            foreach (DailyDeal deal in dayDeals)
            {
                if (filtersList.Exists(x => x.DisplayName == deal.item2.DisplayName))
                    displayedDeals.Add(deal);
            }
        }

        NoDealPanel.SetActive(displayedDeals.Count == 0);
        DealLinesParent.gameObject.SetActive(displayedDeals.Count > 0);

        bool zebra = false;
        foreach (DailyDeal deal in displayedDeals)
        {
            GameObject line = GameObject.Instantiate(DealLinePrefab, DealLinesParent);
            line.GetComponent<DealLine>().Init(deal, zebra, false);

            zebra = !zebra;
        }

        this.gameObject.SetActive(true);
        if (maskEmpty && displayedDeals.Count == 0)
            this.gameObject.SetActive(false);
    }
}
