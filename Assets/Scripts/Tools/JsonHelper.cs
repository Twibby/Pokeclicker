using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class JsonHelper
{
    public static List<T> FromJson<T>(string json)
    {
        if (json.StartsWith("["))
        {
            json = "{\"Items\":" + json + "}";
        }
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(List<T> array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(List<T> array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    public static DateTime UnixTimeToDateTime(long unixtime)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixtime).UtcDateTime;
    }

    public static long DateTimeToUnix(DateTime MyDateTime)
    {
        return new DateTimeOffset(MyDateTime).ToUnixTimeSeconds();
    }

[System.Serializable]
    private class Wrapper<T>
    {
        public List<T> Items;
    }
}
