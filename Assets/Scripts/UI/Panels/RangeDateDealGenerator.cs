using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class RangeDateDealGenerator : DailyDealsGenerator
{
    [Header("Dropdowns")]
    public TMPro.TMP_Dropdown StartYearDropdown, StartMonthDropdown, StartDayDropdown;
    public TMPro.TMP_Dropdown EndYearDropdown, EndMonthDropdown, EndDayDropdown;

    [Space(2)]
    public Transform DealsParent;
    public Toggle HideEmptyDaysToggle;

    [Header("Filter Panel")]
    public GameObject FilterPanel;

    private SortedDictionary<DateTime, List<DailyDeal>> allDeals = new SortedDictionary<DateTime, List<DailyDeal>>();
    private List<UndergroundItem> filtersList = new List<UndergroundItem>();

    public override void Initialize ()
    {
        Debug.Log("Initializing Range Date Panel");

        int year = DateTime.Now.Year;
        {
            StartYearDropdown.ClearOptions();
            for (int y = 0; y < 10; y++)
            {
                StartYearDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData((y + year).ToString()));
            }
            StartYearDropdown.value = 0;

            StartMonthDropdown.ClearOptions();
            for (int m = 1; m <= 12; m++)
            {
                StartMonthDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)));
            }
            StartMonthDropdown.value = DateTime.Now.Month - 1;

            initStartDaysDropdown();
            StartDayDropdown.value = DateTime.Now.Day - 1;
        }

        {
            EndYearDropdown.ClearOptions();
            for (int y = 0; y < 10; y++)
            {
                EndYearDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData((y + year).ToString()));
            }
            EndYearDropdown.value = 0;

            EndMonthDropdown.ClearOptions();
            for (int m = 1; m <= 12; m++)
            {
                EndMonthDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)));
            }
            EndMonthDropdown.value = DateTime.Now.Month - 1;

            initEndDaysDropdown();
            EndDayDropdown.value = DateTime.Now.Day - 1;
        }

        StartYearDropdown.onValueChanged.AddListener(x => initStartDaysDropdown());
        StartMonthDropdown.onValueChanged.AddListener(x => initStartDaysDropdown());

        EndYearDropdown.onValueChanged.AddListener(x => initEndDaysDropdown());
        EndMonthDropdown.onValueChanged.AddListener(x => initEndDaysDropdown());

        foreach (DayBloc day in DealsParent.GetComponentsInChildren<DayBloc>(true)) 
        {
            Destroy(day.gameObject); 
        }

        allDeals = new SortedDictionary<DateTime, List<DailyDeal>>();
        filtersList = new List<UndergroundItem>();

        HideEmptyDaysToggle.isOn = false;
    }

    void initStartDaysDropdown()
    {
        int tmpVal = StartDayDropdown.value;

        StartDayDropdown.ClearOptions();
        for (int d = 1; d <= DateTime.DaysInMonth(DateTime.Now.Year+StartYearDropdown.value, StartMonthDropdown.value+1); d++)
        {
            StartDayDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(d.ToString()));
        }

        StartDayDropdown.value = Mathf.Min(tmpVal, StartDayDropdown.options.Count - 1);
    }

    void initEndDaysDropdown()
    {
        int tmpVal = EndDayDropdown.value;

        EndDayDropdown.ClearOptions();
        for (int d = 1; d <= DateTime.DaysInMonth(DateTime.Now.Year + EndYearDropdown.value, EndMonthDropdown.value + 1); d++)
        {
            EndDayDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(d.ToString()));
        }

        EndDayDropdown.value = Mathf.Min(tmpVal, EndDayDropdown.options.Count - 1);
    }

    #region UI functions
    
    public void OpenFilterPanel()
    {
        FilterPanel.SetActive(true);

        // TODO Init underground items list
    }

    public void OnApplyFilters()
    {
        // TODO update filtersList 

        foreach (DayBloc day in DealsParent.GetComponentsInChildren<DayBloc>(true))
        {
            day.ApplyFilters(filtersList, HideEmptyDaysToggle.isOn);
        }
    }

    public void OnMaskEmptyDays(bool isOn)
    {
        foreach (DayBloc day in DealsParent.GetComponentsInChildren<DayBloc>(true))
        {
            day.MaskEmptyDay(isOn);
        }
    }
    
    public void OnGenerateClick()
    {
        StartCoroutine(coGenerateClick());
    }

    #endregion

    IEnumerator coGenerateClick()
    {
        UIManager.Instance.LoadingActivation(true);
        yield return new WaitForEndOfFrame();

        allDeals = new SortedDictionary<DateTime, List<DailyDeal>>();

        foreach (DayBloc day in DealsParent.GetComponentsInChildren<DayBloc>(true)) { Destroy(day.gameObject); }


        DateTime startDate = new DateTime(DateTime.Now.Year + StartYearDropdown.value, StartMonthDropdown.value + 1, StartDayDropdown.value + 1);
        DateTime endDate = new DateTime(DateTime.Now.Year + EndYearDropdown.value, EndMonthDropdown.value + 1, EndDayDropdown.value + 1, 1, 0, 0);

        if (endDate < startDate)
        {
            Debug.LogError("End date is anterior to startDat, no generation");

            yield return new WaitForEndOfFrame();

            UIManager.Instance.LoadingActivation(false);

            yield break; // TODO error popup
        }

        Debug.Log("Generating range from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString());

       
        int safetyCount = 0;
        for (DateTime day = startDate; day < endDate; day = day.AddDays(1))
        {
            List<DailyDeal> dailyDeals = DailyDeal.GenerateDeals(PlayerSettings.MaxDeals, day);
            
            allDeals.Add(day, dailyDeals);

            GameObject dayBloc = GameObject.Instantiate(DayBlocPrefab, DealsParent);
            dayBloc.name = "Deals_Of_" + day.ToString("yyyy-MM-dd");
            dayBloc.GetComponent<DayBloc>().Init(day, dailyDeals, filtersList, HideEmptyDaysToggle.isOn);

            safetyCount++;
            if (safetyCount > 1)   // arbitrary value to avoid all instantiations in same frame
            {
                safetyCount = 0;
                yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        UIManager.Instance.LoadingActivation(false);

    }

}
