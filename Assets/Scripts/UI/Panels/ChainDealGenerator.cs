using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class ChainDealGenerator : DailyDealsGenerator
{
    public int MaxNumberOfChains = 5;

    [Header("Dropdowns")]
    public TMPro.TMP_Dropdown StartYearDropdown;
    public TMPro.TMP_Dropdown StartMonthDropdown, StartDayDropdown;
    public TMPro.TMP_Dropdown EndYearDropdown, EndMonthDropdown, EndDayDropdown;

    [Space(8)]
    public Transform DealsParent;

    public PickPanel PickPanel;
    public TMPro.TMP_Text PickedItemLabel;

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


        foreach (ChainBloc day in DealsParent.GetComponentsInChildren<ChainBloc>(true))
        {
            Destroy(day.gameObject);
        }

        //pickedItem = null;
        OnPickItem(-1);  
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

    public override void OnPickItem(int itemId)
    {
        pickedItem = UndergroundItemsManager.Singleton.Items.Find(x => x.Id == itemId);

        PickedItemLabel.text = "Current item: <i>" + (pickedItem != null ? pickedItem.DisplayName : "None") + "</i>";
        
        PickPanel.gameObject.SetActive(false);
    }

    public void OnGenerateClick()
    {
        if (pickedItem == null)
        {
            UIManager.Instance.SetMessage("No Item picked, you must pick an underground item to know its best chains", UIManager.MessageLevel.ERROR);
            return;
        }


        DateTime startDate = new DateTime(DateTime.Now.Year + StartYearDropdown.value, StartMonthDropdown.value + 1, StartDayDropdown.value + 1);
        DateTime endDate = new DateTime(DateTime.Now.Year + EndYearDropdown.value, EndMonthDropdown.value + 1, EndDayDropdown.value + 1, 1, 0, 0);

        if (endDate < startDate)
        {
            UIManager.Instance.SetMessage("End date is anterior to startDate, no generation", UIManager.MessageLevel.ERROR);
            return;
        }

        StartCoroutine(coGenerateClick(startDate, endDate));
    }

    IEnumerator coGenerateClick(DateTime startDate, DateTime endDate)
    {
        UIManager.Instance.LoadingActivation(true);

        yield return new WaitForEndOfFrame();
        Debug.Log("Generating chain from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString());

        foreach (ChainBloc bloc in DealsParent.GetComponentsInChildren<ChainBloc>(true)) { Destroy(bloc.gameObject); }

        List<DealChain> chains = DealChain.GetDealChains(pickedItem, startDate, endDate);

        int safetyCount = 0;
        for (int i = 0; i < Mathf.Min(MaxNumberOfChains, chains.Count); i++)
        {
            GameObject dayBloc = GameObject.Instantiate(DayBlocPrefab, DealsParent);
            dayBloc.name = "Chain_" + i.ToString();
            dayBloc.GetComponent<ChainBloc>().Init(chains[i]);

            safetyCount++;
            if (safetyCount > 5)   // arbitrary value to avoid all instantiations in same frame
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
