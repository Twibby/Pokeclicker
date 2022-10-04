using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class UnityWebGLIOManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void DownloadFileCustom(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);


    private bool _isBusy = false;
    public IEnumerator ExportChains(List<DealChain> chains, string filenameSuffix, bool isJSON = true)
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
        byte[] byteArray;

        if (isJSON)
        {
            JWT.DefaultJsonSerializer parser = new JWT.DefaultJsonSerializer();
            string jsonContent = parser.Serialize(deals);

            Debug.LogWarning("* " + jsonContent);
            byteArray = System.Text.Encoding.UTF8.GetBytes(jsonContent);
        }
        else
        {
            string csvContent = "Date;Amount1;Item1;Amount2;Item2" + System.Environment.NewLine;
            foreach (DailyDeal deal in deals)
            {
                csvContent += deal.ToCsvString() + System.Environment.NewLine;
            }
            byteArray = System.Text.Encoding.UTF8.GetBytes(csvContent);
        }


#if UNITY_EDITOR
        string filePath = Application.dataPath + "/chainsExport_" + filenameSuffix + "." + (isJSON ? "json" : "csv");

        Debug.Log("Exporting for Editor, path is : " + filePath);

        System.IO.File.WriteAllBytes(filePath, byteArray);
#elif UNITY_WEBGL

        _isBusy = true;
        DownloadFileCustom("UnityWebGLIOManager", "OnFileDownload", "chainsExport_" + filenameSuffix + "." + (isJSON ? "json" : "csv"), byteArray, byteArray.Length);

        float t0 = Time.time;
        while (_isBusy && (Time.time - t0 < 10f))
        {
            yield return new WaitForEndOfFrame();
        }
#endif
        yield return new WaitForEndOfFrame();
        UIManager.Instance.LoadingActivation(false);

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
        Debug.LogWarning("Received callback : " + address);
    }
}
