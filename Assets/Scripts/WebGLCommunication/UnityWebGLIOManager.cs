using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Linq;

public class UnityWebGLIOManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void DownloadFileCustom(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);


    public enum ExportType { JSON, CSV, ICAL }
    private bool _isBusy = false;


    public static string GetExportTypeExtension(ExportType type)
    {
        switch (type)
        {
            case ExportType.JSON: return "json";
            case ExportType.CSV: return "csv";
            case ExportType.ICAL: return "ics";
            default:
                return "txt";
        }
    }

    public IEnumerator ExportChains(List<DealChain> chains, string filenameSuffix, ExportType exportType = ExportType.JSON)
    {
        UIManager.Instance.LoadingActivation(true);
        yield return new WaitForEndOfFrame();

        List<DailyDeal> deals = new List<DailyDeal>();
        foreach (DealChain dc in chains)
        {
            foreach (DailyDeal dd in dc.Deals)
            {
                if (!deals.Contains(dd))
                    deals.Add(dd);
            }
        }
        deals.Sort(delegate (DailyDeal a, DailyDeal b) { return a.date.CompareTo(b.date); });

        Debug.LogWarning(deals.Count);

        yield return new WaitForEndOfFrame();
        byte[] byteArray = new byte[0];

        switch (exportType)
        {
            case ExportType.JSON:
                JWT.DefaultJsonSerializer parser = new JWT.DefaultJsonSerializer();
                string jsonContent = parser.Serialize(deals);

                Debug.LogWarning("* " + jsonContent);
                byteArray = Encoding.UTF8.GetBytes(jsonContent);
                break;

            case ExportType.CSV:
                string csvContent = "Date;Amount1;Item1;Amount2;Item2" + System.Environment.NewLine;
                foreach (DailyDeal deal in deals)
                {
                    csvContent += deal.ToCsvString() + System.Environment.NewLine;
                }
                byteArray = Encoding.UTF8.GetBytes(csvContent);
                break;

            case ExportType.ICAL:
                byteArray = Encoding.UTF8.GetBytes(getIcalString(deals));
                break;
        }


#if UNITY_EDITOR
        string filePath = Application.dataPath + "/chainsExport_" + filenameSuffix + "." + GetExportTypeExtension(exportType);

        Debug.Log("Exporting for Editor, path is : " + filePath);

        System.IO.File.WriteAllBytes(filePath, byteArray);
#elif UNITY_WEBGL

        _isBusy = true;
        DownloadFileCustom("UnityWebGLIOManager", "OnFileDownload", "chainsExport_" + filenameSuffix + "." + GetExportTypeExtension(exportType), byteArray, byteArray.Length);

        float t0 = Time.time;
        while (_isBusy && (Time.time - t0 < 10f))
        {
            yield return new WaitForEndOfFrame();
        }
#endif
        yield return new WaitForEndOfFrame();
        UIManager.Instance.LoadingActivation(false);

    }

    public IEnumerator ExportBestChains(Dictionary<UndergroundItem, DealChain> chains, string filenameSuffix, ExportType exportType)
    {
        UIManager.Instance.LoadingActivation(true);
        yield return new WaitForEndOfFrame();

        List<DailyDealExtended> deals = new List<DailyDealExtended>();
        foreach (KeyValuePair<UndergroundItem, DealChain> dc in chains)
        {
            foreach (DailyDeal dd in dc.Value.Deals)
            {
                if (!deals.Exists(x => x.deal == dd))
                    deals.Add(new DailyDealExtended(dd, new List<string>()));

                if (!deals.Find(x => x.deal == dd).usedForItems.Contains(dc.Key.DisplayName))
                    deals.Find(x => x.deal == dd).usedForItems.Add(dc.Key.DisplayName);
            }
        }
        deals.Sort(delegate (DailyDealExtended a, DailyDealExtended b) { return a.deal.date.CompareTo(b.deal.date); });

        yield return new WaitForEndOfFrame();
        byte[] byteArray = new byte[0];

        switch (exportType)
        {
            case ExportType.JSON:
                JWT.DefaultJsonSerializer parser = new JWT.DefaultJsonSerializer();
                string jsonContent = parser.Serialize(deals);

                Debug.LogWarning("* " + jsonContent);
                byteArray = Encoding.UTF8.GetBytes(jsonContent);
                break;

            case ExportType.CSV:
                string csvContent = "Date;Amount1;Item1;Amount2;Item2;;Used For;" + System.Environment.NewLine;
                foreach (DailyDealExtended deal in deals)
                {
                    deal.usedForItems.Sort();
                    csvContent += deal.deal.ToCsvString() + ";;" + String.Join("/", deal.usedForItems) + System.Environment.NewLine;
                }
                byteArray = Encoding.UTF8.GetBytes(csvContent);
                break;

            case ExportType.ICAL:
                byteArray = Encoding.UTF8.GetBytes(getIcalString(deals));
                break;
        }


#if UNITY_EDITOR
        string filePath = Application.dataPath + "/chainsExport_" + filenameSuffix + "." + GetExportTypeExtension(exportType);

        Debug.Log("Exporting for Editor, path is : " + filePath);

        System.IO.File.WriteAllBytes(filePath, byteArray);
#elif UNITY_WEBGL

        _isBusy = true;
        DownloadFileCustom("UnityWebGLIOManager", "OnFileDownload", "chainsExport_" + filenameSuffix + "." + GetExportTypeExtension(exportType), byteArray, byteArray.Length);

        float t0 = Time.time;
        while (_isBusy && (Time.time - t0 < 10f))
        {
            yield return new WaitForEndOfFrame();
        }
#endif
        yield return new WaitForEndOfFrame();
        UIManager.Instance.LoadingActivation(false);

    }

    struct DailyDealExtended
    {
        public DailyDeal deal;
        public List<string> usedForItems;

        public DailyDealExtended(DailyDeal m_deal, List<string> m_items)
        {
            this.deal = m_deal;
            this.usedForItems = new List<string>(m_items);
        }
    }

    private static GameObject _instance = null;

    public static UnityWebGLIOManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("UnityWebGLIOManager");
                _instance.AddComponent<UnityWebGLIOManager>();
                DontDestroyOnLoad(_instance);
            }

            return _instance.GetComponent<UnityWebGLIOManager>();
        }
    }

    private UnityWebGLIOManager() { }

    public static void Kill()
    {
        _instance = null;
    }

    public void OnFileDownload(string address)
    {
        _isBusy = false;
        Debug.LogWarning("Received callback : " + address);
        UIManager.Instance.SetMessage("Content successfully exported and saved to file " + address, UIManager.MessageLevel.VALIDATE);
    }


    string getIcalString(List<DailyDeal> deals)
    {
        return getIcalString(deals.Select(x => new DailyDealExtended(x, new List<string>())).ToList());
    }

    string getIcalString(List<DailyDealExtended> deals)
    { 
        //create a new stringbuilder instance
        StringBuilder sb = new StringBuilder();

        //start the calendar item
        sb.AppendLine("BEGIN:VCALENDAR");
        sb.AppendLine("VERSION:2.0");
        sb.AppendLine("PRODID:google.com");
        sb.AppendLine("CALSCALE:GREGORIAN");
        sb.AppendLine("METHOD:PUBLISH");

        double index = SeededRand.next();
        foreach (DailyDealExtended deal in deals)
        {
            sb.AppendLine("BEGIN:VEVENT");

            DateTime startDate = deal.deal.date.AddHours(12);
            DateTime endDate = deal.deal.date.AddHours(12).AddMinutes(1);
            sb.AppendLine("DTSTART:" + startDate.ToString("yyyyMMddTHHmm00"));
            sb.AppendLine("DTEND:" + endDate.ToString("yyyyMMddTHHmm00"));
            sb.AppendLine("DTSTAMP:" + DateTime.Now.ToString("yyyyMMddTHHmm00"));
            
            sb.AppendLine("UID:" + DateTime.Now.ToString("yyyyMMddTHHmm00") + SeededRand.next().ToString() + "_" + index.ToString() + "@pokedeals.com");
            index++;

            sb.AppendLine("SUMMARY: PokeDeal : " + deal.deal.ToString());

            string description = "DESCRIPTION: You need to make the following underground deal today : " + deal.deal.ToString();
            if (deal.usedForItems.Count > 0)
            {
                description += System.Environment.NewLine + "This will be used in chains for " + String.Join(',', deal.usedForItems);
            }
            sb.AppendLine(description);
            sb.AppendLine("STATUS:CONFIRMED");
            sb.AppendLine("TRANSP:TRANSPARENT");
            sb.AppendLine("SEQUENCE:0");


            sb.AppendLine("END:VEVENT");

        }

        //end calendar item
        sb.AppendLine("END:VCALENDAR");

        return sb.ToString();
    }
}
