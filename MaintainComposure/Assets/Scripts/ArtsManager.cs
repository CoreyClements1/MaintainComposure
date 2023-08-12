using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;
using static ArtsManager;

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

    public List<ArtData> artRegistryAlphabetical { get; private set; } = new List<ArtData>();
    public List<List<ArtData>> artRegistryByType { get; private set; } = new List<List<ArtData>>();
    [SerializeField] private TextAsset artRegistryTextAsset;
    [SerializeField] private TextAsset updateRegistryTextAsset;

    [SerializeField] private ArtEditorPanel artEditorPanel;
    [SerializeField] private Transform registryDisplayContentParent;
    [SerializeField] private RegistryArt registryArtPrefab;
    [SerializeField] private TMP_Dropdown registryTypeDropdown;
    [SerializeField] private TMP_Dropdown registrySortDropdown;
    [SerializeField] private TMP_Dropdown registryComplexityDropdown;

    [SerializeField] private TMP_Dropdown learnFilterDropdown;
    [SerializeField] private TMP_Dropdown learnSortDropdown;
    [SerializeField] private TMP_Dropdown learnComplexityDropdown;
    [SerializeField] private Toggle ignoreSpecToggle;
    [SerializeField] private Transform learnableArtsContentParent;
    [SerializeField] private Art learnableArtPrefab;


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

        LoadArtRegistry();

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


    #region REGISTRY LOAD, SAVE, CONSOLIDATE


    // Loads arts from registry
    //----------------------------------------//
    public void LoadArtRegistry()
    //----------------------------------------//
    {
        artRegistryAlphabetical.Clear();
        artRegistryByType.Clear();

        // Load saved characters into characterDatas
        artRegistryAlphabetical = JsonHelper.FromJsonArray<ArtData>(artRegistryTextAsset.text).ToList<ArtData>();

        if (artRegistryAlphabetical != null)
        {
            artRegistryAlphabetical = artRegistryAlphabetical.OrderBy(t => t.artName).ToList();

            int numTypes = Enum.GetNames(typeof(ArtType)).Length;

            // Create new List List
            for (int i = 0; i < numTypes; i++)
            {
                artRegistryByType.Add(new List<ArtData>());
            }

            // Add all datas to respective type
            foreach (ArtData data in artRegistryAlphabetical)
            {
                artRegistryByType[(int)data.artType].Add(data);
            }

            for (int i = 0; i < artRegistryByType.Count; i++)
            {
                artRegistryByType[i] = artRegistryByType[i].OrderBy(t => t.artName).ToList();
            }
        }

    } // END LoadArtRegistry


    // Saves art registry
    //----------------------------------------//
    public void SaveArtRegistry()
    //----------------------------------------//
    {
        // Save as JSON
        FileHelper.SaveAsJsonArray<ArtData>("ArtRegistry", artRegistryAlphabetical.ToArray(), false);

    } // END SaveArtRegistry


    // Consolidates arts from update txt into registry
    //----------------------------------------//
    public void ConsolidateUpdateRegistry()
    //----------------------------------------//
    {
        ArtData[] updateRegistryDatas = JsonHelper.FromJsonArray<ArtData>(updateRegistryTextAsset.text);

        foreach (ArtData data in updateRegistryDatas)
        {
            bool inRegistry = false;
            for (int i = 0; i < artRegistryAlphabetical.Count; i++)
            {
                if (data.artName == artRegistryAlphabetical[i].artName)
                {
                    artRegistryAlphabetical[i] = data;
                    inRegistry = true;
                    break;
                }
            }

            if (!inRegistry)
            {
                artRegistryAlphabetical.Add(data);
            }
        }

        SaveArtRegistry();
        LoadArtRegistry();

    } // END ConsolidateUpdateRegistry


    #endregion


    #region REGISTRY NEW, EDIT, DELETE


    // Opens menu to create a new art
    //----------------------------------------//
    public void BeginCreateNewArt()
    //----------------------------------------//
    {
        artEditorPanel.gameObject.SetActive(true);
        artEditorPanel.ShowPanelForNewArt();

    } // END BeginCreateNewArt


    // Ends creating new art, adding to registry
    //----------------------------------------//
    public void EndCreateNewArt(ArtData newArt)
    //----------------------------------------//
    {
        artRegistryAlphabetical.Add(newArt);
        SaveArtRegistry();
        LoadArtRegistry();
        OnRegistryValueChange();

    } // END EndCreateNewArt


    // Opens menu to edit an existing art
    //----------------------------------------//
    public void BeginEditingRegistryArt(ArtData artToEdit)
    //----------------------------------------//
    {
        for (int i = 0; i < artRegistryAlphabetical.Count; i++)
        {
            if (artRegistryAlphabetical[i].artName == artToEdit.artName)
            {
                artEditorPanel.gameObject.SetActive(true);
                artEditorPanel.ShowPanel(artRegistryAlphabetical[i]);
                return;
            }
        }

        Debug.LogError("Oops died when trying to edit art");

    } // END UpdateRegistry


    // Ends editing registry art, updating registry with new edits
    //----------------------------------------//
    public void EndEditingRegistryArt(string nameToReplace, ArtData replacingData)
    //----------------------------------------//
    {
        for (int i = 0; i < artRegistryAlphabetical.Count; i++)
        {
            if (artRegistryAlphabetical[i].artName == nameToReplace)
            {
                artRegistryAlphabetical[i] = replacingData;
                SaveArtRegistry();
                LoadArtRegistry();
                OnRegistryValueChange();
                return;
            }
        }

        Debug.LogError("Oops died when trying to replace art");

    } // END EndEditingRegistryArt


    // Deletes art from registry, updating registry
    //----------------------------------------//
    public void DeleteArtFromRegistry(ArtData dataToDelete)
    //----------------------------------------//
    {
        for (int i = 0; i < artRegistryAlphabetical.Count; i++)
        {
            if (artRegistryAlphabetical[i].artName == dataToDelete.artName)
            {
                artRegistryAlphabetical.RemoveAt(i);
                SaveArtRegistry();
                LoadArtRegistry();
                OnRegistryValueChange();
                return;
            }
        }

        Debug.LogError("Oops died when trying to delete art");

    } // END DeleteArtFromRegistry


    #endregion


    #region REGISTRY DISPLAY


    // Called on panel initial show
    //----------------------------------------//
    public void RegistryInitialDisplay()
    //----------------------------------------//
    {
        registryTypeDropdown.value = 0;
        registrySortDropdown.value = 0;

        RegistryDisplayAllByType();

    } // END RegistryInitialDisplay


    // Displays all arts by type, then alphabetically
    //----------------------------------------//
    private void RegistryDisplayAllByType()
    //----------------------------------------//
    {
        foreach (Transform child in registryDisplayContentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Spawn arts and display
        foreach (List<ArtData> artDataList in artRegistryByType)
        {
            foreach (ArtData data in artDataList)
            {
                if ((int)data.artComplexity <= (int)registryComplexityDropdown.value)
                {
                    RegistryArt newArt = GameObject.Instantiate(registryArtPrefab);
                    newArt.SetupArt(data);
                    newArt.transform.SetParent(registryDisplayContentParent);
                }
            }
        }

    } // END RegistryDisplayAllByType


    // Displays all arts of a certain type alphabetically
    //----------------------------------------//
    private void RegistryDisplayAllOfType(ArtType artType)
    //----------------------------------------//
    {
        foreach (Transform child in registryDisplayContentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (ArtData data in artRegistryByType[(int)artType])
        {
            if ((int)data.artComplexity <= (int)registryComplexityDropdown.value)
            {
                RegistryArt newArt = GameObject.Instantiate(registryArtPrefab);
                newArt.SetupArt(data);
                newArt.transform.SetParent(registryDisplayContentParent);
            }
        }

    } // END RegistryDisplayAllOfType


    // Displays all arts in alphabetical order
    //----------------------------------------//
    private void RegistryDisplayAllAlphabetical()
    //----------------------------------------//
    {
        foreach (Transform child in registryDisplayContentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Spawn arts and display
        foreach (ArtData data in artRegistryAlphabetical)
        {
            if ((int)data.artComplexity <= (int)registryComplexityDropdown.value)
            {
                RegistryArt newArt = GameObject.Instantiate(registryArtPrefab);
                newArt.SetupArt(data);
                newArt.transform.SetParent(registryDisplayContentParent);
            }
        }

    } // END RegistryDisplayAllAlphabetical


    // OnRegistryValueChange, display accordingly
    //----------------------------------------//
    public void OnRegistryValueChange()
    //----------------------------------------//
    {
        switch (registryTypeDropdown.value)
        {
            case 0:
                if (registrySortDropdown.value == 0)
                {
                    RegistryDisplayAllByType();
                }
                else
                {
                    RegistryDisplayAllAlphabetical();
                }
                break;
            case 1:
                // TODO
                break;
            default:
                RegistryDisplayAllOfType((ArtType)Enum.ToObject(typeof(ArtType), registryTypeDropdown.value - 1));
                break;
        }

    } // END OnRegistryValueChange


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


    #region LEARN / UNLEARN ART


    // Learns art
    //----------------------------------------//
    public void LearnArt(ArtData artData)
    //----------------------------------------//
    {
        learnedArtsAlphabetical.Add(artData);
        SetupArts(learnedArtsAlphabetical.ToArray());

        if (learnableArtsContentParent.gameObject.activeSelf)
        {
            DisplayLearnable();
        }

    } // END LearnArt


    // Unlearns art
    //----------------------------------------//
    public void UnlearnArt(ArtData artData)
    //----------------------------------------//
    {
        learnedArtsAlphabetical.Remove(artData);
        SetupArts(learnedArtsAlphabetical.ToArray());

        if (learnableArtsContentParent.gameObject.activeSelf)
        {
            DisplayLearnable();
        }

    } // END UnlearnArt


    #endregion


    #region DISPLAY CUSTOM


    // Displays all arts by type, then alphabetically
    //----------------------------------------//
    private void DisplayCustomByType(List<List<ArtData>> artDatasToDisplay, Transform parentTransform, Art prefabToSpawn)
    //----------------------------------------//
    {
        foreach (Transform child in parentTransform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Spawn arts and display
        foreach (List<ArtData> artDataList in artDatasToDisplay)
        {
            foreach (ArtData data in artDataList)
            {
                
                Art newArt = GameObject.Instantiate(prefabToSpawn);
                newArt.SetupArt(data);
                newArt.transform.SetParent(parentTransform);
            }
        }

    } // END DisplayAllByType


    // Displays all arts by type, then alphabetically
    //----------------------------------------//
    private void DisplayCustomOfSingleType(List<List<ArtData>> artDatasToDisplay, ArtType typeToDisplay, Transform parentTransform, Art prefabToSpawn)
    //----------------------------------------//
    {
        foreach (Transform child in parentTransform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (ArtData data in artDatasToDisplay[(int)typeToDisplay])
        {
            
            Art newArt = GameObject.Instantiate(prefabToSpawn);
            newArt.SetupArt(data);
            newArt.transform.SetParent(parentTransform);
        }

    } // END DisplayAllByType


    // Displays all arts in alphabetical order
    //----------------------------------------//
    private void DisplayCustomAlphabetical(List<ArtData> artDatasToDisplay, Transform parentTransform, Art prefabToSpawn)
    //----------------------------------------//
    {
        foreach (Transform child in parentTransform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Spawn arts and display
        foreach (ArtData data in artDatasToDisplay)
        {
            
            Art newArt = GameObject.Instantiate(prefabToSpawn);
            newArt.SetupArt(data);
            newArt.transform.SetParent(parentTransform);
        }

    } // END DisplayAllAlphabetical


    #endregion


    #region LEARNABLE MENU


    // Displays learnable
    //----------------------------------------//
    public void DisplayLearnable()
    //----------------------------------------//
    {
        List<ArtData> learnableArtsAlpha = new List<ArtData>();

        // Get learnable arts
        foreach (ArtData artData in artRegistryAlphabetical)
        {
            // If the art is versatile (or ignore spec)...
            if (ignoreSpecToggle.isOn || !artData.specializationSpecific)
            {
                // If the art is lower than complexity...
                if ((int)artData.artComplexity <= (int)learnComplexityDropdown.value)
                {
                    // If we don't know the art already...
                    if (!learnedArtsAlphabetical.Contains(artData))
                    {
                        // Add art to learnable list
                        learnableArtsAlpha.Add(artData);
                    }
                }
            }
        }

        // Sort learnable arts
        learnableArtsAlpha = learnableArtsAlpha.OrderBy(t => t.artName).ToList();

        // Type sort
        if (learnSortDropdown.value == 0)
        {
            // Create new List List
            int numTypes = Enum.GetNames(typeof(ArtType)).Length;
            List<List<ArtData>> learnableArtsByType = new List<List<ArtData>>();
            for (int i = 0; i < numTypes; i++)
            {
                learnableArtsByType.Add(new List<ArtData>());
            }

            // Add all datas to respective type
            foreach (ArtData data in learnableArtsAlpha)
            {
                learnableArtsByType[(int)data.artType].Add(data);
            }

            // Sort type list list
            for (int i = 0; i < learnableArtsByType.Count; i++)
            {
                learnableArtsByType[i] = learnableArtsByType[i].OrderBy(t => t.artName).ToList();
            }

            // All arts
            if (learnFilterDropdown.value == 0)
            {
                DisplayCustomByType(learnableArtsByType, learnableArtsContentParent, learnableArtPrefab);
            }
            // Specific art type
            else if (learnFilterDropdown.value != 1)
            {
                DisplayCustomOfSingleType(learnableArtsByType, (ArtType)Enum.ToObject(typeof(ArtType), learnFilterDropdown.value - 2), learnableArtsContentParent, learnableArtPrefab);
            }
        }
        // Alpha sort
        else
        {
            DisplayCustomAlphabetical(learnableArtsAlpha, learnableArtsContentParent, learnableArtPrefab);
        }

    } // END DisplayLearnable


    // Displays learnable with prerequisites
    public void DisplayLearnableWithPrereqs(ArtType artType, ArtComplexity artComplexity)
    {

    }


    // Displays learnable from list
    public void DisplayLearnableFromList(List<ArtData> artDatas)
    {

    }


    #endregion


} // END ArtsManager.cs
