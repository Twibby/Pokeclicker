using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;

public class DealChain
{
    public List<DailyDeal> Deals;

    public DealChain() { this.Deals = new List<DailyDeal>(); }
    public DealChain(DealChain copy, DailyDeal adder = null)
    {
        this.Deals = new List<DailyDeal>();
        if (adder != null)
            Deals.Add(adder);
        
        foreach(DailyDeal deal in copy.Deals) { Deals.Add(deal); }
    }

    public float Profit
    {
        get
        {
            float profit = 1f;
            foreach (DailyDeal deal in Deals) { profit = profit * (float)deal.amount2 / (float)deal.amount1;  }

            return profit;
        }
    }

    public override string ToString()
    {
        if (Deals.Count == 0) { return "DealChain is empty"; }
        
        string log = Deals[0].item1.DisplayName;
        Deals.ForEach(x => log += " (" + x.amount1 +") ==" + x.date.ToString("dd/MM") + "==> (" + x.amount2 + ") " + x.item2.DisplayName);
        return log;
    }

    public static List<DealChain> GetDealChains(List<UndergroundItem> requiredItems, DateTime startDate, DateTime endDate)
    {
        List<DealChain> dealChains = new List<DealChain>();

        if (requiredItems == null || requiredItems.Count == 0)
        {
            Debug.LogError("No item picked");
            return dealChains;
        }

        List<DailyDeal> allDealsAsList = new List<DailyDeal>();
        // First, generate all deals and put them into a single list, sorted (to have no rec loop)
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

            allDealsAsList.AddRange(dailyDeals);

            //await Task.Delay(2);
        }

        //await Task.Delay(10);
        Debug.Log("Deals generated, starting computing chains");

        // go through deals list and if deal is about required item add it to a chain
        while (allDealsAsList.Count > 0)
        {
            DailyDeal lastDeal = allDealsAsList.Last();
            //Debug.LogWarning("Dealing with  : " + lastDeal.ToString());

            List<DealChain> newChains = new List<DealChain>();
            foreach (DealChain chain in dealChains)
            {
                //Debug.Log(" Current chain is " + chain.ToString());
                if (chain.Deals[0].item1.Id == lastDeal.item2.Id)
                {
                    //Debug.Log("First item match, so duplicate chain and add this deal");
                    newChains.Add(new DealChain(chain, lastDeal));
                }
            }
            dealChains.AddRange(newChains);

            if (requiredItems.Exists(x => x.Id == lastDeal.item2.Id))
            {
                //Debug.Log("SelectedItem is found, create a new chain with it");
                DealChain dc = new DealChain();
                dc.Deals.Add(lastDeal);
                dealChains.Add(dc);
            }

            //Debug.LogWarning("After this deal, we have now these chains : ");
            //Debug.Log(String.Join(System.Environment.NewLine, dealChains));
            //Debug.Log(" ********************* ");

            allDealsAsList.RemoveAt(allDealsAsList.Count - 1);
            //Debug.Log("Remaining deals are : " + String.Join('|', allDealsAsList.Select(x => x.ToString())));
        }

        dealChains.Sort(delegate (DealChain d1, DealChain d2)
           {
               if (d1.Profit != d2.Profit)
                   return d2.Profit.CompareTo(d1.Profit);

               if (d1.Deals.Last().date != d2.Deals.Last().date)
                   return d1.Deals.Last().date.CompareTo(d2.Deals.Last().date);

               return d1.Deals.Count.CompareTo(d2.Deals.Count);
           });

        return dealChains;
    }

}
