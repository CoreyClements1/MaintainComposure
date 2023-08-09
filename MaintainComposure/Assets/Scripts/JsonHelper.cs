using System;
using UnityEngine;

public static class JsonHelper
{

    // JsonHelper helps convert arrays of stuff to jsons
    // From https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity


    #region JSON


    // FromJson
    //--------------------------------------//
    public static T FromJson<T>(string json)
    //--------------------------------------//
    {
        T item = JsonUtility.FromJson<T>(json);
        return item;

    } // END FromJson


    // ToJson
    //--------------------------------------//
    public static string ToJson<T>(T item, bool prettyText)
    //--------------------------------------//
    {
        string json = JsonUtility.ToJson(item, prettyText);
        return json;

    } // END ToJson


    #endregion


    #region JSON ARRAY


    // FromJson
    //--------------------------------------//
    public static T[] FromJsonArray<T>(string json)
    //--------------------------------------//
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;

    } // END FromJson


    // ToJson overload
    //--------------------------------------//
    public static string ToJsonArray<T>(T[] array)
    //--------------------------------------//
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);

    } // END ToJson overload


    // ToJson
    //--------------------------------------//
    public static string ToJsonArray<T>(T[] array, bool prettyPrint)
    //--------------------------------------//
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);

    } // END ToJson


    // Wrapper class
    //--------------------------------------//
    [Serializable]
    private class Wrapper<T>
    //--------------------------------------//
    {
        public T[] Items;

    } // END Wrapper.cs


    #endregion


} // END JsonHelper.cs