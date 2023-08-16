using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static InvItemsManager;

public class SpecializeManager : MonoBehaviour
{

    // SpecializeManager manages specializations panel


    #region VARIABLES


    public static SpecializeManager Instance { get; private set; }

    private List<SpecializationData> registrySpecDatasAlpha = new List<SpecializationData>();
    private List<List<SpecializationData>> registrySpecDatasByType = new List<List<SpecializationData>>();
    private List<string> currentCharSpecs = new List<string>();
    private List<int> currentCharSpecLevels = new List<int>();
    private List<SpecializationData> charSpecs;

    private SpecializationData currentlyDisplayedSpec;
    private int currentlyDisplayedLevel;
    [SerializeField] private LevelUpHandler levelUpHandler;

    // TXTs
    [SerializeField] private TextAsset specRegistryTxt;

    // UI
    [SerializeField] private Transform rightPanelSpecContentParent;
    [SerializeField] private SpecDisplayHandler rightPanelSpecPrefab;

    [SerializeField] private Transform registrySpecContentParent;
    [SerializeField] private SpecDisplayHandler registrySpecPrefab;
    [SerializeField] private TMP_Dropdown registryTypeDropdown;
    [SerializeField] private TMP_Dropdown registrySortDropdown;

    [SerializeField] private GameObject playerSpecLevelPrefab;
    [SerializeField] private Transform viewSpecContentParent;
    [SerializeField] private TMP_Text viewSpecNameText;
    [SerializeField] private SpecLevelDisplayHandler specLevelPrefab;

    [SerializeField] private NewSpecHandler newSpecHandler;


    #endregion


    #region SETUP


    // Sets up singleton
    //----------------------------------------//
    public void SetupSingleton()
    //----------------------------------------//
    {
        if (SpecializeManager.Instance == null)
        {
            SpecializeManager.Instance = this;
        }
        else
        {
            if (SpecializeManager.Instance != this)
            {
                Destroy(this);
            }
        }

        LoadRegistry();

    } // END SetupSingleton


    // Loads a character's specs
    public void SetupCharSpecs(string[] _currentCharSpecs, int[] _currentCharSpecLevels)
    {
        currentCharSpecs = _currentCharSpecs.ToList<string>();
        currentCharSpecLevels = _currentCharSpecLevels.ToList<int>();
        charSpecs = new List<SpecializationData>();

        // Clear old specs
        foreach (Transform child in rightPanelSpecContentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (currentCharSpecs != null)
        {
            // Add all datas to respective type
            foreach (string s in currentCharSpecs)
            {
                foreach(SpecializationData data in registrySpecDatasAlpha)
                {
                    if (data.specName == s)
                    {
                        charSpecs.Add(data);
                    }
                }
            }
        }

        // Display all
        DisplayRightPanel();

    } // END SetupObtainedItems


    // Sets up specs from scratch (empty)
    //----------------------------------------//
    public void SetupCharSpecsFromScratch()
    //----------------------------------------//
    {
        currentCharSpecs = new List<string>();
        currentCharSpecs.Add("Fighter");
        currentCharSpecLevels = new List<int>();
        currentCharSpecLevels.Add(0);

        // Clear old specs
        foreach (Transform child in rightPanelSpecContentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        SetupCharSpecs(currentCharSpecs.ToArray(), currentCharSpecLevels.ToArray());

        // Display all
        DisplayRightPanel();

    } // END SetupItemsFromScratch


    #endregion


    #region ADDING, REMOVING, SORTING LISTS


    // Adds an item to lists
    //----------------------------------------//
    private void AddItemToLists(SpecializationData dataToAdd, List<SpecializationData> alphaList, List<List<SpecializationData>> byTypeList, bool reSort)
    //----------------------------------------//
    {
        alphaList.Add(dataToAdd);
        byTypeList[(int)dataToAdd.starLevel].Add(dataToAdd);

        if (reSort)
        {
            ReorderLists(alphaList, byTypeList);
        }

    } // END AddItemToList


    // Removes an item from lists
    //----------------------------------------//
    private void RemoveItemFromLists(SpecializationData dataToRemove, List<SpecializationData> alphaList, List<List<SpecializationData>> byTypeList, bool reSort)
    //----------------------------------------//
    {
        alphaList.Remove(dataToRemove);
        byTypeList[(int)dataToRemove.starLevel].Remove(dataToRemove);

        if (reSort)
        {
            ReorderLists(alphaList, byTypeList);
        }

    } // END RemoveItemFromLists


    // Reorders lists
    //----------------------------------------//
    private void ReorderLists(List<SpecializationData> alphaList, List<List<SpecializationData>> byTypeList)
    //----------------------------------------//
    {
        alphaList = alphaList.OrderBy(t => t.specName).ToList();

        for (int i = 0; i < byTypeList.Count; i++)
        {
            byTypeList[i] = byTypeList[i].OrderBy(t => t.specName).ToList();
        }

    } // END ReorderLists


    #endregion


    #region DISPLAY


    // Displays specs by type
    //----------------------------------------//
    private void DisplayByType(List<List<SpecializationData>> specsToDisplay, Transform contentParent, SpecDisplayHandler prefabToSpawn)
    //----------------------------------------//
    {
        foreach (Transform child in contentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Spawn specs and display
        foreach (List<SpecializationData> specDataList in specsToDisplay)
        {
            foreach (SpecializationData data in specDataList)
            {
                SpecDisplayHandler newSpec = GameObject.Instantiate(prefabToSpawn);
                newSpec.SetupSpec(data);
                newSpec.transform.SetParent(contentParent);
            }
        }

    } // END DisplayInvByType


    // Displays specs alphabetically
    //----------------------------------------//
    private void DisplayAlpha(List<SpecializationData> specsToDisplay, Transform contentParent, SpecDisplayHandler prefabToSpawn)
    //----------------------------------------//
    {
        foreach (Transform child in contentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (SpecializationData data in specsToDisplay)
        {
            SpecDisplayHandler newSpec = GameObject.Instantiate(prefabToSpawn);
            newSpec.SetupSpec(data);
            newSpec.transform.SetParent(contentParent);
        }

    } // END DisplayAlpha


    // Displays specs of a certain type
    //----------------------------------------//
    private void DisplayOfType(List<List<SpecializationData>> specsToDisplay, int typeToDisplay, Transform contentParent, SpecDisplayHandler prefabToSpawn)
    //----------------------------------------//
    {
        foreach (Transform child in contentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (SpecializationData data in specsToDisplay[(int)typeToDisplay])
        {
            SpecDisplayHandler newSpec = GameObject.Instantiate(prefabToSpawn);
            newSpec.SetupSpec(data);
            newSpec.transform.SetParent(contentParent);
        }

    } // END DisplayInvOfType


    #endregion


    #region RIGHT PANEL


    // Displays right panel
    //----------------------------------------//
    public void DisplayRightPanel()
    //----------------------------------------//
    {
        foreach (Transform child in rightPanelSpecContentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < charSpecs.Count; i++)
        {
            SpecDisplayHandler newSpec = GameObject.Instantiate(rightPanelSpecPrefab);
            newSpec.SetupSpec(charSpecs[i], currentCharSpecLevels[i]);
            newSpec.transform.SetParent(rightPanelSpecContentParent);
        }

    } // END OnInvValueChange


    // Adds a spec to the right panel
    //----------------------------------------//
    public void AddCharSpec(SpecializationData addedSpec)
    //----------------------------------------//
    {
        currentCharSpecs.Add(addedSpec.specName);
        currentCharSpecLevels.Add(0);

        SetupCharSpecs(currentCharSpecs.ToArray(), currentCharSpecLevels.ToArray());

    } // EMD AddCharSpec


    // Gets char specs
    //----------------------------------------//
    public string[] GetCharSpecs()
    //----------------------------------------//
    {
        return currentCharSpecs.ToArray();

    } // END GetCharSpecs


    // Gets char spec levels
    //----------------------------------------//
    public int[] GetCharSpecLevels()
    //----------------------------------------//
    {
        return currentCharSpecLevels.ToArray();

    } // END GetCharSpecLevels


    #endregion


    #region REGISTRY DISPLAY


    // On registry header value change, display accordingly
    //----------------------------------------//
    public void OnRegistryHeaderValueChange()
    //----------------------------------------//
    {
        switch (registryTypeDropdown.value)
        {
            case 0:
                if (registrySortDropdown.value == 0)
                {
                    DisplayByType(registrySpecDatasByType, registrySpecContentParent, registrySpecPrefab);
                }
                else
                {
                    DisplayAlpha(registrySpecDatasAlpha, registrySpecContentParent, registrySpecPrefab);
                }
                break;
            default:
                DisplayOfType(registrySpecDatasByType, registryTypeDropdown.value - 1, registrySpecContentParent, registrySpecPrefab);
                break;
        }

    } // END OnRegistryHeaderValueChange


    // Adds a spec to the registry
    //----------------------------------------//
    public void AddRegistrySpec(SpecializationData addedSpec)
    //----------------------------------------//
    {
        AddItemToLists(addedSpec, registrySpecDatasAlpha, registrySpecDatasByType, true);
        SaveRegistry();
        LoadRegistry();
        OnRegistryHeaderValueChange();

    } // EMD AddRegistrySpec


    // Removes a spec from the registry
    //----------------------------------------//
    public void RemoveRegistrySpec(SpecializationData addedSpec)
    {
        RemoveItemFromLists(addedSpec, registrySpecDatasAlpha, registrySpecDatasByType, true);
        SaveRegistry();
        LoadRegistry();
        OnRegistryHeaderValueChange();

    } // END RemoveRegistrySpec


    #endregion


    #region REGISTRY SAVE / LOAD


    // Loads items from registry
    //----------------------------------------//
    private void LoadRegistry()
    //----------------------------------------//
    {
        registrySpecDatasAlpha.Clear();
        registrySpecDatasByType.Clear();

        // Load saved characters into characterDatas
        SpecializationData[] loadedSpecs = JsonHelper.FromJsonArray<SpecializationData>(specRegistryTxt.text);

        if (loadedSpecs == null)
        {
            registrySpecDatasAlpha = new List<SpecializationData>();
        }
        else
        {
            registrySpecDatasAlpha = loadedSpecs.ToList();
        }

        if (registrySpecDatasAlpha != null)
        {
            registrySpecDatasAlpha = registrySpecDatasAlpha.OrderBy(t => t.specName).ToList();

            int numTypes = 5;

            // Create new List List
            for (int i = 0; i < numTypes; i++)
            {
                registrySpecDatasByType.Add(new List<SpecializationData>());
            }

            // Add all datas to respective type
            foreach (SpecializationData data in registrySpecDatasAlpha)
            {
                registrySpecDatasByType[(int)data.starLevel].Add(data);
            }

            for (int i = 0; i < registrySpecDatasByType.Count; i++)
            {
                registrySpecDatasByType[i] = registrySpecDatasByType[i].OrderBy(t => t.specName).ToList();
            }
        }

    } // END LoadRegistry


    // Saves spec registry
    //----------------------------------------//
    private void SaveRegistry()
    //----------------------------------------//
    {
        // Save as JSON
        FileHelper.SaveAsJsonArray<SpecializationData>("SpecializationRegistry", registrySpecDatasAlpha.ToArray(), false);

    } // END SaveRegistry


    // Consolidates specs from update txt into registry
    //----------------------------------------//
    private void ConsolidateUpdateRegistry()
    //----------------------------------------//
    {
        SpecializationData[] updateRegistryDatas = JsonHelper.FromJsonArray<SpecializationData>(specRegistryTxt.text);

        foreach (SpecializationData data in updateRegistryDatas)
        {
            bool inRegistry = false;
            for (int i = 0; i < registrySpecDatasAlpha.Count; i++)
            {
                if (data.specName == registrySpecDatasAlpha[i].specName)
                {
                    registrySpecDatasAlpha[i] = data;
                    inRegistry = true;
                    break;
                }
            }

            if (!inRegistry)
            {
                registrySpecDatasAlpha.Add(data);
            }
        }

        SaveRegistry();

    } // END ConsolidateUpdateRegistry


    #endregion


    #region VIEW DISPLAY


    // Displays specialization in the level up panel
    //----------------------------------------//
    public void DisplaySpecInLevelUpPanel(SpecializationData specToDisplay, bool playerObtained, int playerLevel)
    //----------------------------------------//
    {
        currentlyDisplayedSpec = specToDisplay;
        currentlyDisplayedLevel = playerLevel;

        LearnMenuManager.Instance.ShowSpecializationPanel();
        viewSpecNameText.text = specToDisplay.specName + " (" + specToDisplay.numLevels + " Levels)";

        foreach(Transform child in viewSpecContentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Display if player level is 0
        if (playerObtained && playerLevel == 0)
        {
            GameObject.Instantiate(playerSpecLevelPrefab).transform.SetParent(viewSpecContentParent);
        }

        for (int i = 0; i < specToDisplay.numLevels; i++)
        {
            // Spawn level
            SpecLevelDisplayHandler specLevel = GameObject.Instantiate(specLevelPrefab);
            specLevel.transform.SetParent(viewSpecContentParent);
            specLevel.SetupDisplay(specToDisplay.levelUpDatas[i], i);

            // Display player level if necessary
            if (playerObtained && playerLevel == i + 1)
            {
                GameObject.Instantiate(playerSpecLevelPrefab).transform.SetParent(viewSpecContentParent);
            }
        }

    } // END DisplaySpecInLevelUpPanel


    #endregion


    #region NEW AND EDIT SPEC


    // Starts creating new specialization
    //----------------------------------------//
    public void BeginCreateNewSpec()
    //----------------------------------------//
    {
        newSpecHandler.gameObject.SetActive(true);
        newSpecHandler.SetupDataForNew();

    } // END BeginCreateNewSpec


    // Ends creating new spec and adds to registry
    //----------------------------------------//
    public void EndCreateNewSpec(SpecializationData newSpecData)
    //----------------------------------------//
    {
        AddRegistrySpec(newSpecData);
        newSpecHandler.HidePanel();

    } // END EndCreateNewSpec


    // Begins editing specialization
    //----------------------------------------//
    public void BeginEditSpec(SpecializationData specDataToEdit)
    //----------------------------------------//
    {
        newSpecHandler.gameObject.SetActive(true);
        newSpecHandler.SetupDataForEdit(specDataToEdit);

    } // END BeginEditSpec


    // Ends editing specialization and updates registry
    //----------------------------------------//
    public void EndEditSpec(string oldSpecName, SpecializationData updatedSpecData)
    //----------------------------------------//
    {
        for (int i = 0; i < registrySpecDatasAlpha.Count; i++)
        {
            if (registrySpecDatasAlpha[i].specName == oldSpecName)
            {
                registrySpecDatasAlpha[i] = updatedSpecData;
                SaveRegistry();
                LoadRegistry();
                OnRegistryHeaderValueChange();
                newSpecHandler.HidePanel();
            }
        }

    } // END EndEditSpec


    #endregion


    #region LEVEL UP


    // On level up display level up panel
    //----------------------------------------//
    public void OnLevelUp()
    //----------------------------------------//
    {
        levelUpHandler.gameObject.SetActive(true);
        levelUpHandler.SetupLevelUp(currentlyDisplayedSpec, currentlyDisplayedLevel - 1);

    } // END OnLevelUp


    // Ends level up
    //----------------------------------------//
    public void EndLevelup()
    //----------------------------------------//
    {
        int idx = currentCharSpecs.IndexOf(currentlyDisplayedSpec.specName);
        currentCharSpecLevels[idx] += 1;
        DisplaySpecInLevelUpPanel(currentlyDisplayedSpec, true, currentCharSpecLevels[idx]);
        DisplayRightPanel();

    } // END EndLevelUp


    // Displays choosable arts
    //----------------------------------------//
    public void DisplayChoosableSpecs(LevelUpHandlerAttribute handlerAttribute, Transform contentParent, LevelUpChooseSpec chooseSpecPrefab)
    //----------------------------------------//
    {
        foreach (Transform child in contentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Any of star level
        if (handlerAttribute.levelUpAspect.assocString == "Any" || handlerAttribute.levelUpAspect.assocString == "")
        {
            foreach (SpecializationData data in registrySpecDatasByType[handlerAttribute.levelUpAspect.assocIncNumber])
            {
                LevelUpChooseSpec newChooseSpec = GameObject.Instantiate(chooseSpecPrefab);
                newChooseSpec.transform.SetParent(contentParent);
                newChooseSpec.SetupSpec(data, handlerAttribute, "Choose");
            }
        }
        // From list
        else
        {
            List<string> namesToDisplay = handlerAttribute.levelUpAspect.assocString.Split(", ").ToList();
            foreach (SpecializationData data in registrySpecDatasByType[handlerAttribute.levelUpAspect.assocIncNumber])
            {
                foreach (string s in namesToDisplay)
                {
                    if (data.specName == s)
                    {
                        LevelUpChooseSpec newChooseSpec = GameObject.Instantiate(chooseSpecPrefab);
                        newChooseSpec.transform.SetParent(contentParent);
                        newChooseSpec.SetupSpec(data, handlerAttribute, "Choose");
                        break;
                    }
                }
            }
        }

    } // END DisplayChoosableSpecs


    #endregion


} // END SpecializeManager.cs


[System.Serializable]
public class SpecializationData
{

    // SpecializationData contains necessary information on a specialization


    #region VARIABLES


    public string specName = "";
    public int starLevel = 0;
    public bool anySpecFollows = true;
    public int numLevels = 5;
    public int thresholdAfterLevel = 0;
    public int thresholdNeedToReachLevel = 0;
    public string[] followingSpecs;
    public LevelUpData[] levelUpDatas = null;


    #endregion


    #region SETUP


    // Constructor
    //----------------------------------------//
    public SpecializationData(string _specName, bool _anySpecFollows, int _numLevels, string[] _followingSpecs, int _thresholdAfterLevel, int _thresholdNeedToReachLevel, LevelUpData[] _levelUpDatas)
    //----------------------------------------//
    {
        specName = _specName;
        anySpecFollows = _anySpecFollows;
        numLevels = _numLevels;
        levelUpDatas = _levelUpDatas;
        followingSpecs = _followingSpecs;
        thresholdAfterLevel = _thresholdAfterLevel;
        thresholdNeedToReachLevel = _thresholdNeedToReachLevel;

    } // END Constructor


    // Constructor
    //----------------------------------------//
    public SpecializationData()
    //----------------------------------------//
    {
        specName = "";
        anySpecFollows = true;
        numLevels = 0;
        levelUpDatas = null;
        followingSpecs = null;
        thresholdAfterLevel = 0;
        thresholdNeedToReachLevel = 0;

    } // END Constructor


    #endregion


} // END SpecializationData.cs


[System.Serializable]
public class LevelUpData
{

    // LevelUpData contains necessary information for a level up


    #region VARIABLES


    public string levelUpName;
    public LevelUpAspect[] levelUpAspects;


    #endregion


    #region SETUP


    // Constructor
    //----------------------------------------//
    public LevelUpData(string _levelUpName, LevelUpAspect[] _levelUpAspects)
    //----------------------------------------//
    {
        levelUpName = _levelUpName;
        levelUpAspects = _levelUpAspects;

    } // END Constructor


    // Constructor
    //----------------------------------------//
    public LevelUpData()
    //----------------------------------------//
    {
        levelUpName = string.Empty;
        levelUpAspects = null;

    } // END Constructor


    #endregion


} // END LevelUpData.cs


[System.Serializable]
public class LevelUpAspect
{

    // LevelUpAspect contains details on a single aspect of level up


    #region VARIABLES


    [System.Serializable]
    public enum LevelUpType
    {
        SkillIncrease, // assocIncNumber: skill to increase (-1 = any)
        AppBonus, // assocString: app or skill to increase ("Any" = any)
        MaxComposureIncrease, // assocIncNumber: amount of assoc die; assocDie: die to inc by
        SpeedIncrease, // assocIncNumber: amount to increase speed by
        ThresholdIncrease, // assocIncNumber: amount to increase threshold by
        ActIncrease, // N/A; increase acts by 1
        DefianceIncrease, // N/A; increase defiances by 1
        LearnSpecificArt, // assocString: specific art to learn; assocArtType: art type; assocIncNumber: complexity
        LearnArtFromList, // assocString: art "group" (eg, "Heavy Attack"); assocString2: art list (comma separated)
        LearnArtOfType, // assocArtType: art type to learn; assocIncNumber: max complexity
        LearnAnyArt, // assocIncNumber: max complexity
        UnlockSpec, // assocIncNumber: spec star rating; assocString: required spec group ("Any" = none)
        Other, // assocString: special command (NOTE: MUST BE HANDLED IN BACKEND)
    }

    public LevelUpType levelUpType;

    public int assocIncNumber = 0;
    public string assocString = "";
    public string assocString2 = "";
    public ArtsManager.ArtType assocArtType;
    public InvItemsManager.DamageDie assocDie;


    #endregion


    #region SETUP


    // Constructor
    //----------------------------------------//
    public LevelUpAspect(LevelUpType _levelUpType)
    //----------------------------------------//
    {
        levelUpType = _levelUpType;

    } // END Constructor


    #endregion


} // END LevelUpAspect.cs
