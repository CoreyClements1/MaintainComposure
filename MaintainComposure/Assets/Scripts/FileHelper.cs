using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class FileHelper
{

    // FileExportHelper exports txt files to the data folder


    #region EXPORTING


    // Writes text to a file at a given path; overwrites by default
    //--------------------------------------//
    public static void ExportToTxt(string fileName, string text)
    //--------------------------------------//
    {
        string path = "Assets/Data/" + fileName + ".txt";

        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(text);
        writer.Close();

        // Re-load to get asset reference
        //AssetDatabase.ImportAsset(path);
        //TextAsset asset = Resources.Load(fileName) as TextAsset;

    } // END ExportToTxt


    #endregion


    #region JSON SAVING


    // Writes a given object to a JSON, then saves to file
    //--------------------------------------//
    public static void SaveAsJson<T>(string fileName, T dataToSave, bool prettyText)
    //--------------------------------------//
    {
        string json = JsonHelper.ToJson<T>(dataToSave, prettyText);
        ExportToTxt(fileName, json);

    } // END SaveAsJson


    // Write a given object to a JSON array, then saves to file
    //--------------------------------------//
    public static void SaveAsJsonArray<T>(string fileName, T[] dataToSave, bool prettyText)
    //--------------------------------------//
    {
        string json = JsonHelper.ToJsonArray<T>(dataToSave, prettyText);
        ExportToTxt(fileName, json);

    } // END SaveAsJsonArray


    #endregion


    #region ARRAY CONVERSIONS (2D / 1D)


    // Makes a 2D array from a 1D array
    //--------------------------------------//
    public static T[,] To2DArray<T>(T[] input, int height, int width)
    //--------------------------------------//
    {
        T[,] output = new T[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                output[i, j] = input[i * width + j];
            }
        }

        return output;

    } // END To2DArray


    // Makes a 1D array from a 2D array
    //--------------------------------------//
    public static T[] To1DArray<T>(T[,] input)
    //--------------------------------------//
    {
        // Step 1: get total size of 2D array, and allocate 1D array.
        int size = input.Length;
        T[] result = new T[size];

        // Step 2: copy 2D array elements into a 1D array.
        int write = 0;
        for (int i = 0; i <= input.GetUpperBound(0); i++)
        {
            for (int z = 0; z <= input.GetUpperBound(1); z++)
            {
                result[write++] = input[i, z];
            }
        }
        // Step 3: return the new array.
        return result;

    } // END To1DArray


    #endregion


    } // END FileExportHelper
