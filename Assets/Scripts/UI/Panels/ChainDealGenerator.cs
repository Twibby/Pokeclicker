using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class ChainDealGenerator : DailyDealsGenerator
{
    [Header("Dropdowns")]
    public TMPro.TMP_Dropdown StartYearDropdown, StartMonthDropdown, StartDayDropdown;
    public TMPro.TMP_Dropdown EndYearDropdown, EndMonthDropdown, EndDayDropdown;

    [Space(2)]
    public Transform DealsParent;

    [Header("Filter Panel")]
    public FilterPanel PickItemPanel;

    [SerializeField] private UndergroundItem pickedItem;

    public override void Initialize()
    {
        Debug.Log("Initializing Deal Chain Panel");

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

        pickedItem = null;
    }

    void initStartDaysDropdown()
    {
        int tmpVal = StartDayDropdown.value;

        StartDayDropdown.ClearOptions();
        for (int d = 1; d <= DateTime.DaysInMonth(DateTime.Now.Year + StartYearDropdown.value, StartMonthDropdown.value + 1); d++)
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

    public void OnGenerateClick()
    {
        if (pickedItem == null)
        {
            Debug.LogError("No Item picked, you must pick an underground item to know its best chains");
            return;
        }


        DateTime startDate = new DateTime(DateTime.Now.Year + StartYearDropdown.value, StartMonthDropdown.value + 1, StartDayDropdown.value + 1);
        DateTime endDate = new DateTime(DateTime.Now.Year + EndYearDropdown.value, EndMonthDropdown.value + 1, EndDayDropdown.value + 1, 1, 0, 0);

        if (endDate < startDate)
        {
            Debug.LogError("End date is anterior to startDat, no generation");
            return;
        }

        StartCoroutine(coGenerateClick(startDate, endDate));
    }

    IEnumerator coGenerateClick(DateTime startDate, DateTime endDate)
    {
        UIManager.Instance.LoadingActivation(true);
        
        yield return new WaitForEndOfFrame();

        Debug.Log("Generating chain from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString());

        SortedDictionary<DateTime, List<DailyDeal>> allDeals = new SortedDictionary<DateTime, List<DailyDeal>>();
        List<KeyValuePair<DateTime, DailyDeal>> dealsAsList = new List<KeyValuePair<DateTime, DailyDeal>>();

        int safetyCount = 0;
        for (DateTime day = startDate; day < endDate; day = day.AddDays(1))
        {
            List<DailyDeal> dailyDeals = DailyDeal.GenerateDeals(PlayerSettings.MaxDeals, day);

            dailyDeals.Sort(delegate (DailyDeal a, DailyDeal b)
            {
                if (a.item1 == b.item2) { return 1; }
                if (b.item1 == a.item2) { return -1; }

                // if `a` can be linked from something sort `a` after `b`
                if (dailyDeals.Exists(x => a.item1 == x.item2)) { return 1; }
                // if `b` can be linked from something, sort `a` before `b`
                if (dailyDeals.Exists(x => b.item1 == x.item2)) { return -1; }

                return 0;
            });

            dealsAsList.AddRange(dailyDeals.Select(x => new KeyValuePair<DateTime, DailyDeal>(day, x)));

            allDeals.Add(day, dailyDeals);
                        
            safetyCount++;
            if (safetyCount > 30)   // arbitrary value to avoid all instantiations in same frame
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
