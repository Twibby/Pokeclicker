using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;

public class ChainDealGenerator : DailyDealsGenerator
{
    public int MaxNumberOfChains = 10;

    [Header("Dropdowns")]
    public TMPro.TMP_Dropdown StartYearDropdown;
    public TMPro.TMP_Dropdown StartMonthDropdown, StartDayDropdown;
    public TMPro.TMP_Dropdown EndYearDropdown, EndMonthDropdown, EndDayDropdown;

    [Space(8)]
    public Transform DealsParent;

    //public PickPanel PickPanel;       Old way with single item
    public FilterPanel MyFilterPanel;
    public TMPro.TMP_Text PickedItemLabel;
    public Toggle SubchainToggle;

    public GameObject NoChainsLabel;

    public Toggle ExportToggle;
    public RectTransform ExportDetailsPanel;

    [SerializeField] private List<UndergroundItem> pickedItems;
    
    private List<DealChain> _chains = new List<DealChain>();

    public override void Initialize()
    {
        Debug.Log("Initializing Deal Chain Panel");

        int year = DateTime.Now.Year;
        {
            StartYearDropdown.ClearOptions();
            for (int y = 0; y < 4; y++)
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

        StartDayDropdown.RefreshShownValue();
        StartMonthDropdown.RefreshShownValue();
        StartYearDropdown.RefreshShownValue();
        EndDayDropdown.RefreshShownValue();
        EndMonthDropdown.RefreshShownValue();
        EndYearDropdown.RefreshShownValue();

        foreach (ChainBloc day in DealsParent.GetComponentsInChildren<ChainBloc>(true))
        {
            Destroy(day.gameObject);
        }

        SubchainToggle.isOn = true;
        ExportToggle.interactable = false;

        //pickedItem = null;
        //OnPickItem(-1);
        ApplyFilters(new List<UndergroundItem>());
        NoChainsLabel.SetActive(false);

        _chains = new List<DealChain>();        
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.Quote))
        {
            Debug.Log("Looking for best chain for each item");
            DateTime startDate = new DateTime(DateTime.Now.Year + StartYearDropdown.value, StartMonthDropdown.value + 1, StartDayDropdown.value + 1);
            DateTime endDate = new DateTime(DateTime.Now.Year + EndYearDropdown.value, EndMonthDropdown.value + 1, EndDayDropdown.value + 1, 1, 0, 0);

            var chains = DealChain.GetBestDealChainsForItem(pickedItems, startDate, endDate);
            UnityWebGLIOManager.ExportType exportType = UnityWebGLIOManager.ExportType.JSON;
            if (ExportDetailsPanel.GetComponentInChildren<TMPro.TMP_Dropdown>() != null)
                exportType = (UnityWebGLIOManager.ExportType)ExportDetailsPanel.GetComponentInChildren<TMPro.TMP_Dropdown>().value;
            StartCoroutine(UnityWebGLIOManager.Instance.ExportBestChains(chains, "bestDeals", exportType));
        }
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
        StartDayDropdown.RefreshShownValue();
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
        EndDayDropdown.RefreshShownValue();
    }

    public void OpenFilterPanel()
    {
        MyFilterPanel.gameObject.SetActive(true);
        this.MyFilterPanel.Set(pickedItems);
    }

    public override void OnPickItem(int itemId)
    {
        UndergroundItem item = UndergroundItemsManager.Singleton.Items.Find(x => x.Id == itemId);
        pickedItems = new List<UndergroundItem>() { item };

        PickedItemLabel.text = "Current item: <i>" + (pickedItems != null && pickedItems.Count > 0 ? pickedItems[0].DisplayName : "None") + "</i>";
        
        MyFilterPanel.gameObject.SetActive(false);
    }

    public override void ApplyFilters(List<UndergroundItem> items)
    {
        pickedItems = new List<UndergroundItem>(items);
        MyFilterPanel.gameObject.SetActive(false);

        string log = "Current item: <i>";
        if (pickedItems == null || pickedItems.Count == 0)
        {
            log += "None";
        }
        else
        {
            int maxItemDisplayed = 2;
            for (int i = 0; i < Mathf.Min(pickedItems.Count, maxItemDisplayed); i++)
            {
                log += pickedItems[i].DisplayName + ", ";
            }
            if (pickedItems.Count > maxItemDisplayed)
                log += "... (total: " + pickedItems.Count + ")";
            else
                log = log[0..^2];            
        }
        log += "</i>";
        PickedItemLabel.text = log;
    }

    public void OnExportToggleClick(bool isOn)
    {
        //float duration = 0.4f;
        DOTween.Kill(ExportDetailsPanel.gameObject.name + "_pivot");
        ExportDetailsPanel.DOPivotX(isOn ? 0 : 1f, 0.4f).SetEase(Ease.InOutCubic).SetId(ExportDetailsPanel.gameObject.name + "_pivot");

        ExportToggle.GetComponentInChildren<RotateOnToggle>()?.OnToggle(isOn);

        foreach (ChainBloc bloc in DealsParent.GetComponentsInChildren<ChainBloc>(true))
        {
            bloc.ExportToggleActivation(isOn);
        }
    }

    public void OnExportClick()
    {
        Debug.Log("Start Export");
        List<DealChain> chainsToExport = new List<DealChain>();
        foreach (ChainBloc bloc in DealsParent.GetComponentsInChildren<ChainBloc>(false))
        {
            if (bloc.ExportToggle.isOn)
                chainsToExport.Add(bloc.MyDealChain);
        }
        string filenameSuffix = DateTime.Now.ToString("yyyyMMdd");
        if (ExportDetailsPanel.GetComponentInChildren<TMPro.TMP_InputField>() != null
            && ExportDetailsPanel.GetComponentInChildren<TMPro.TMP_InputField>().text != string.Empty)
            filenameSuffix = ExportDetailsPanel.GetComponentInChildren<TMPro.TMP_InputField>().text;

        UnityWebGLIOManager.ExportType exportType = UnityWebGLIOManager.ExportType.JSON;
        if (ExportDetailsPanel.GetComponentInChildren<TMPro.TMP_Dropdown>() != null) 
            exportType = (UnityWebGLIOManager.ExportType)ExportDetailsPanel.GetComponentInChildren<TMPro.TMP_Dropdown>().value;

        StartCoroutine(UnityWebGLIOManager.Instance.ExportChains(chainsToExport, filenameSuffix, exportType));
    }

    public void OnSubchainToggleClick(bool isOn)
    {
        foreach (ChainBloc bloc in DealsParent.GetComponentsInChildren<ChainBloc>(true))
        {
            bloc.gameObject.SetActive(isOn || bloc.MyDealChain.IsMainChain);
        }
    }

    public void OnGenerateClick()
    {
        if (pickedItems == null || pickedItems.Count == 0)
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

        //await Task.Delay(5);
        yield return new WaitForEndOfFrame();

        Debug.Log("Generating chain from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString());

        foreach (ChainBloc bloc in DealsParent.GetComponentsInChildren<ChainBloc>(true)) { Destroy(bloc.gameObject); }

        yield return new WaitForEndOfFrame();
       
        //_chains = await DealChain.GetDealChains(pickedItems, startDate, endDate);
        _chains = DealChain.GetDealChains(pickedItems, startDate, endDate);

        int safetyCount = 0;
        for (int i = 0; i < Mathf.Min(MaxNumberOfChains, _chains.Count); i++)
        {
            GameObject dayBloc = GameObject.Instantiate(DayBlocPrefab, DealsParent);
            dayBloc.name = "Chain_" + i.ToString();
            dayBloc.GetComponent<ChainBloc>().Init(_chains[i]);

            safetyCount++;
            if (safetyCount > 5)   // arbitrary value to avoid all instantiations in same frame
            {
                safetyCount = 0;
                yield return new WaitForEndOfFrame();
                
            }
            // await Task.Delay(5);
        }
        NoChainsLabel.SetActive(_chains.Count == 0);

        OnSubchainToggleClick(SubchainToggle.isOn);
        ExportToggle.interactable = _chains.Count > 0;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        UIManager.Instance.LoadingActivation(false);
    }
}
