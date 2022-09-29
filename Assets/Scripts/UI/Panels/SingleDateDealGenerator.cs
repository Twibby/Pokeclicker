using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SingleDateDealGenerator : DailyDealsGenerator
{
    public TMPro.TMP_Dropdown YearDropdown, MonthDropdown, DayDropdown;
    public Transform DealsParent;


    public override void Initialize ()
    {
        Debug.Log("Initializing SingleDate Panel");

        int year = DateTime.Now.Year;
        YearDropdown.ClearOptions();
        for (int y = 0; y < 10; y++)
        {
            YearDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData((y + year).ToString()));
        }
        YearDropdown.value = 0;

        MonthDropdown.ClearOptions();
        for (int m = 1; m <= 12; m++)
        {
            MonthDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)));
        }
        MonthDropdown.value = DateTime.Now.Month - 1;
        
        initDaysDropdown();
        DayDropdown.value = DateTime.Now.Day - 1;
        
        YearDropdown.onValueChanged.AddListener(x => initDaysDropdown());
        MonthDropdown.onValueChanged.AddListener(x => initDaysDropdown());

        DisplayDeals(DateTime.Now, new List<DailyDeal>(), DealsParent);
    }

    void initDaysDropdown()
    {
        int tmpVal = DayDropdown.value;

        DayDropdown.ClearOptions();
        for (int d = 1; d <= DateTime.DaysInMonth(DateTime.Now.Year+YearDropdown.value, MonthDropdown.value+1); d++)
        {
            DayDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(d.ToString()));
        }

        DayDropdown.value = Mathf.Min(tmpVal, DayDropdown.options.Count - 1);

    }

    public void OnGenerateClick()
    {
        DateTime date = new DateTime(DateTime.Now.Year + YearDropdown.value, MonthDropdown.value + 1, DayDropdown.value + 1);

        Debug.LogWarning(date.ToLongDateString() + "   ---   " + date.ToShortDateString());


        DisplayDeals(date, DailyDeal.GenerateDeals(PlayerSettings.MaxDeals, date), DealsParent);
    }

    public void DisplayDeals(DateTime date, List<DailyDeal> deals, Transform parent)
    {
        foreach (DayBloc day in parent.GetComponentsInChildren<DayBloc>(true)) { Destroy(day.gameObject); }

        if (deals.Count == 0)
            return;

        GameObject dayBloc = GameObject.Instantiate(DayBlocPrefab, parent);
        dayBloc.name = "Deals_Of_" + date.ToString("yyyy-MM-dd");
        dayBloc.GetComponent<DayBloc>().Init(date, deals);
    }

}
