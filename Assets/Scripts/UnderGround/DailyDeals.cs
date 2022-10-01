using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class DailyDeal
{
    public UndergroundItem item1;
    public UndergroundItem item2;
    public int amount1;
    public int amount2;
    
    public DateTime date;

    public DailyDeal()
    {
        this.item1 = getRandomItem();
        this.amount1 = getRandomAmount();
        this.item2 = getRandomItem();
        this.amount2 = getRandomAmount();
    }

    public DailyDeal(DateTime p_date) : base()
    {
        this.date = p_date;
    }

    private static UndergroundItem getRandomItem()
    {
        return SeededRand.FromList<UndergroundItem>(UndergroundItemsManager.Singleton.Items);
    }

    private static int getRandomAmount()
    {
        return SeededRand.intBetween(1, 3);
    }

    #region DailyDeal generation
    public static Task<List<DailyDeal>> GenerateDeals(int p_maxDeals, DateTime p_date)
    {
        SeededRand.seedWithDate(p_date);

        List<DailyDeal> deals = new List<DailyDeal>();

        int maxTries = p_maxDeals * 10;
        int safetyCount = 0;
        while (safetyCount < maxTries && deals.Count < p_maxDeals)
        {
            DailyDeal deal = new DailyDeal();
            deal.date = p_date;
            if (deal.IsValid(deals))
            {
                deals.Add(deal);
            }
            safetyCount++;
        }

        return Task.FromResult(deals);

        //return deals; new Task<List<DailyDeal>>( () => { return deals; });
    }

    public bool IsValid(List<DailyDeal> dealList)
    {
        string item1Name = this.item1.DisplayName;
        string item2Name = this.item2.DisplayName;

        if (item1Name == item2Name)
            return false;

        // Left side item cannot be Evolution Item or Shard
        if (
            this.item1.valueType == UndergroundItem.ValueType.EvolutionItem
            || this.item1.valueType == UndergroundItem.ValueType.Shard
        )
        {
            return false;
        }

        if (DailyDeal.sameDealExists(item1Name, item2Name, dealList))
        {
            return false;
        }

        if (DailyDeal.reverseDealExists(item1Name, item2Name, dealList))
        {
            return false;
        }

        return true;
    }

    private static bool sameDealExists(string name1, string name2, List<DailyDeal> dealList)
    {
        return dealList.Exists(x => x.item1.DisplayName == name1 && x.item2.DisplayName == name2);
    }

    private static bool reverseDealExists(string name1, string name2, List<DailyDeal> dealList)
    {
        return dealList.Exists(x => x.item2.DisplayName == name1 && x.item1.DisplayName == name2);
    }
    #endregion

    public override string ToString()
    {
        return this.amount1 + " x " + this.item1.DisplayName + "\t => \t" + this.amount2 + " x " + this.item2.DisplayName;
    }
}
