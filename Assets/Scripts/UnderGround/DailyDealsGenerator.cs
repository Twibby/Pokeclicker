using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DailyDealsGenerator : MonoBehaviour
{
    public GameObject DayBlocPrefab;    

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public virtual void Initialize() { Debug.Log("Init generic"); }

    //public void DisplayDeals(DateTime date, List<DailyDeal> deals, Transform parent)
    //{
    //    SortedDictionary<DateTime, List<DailyDeal>> tmpDict = new SortedDictionary<DateTime, List<DailyDeal>>();
    //    tmpDict.Add(date, new List<DailyDeal>(deals));
    //    DisplayDeals(tmpDict, parent);
    //}

    //public void DisplayDeals(SortedDictionary<DateTime, List<DailyDeal>> deals, Transform parent)
    //{
    //    foreach (DayBloc day in parent.GetComponentsInChildren<DayBloc>(true)) { Destroy(day.gameObject); }

    //    foreach (var day in deals)
    //    {
    //        GameObject dayBloc = GameObject.Instantiate(DayBlocPrefab, parent);
    //        dayBloc.name = "Deals_Of_" + day.Key.ToString("yyyy-MM-dd");
    //        dayBloc.GetComponent<DayBloc>().Init(day.Key, day.Value);
    //    }
    //}


}
