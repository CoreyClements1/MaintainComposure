using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtsManager : MonoBehaviour
{

    // ArtsManager manages the arts panel


    #region ENUMS AND STATIC VARIABLES


    public enum ArtType
    {
        Fundamental,
        Combat,
        Collaborative,
        Reactionary,
        Recovery,
        Stealth,
        Utility,
    }

    public enum ArtComplexity
    {
        Basic,
        Advanced,
        Expert,
    }

    public static Color[] artTextColors = new Color[]
    {
        new Color(183f/255f, 183f/255f, 183f/255f), // Fundamental
        new Color(241f/255f, 135f/255f, 146f/255f), // Combat
        new Color(125f/255f, 204f/255f, 224f/255f), // Collaborative
        new Color(243f/255f, 190f/255f, 127f/255f), // Reactionary
        new Color(243f/255f, 127f/255f, 157f/255f), // Recovery
        new Color(112f/255f, 203f/255f, 116f/255f), // Stealth
        new Color(220f/255f, 227f/255f, 106f/255f), // Utility
    };

    public static Color[] artBgColors = new Color[]
    {
        new Color(34f/255f, 34f/255f, 34f/255f), // Fundamental
        new Color(49f/255f, 6f/255f, 11f/255f), // Combat
        new Color(9f/255f, 42f/255f, 56f/255f), // Collaborative
        new Color(49f/255f, 30f/255f, 17f/255f), // Reactionary
        new Color(51f/255f, 6f/255f, 26f/255f), // Recovery
        new Color(8f/255f, 29f/255f, 13f/255f), // Stealth
        new Color(39f/255f, 44f/255f, 7f/255f), // Utility
    };


    #endregion


    #region OTHER VARIABLES


    public static ArtsManager Instance { get; private set; }

    [SerializeField] private Transform contentParent;
    [SerializeField] private TMP_Dropdown typeDropdown;
    [SerializeField] private TMP_Dropdown sortDropdown;
    [SerializeField] private Toggle showPassiveToggle;
    [SerializeField] private Art artPrefab;

    public List<List<ArtData>> learnedArtsByType { get; private set; } = new List<List<ArtData>>();
    private List<ArtData> learnedArtsAlphabetical = new List<ArtData>();


    #endregion


    #region SETUP


    // Sets up singleton
    //----------------------------------------//
    public void SetupSingleton()
    //----------------------------------------//
    {
        if (ArtsManager.Instance == null)
        {
            ArtsManager.Instance = this;
        }
        else
        {
            if (ArtsManager.Instance != this)
            {
                Destroy(this);
            }
        }

    } // END SetupSingleton


    // Sets up arts from list of learned arts
    //----------------------------------------//
    public void SetupArts(ArtData[] learnedArts)
    //----------------------------------------//
    {
        int numTypes = Enum.GetNames(typeof(ArtType)).Length;

        // Clear old arts
        foreach (Transform child in contentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        learnedArtsByType.Clear();
        learnedArtsAlphabetical.Clear();

        // Create new List List
        for (int i = 0; i < numTypes; i++)
        {
            learnedArtsByType.Add(new List<ArtData>());
        }

        // Add all datas to respective type
        foreach (ArtData data in learnedArts)
        {
            learnedArtsByType[(int)data.artType].Add(data);
        }

        // Sort respective types
        SortTypeList();

        // Sort alphabetical list
        learnedArtsAlphabetical = learnedArts.ToList();
        SortAlphaList();

        // Display all
        DisplayAllByType();

    } // END SetupArts


    // Sets up blank arts
    //----------------------------------------//
    public void SetupArtsFromScratch()
    //----------------------------------------//
    {
        int numTypes = Enum.GetNames(typeof(ArtType)).Length;

        // Clear old arts
        foreach (Transform child in contentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        learnedArtsByType.Clear();
        learnedArtsAlphabetical.Clear();

        // Create new List List
        for (int i = 0; i < numTypes; i++)
        {
            learnedArtsByType.Add(new List<ArtData>());
        }

        // Set dropdowns
        typeDropdown.value = 0;
        sortDropdown.value = 0;

        // Display nothing
        DisplayAllByType();

    } // END SetupArtsFromScratch


    #endregion


    #region SORTING


    // Sorts each type of the learned arts array alphabetically
    //----------------------------------------//
    private void SortTypeList()
    //----------------------------------------//
    {
        for (int i = 0; i < learnedArtsByType.Count; i++)
        {
            learnedArtsByType[i] = learnedArtsByType[i].OrderBy(t => t.artName).ToList();
        }

    } // END SortTypeList


    // Sorts alphabetical list
    //----------------------------------------//
    private void SortAlphaList()
    //----------------------------------------//
    {
        learnedArtsAlphabetical = learnedArtsAlphabetical.OrderBy(t => t.artName).ToList();

    } // END SortAlphaList


    #endregion


    #region DISPLAY


    // Displays all arts by type, then alphabetically
    //----------------------------------------//
    private void DisplayAllByType()
    //----------------------------------------//
    {
        foreach (Transform child in contentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Spawn arts and display
        foreach (List<ArtData> artDataList in learnedArtsByType)
        {
            foreach (ArtData data in artDataList)
            {
                if (!showPassiveToggle.isOn)
                {
                    if (data.passive == false)
                    {
                        Art newArt = GameObject.Instantiate(artPrefab);
                        newArt.SetupArt(data);
                        newArt.transform.SetParent(contentParent);
                    }
                }
                else
                {
                    Art newArt = GameObject.Instantiate(artPrefab);
                    newArt.SetupArt(data);
                    newArt.transform.SetParent(contentParent);
                }
            }
        }

    } // END DisplayAllByType


    // Displays all arts of a certain type alphabetically
    //----------------------------------------//
    private void DisplayAllOfType(ArtType artType)
    //----------------------------------------//
    {
        foreach (Transform child in contentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (ArtData data in learnedArtsByType[(int)artType])
        {
            if (!showPassiveToggle.isOn)
            {
                if (data.passive == false)
                {
                    Art newArt = GameObject.Instantiate(artPrefab);
                    newArt.SetupArt(data);
                    newArt.transform.SetParent(contentParent);
                }
            }
            else
            {
                Art newArt = GameObject.Instantiate(artPrefab);
                newArt.SetupArt(data);
                newArt.transform.SetParent(contentParent);
            }
        }

    } // END DisplayAllOfType


    // Displays all arts in alphabetical order
    //----------------------------------------//
    private void DisplayAllAlphabetical()
    //----------------------------------------//
    {
        foreach (Transform child in contentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Spawn arts and display
        foreach (ArtData data in learnedArtsAlphabetical)
        {
            if (!showPassiveToggle.isOn)
            {
                if (data.passive == false)
                {
                    Art newArt = GameObject.Instantiate(artPrefab);
                    newArt.SetupArt(data);
                    newArt.transform.SetParent(contentParent);
                }
            }
            else
            {
                Art newArt = GameObject.Instantiate(artPrefab);
                newArt.SetupArt(data);
                newArt.transform.SetParent(contentParent);
            }
        }

    } // END DisplayAllAlphabetical


    #endregion


    #region DROPDOWN VAL CHANGED


    // OnValueChange, display accordingly
    //----------------------------------------//
    public void OnValueChange()
    //----------------------------------------//
    {
        switch(typeDropdown.value)
        {
            case 0:
                if (sortDropdown.value == 0)
                {
                    DisplayAllByType();
                }
                else
                {
                    DisplayAllAlphabetical();
                }
                break;
            case 1:
                // TODO
                break;
            default:
                DisplayAllOfType((ArtType)Enum.ToObject(typeof(ArtType), typeDropdown.value - 2));
                break;
        }

    } // END OnValueChange


    #endregion


    #region GET


    // Gets art datas as an array
    //----------------------------------------//
    public ArtData[] GetArtsAsArr()
    //----------------------------------------//
    {
        return learnedArtsAlphabetical.ToArray();

    } // END GetArtsAsArr


    #endregion


} // END ArtsManager.cs
