using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HeaderAndSaveManager : MonoBehaviour
{

    // HeaderAndSaveManager manages the header area and saving / loading


    #region VARIABLES


    public static HeaderAndSaveManager Instance { get; private set; }

    private int activeCharId = 0;

    [SerializeField] private TextAsset savedCharsJson;
    private CharacterData[] characterDatas;

    [SerializeField] private Transform charChooserParent;
    [SerializeField] private CharDisplay charDisplayPrefab;

    [SerializeField] public TMP_Text charNameText;

    private int currentMaxId = 0;

    [SerializeField] private GameObject savedTextPrefab;
    [SerializeField] private Transform savedTextArea;


    #endregion


    #region SETUP


    // Sets up singleton
    //----------------------------------------//
    public void SetupSingleton()
    //----------------------------------------//
    {
        if (HeaderAndSaveManager.Instance == null)
        {
            HeaderAndSaveManager.Instance = this;
        }
        else
        {
            if (HeaderAndSaveManager.Instance != this)
            {
                Destroy(this);
            }
        }

    } // END SetupSingleton


    // Loads all characters and populates them into load list
    //----------------------------------------//
    public void InitialLoad()
    //----------------------------------------//
    {
        // Load saved characters into characterDatas
        characterDatas = JsonHelper.FromJsonArray<CharacterData>(savedCharsJson.text);

        if (characterDatas != null)
        {
            foreach (CharacterData characterData in characterDatas)
            {
                if (characterData.characterId > currentMaxId)
                {
                    currentMaxId = characterData.characterId;
                }

                SpawnCharDisplay(characterData);
            }
        }

    } // END InitialLoad


    #endregion


    #region CHAR DISPLAYS


    // Clears char displays
    //----------------------------------------//
    private void ClearCharDisplays()
    //----------------------------------------//
    {
        for (int i = 0; i < charChooserParent.childCount - 1; i++)
        {
            GameObject.Destroy(charChooserParent.GetChild(i).gameObject);
        }

    } // END ClearCharDisplays


    // Spawns a new char display
    //----------------------------------------//
    private void SpawnCharDisplay(CharacterData characterData)
    //----------------------------------------//
    {
        CharDisplay newCharDisplay = GameObject.Instantiate(charDisplayPrefab);

        newCharDisplay.SetupDisplay(characterData.characterId, characterData.characterName);
        newCharDisplay.transform.SetParent(charChooserParent);

        Transform newButton = charChooserParent.GetChild(charChooserParent.childCount - 2);
        newButton.SetParent(this.transform);
        newButton.SetParent(charChooserParent);

    } // END SpawnCharDisplay


    #endregion


    #region SAVING


    // Saves current character
    //----------------------------------------//
    public void SaveCharacter()
    //----------------------------------------//
    {
        CharacterData characterData = new CharacterData();

        // Header
        characterData.characterId = activeCharId;
        characterData.characterName = charNameText.text;

        // General
        characterData.currentActs = GeneralAreaManager.Instance.currentActs;
        characterData.maxActs = GeneralAreaManager.Instance.maxActs;

        characterData.currentComposure = GeneralAreaManager.Instance.currentComposure;
        characterData.maxComposure = GeneralAreaManager.Instance.maxComposure;
        characterData.tempComposure = GeneralAreaManager.Instance.tempComposure;

        characterData.currentDefiances = GeneralAreaManager.Instance.currentDefiances;
        characterData.maxDefiances = GeneralAreaManager.Instance.maxDefiances;

        characterData.composureThreshold = GeneralAreaManager.Instance.composureThreshold;

        characterData.speedBonuses = GeneralAreaManager.Instance.speedBonuses;
        characterData.speedMultipliers = GeneralAreaManager.Instance.speedMultipliers;

        // Stats and applications
        for (int i = 0; i < 5; i++)
        {
            characterData.skillScores[i] = SkillsManager.Instance.skills[i].skillScore;

            for (int j = 0; j < 4; j++)
            {
                characterData.appBonuses[(i * 4) + j] = SkillsManager.Instance.skills[i].applications[j].applicationBonuses;
            }
        }

        // Arts
        characterData.knownArtDatas = ArtsManager.Instance.GetArtsAsArr();
        characterData.itemDatas = InvItemsManager.Instance.GetItemsAsArr();

        // Specs
        characterData.knownSpecNames = SpecializeManager.Instance.GetCharSpecs();
        characterData.specLevels = SpecializeManager.Instance.GetCharSpecLevels();

        // Overwrite or add new character to saved characters array
        if (characterDatas == null)
        {
            characterDatas = new CharacterData[1];

            characterDatas[0] = characterData;
        }
        else
        {
            bool overwrote = false;
            for (int i = 0; i < characterDatas.Length; i++)
            {
                if (characterDatas[i].characterId == activeCharId)
                {
                    characterDatas[i] = characterData;
                    overwrote = true;
                }
            }

            if (!overwrote)
            {
                List<CharacterData> tempList = characterDatas.ToList();
                tempList.Add(characterData);
                characterDatas = tempList.ToArray();
            }
        }

        // Save as JSON
        FileHelper.SaveAsJsonArray<CharacterData>("SavedCharacters", characterDatas, false);

        Transform newTrans = GameObject.Instantiate(savedTextPrefab, savedTextArea.position, savedTextPrefab.transform.rotation).transform;
        newTrans.SetParent(savedTextArea);

        // Reload char displays
        ClearCharDisplays();
        foreach (CharacterData newData in characterDatas)
        {
            SpawnCharDisplay(newData);
        }

    } // END SaveCharacter


    // Saves the characters in characterDatas
    //----------------------------------------//
    public void SaveCharactersFromDatas()
    //----------------------------------------//
    {
        // Save as JSON
        FileHelper.SaveAsJsonArray<CharacterData>("SavedCharacters", characterDatas, false);

        // Reload char displays
        ClearCharDisplays();

        if (characterDatas.Length != 0)
        {
            foreach (CharacterData newData in characterDatas)
            {
                SpawnCharDisplay(newData);
            }
        }

    } // END SaveCharacter


    #endregion


    #region LOADING


    // Loads character from id
    //----------------------------------------//
    public void LoadCharacter(int playerId)
    //----------------------------------------//
    {
        // Find data with matching ID
        CharacterData dataToLoad = null;

        foreach (CharacterData charData in characterDatas)
        {
            if (charData.characterId == playerId)
            {
                activeCharId = charData.characterId;
                dataToLoad = charData;
                break;
            }
        }

        if (dataToLoad == null)
        {
            Debug.LogError("Something went wrong oops");
            return;
        }

        // Load data
        SkillsManager.Instance.SetupSkills(dataToLoad.skillScores, dataToLoad.appBonuses);
        GeneralAreaManager.Instance.SetupAttributes(dataToLoad);
        ArtsManager.Instance.SetupArts(dataToLoad.knownArtDatas);
        InvItemsManager.Instance.SetupObtainedItems(dataToLoad.itemDatas);
        SpecializeManager.Instance.SetupCharSpecs(dataToLoad.knownSpecNames, dataToLoad.specLevels);

        charNameText.text = dataToLoad.characterName;

        LearnMenuManager.Instance.HidePanel();

    } // END LoadCharacter


    #endregion


    #region NEW CHARACTER


    // Creates new character
    //----------------------------------------//
    public void OnNewCharacter(string newCharName)
    //----------------------------------------//
    {
        // Create new data
        SkillsManager.Instance.SetupSkillsFromScratch();
        GeneralAreaManager.Instance.SetupAttributesFromScratch();
        ArtsManager.Instance.SetupArtsFromScratch();
        InvItemsManager.Instance.SetupItemsFromScratch();
        SpecializeManager.Instance.SetupCharSpecsFromScratch();

        CharacterData characterData = new CharacterData();

        currentMaxId++;
        characterData.characterId = currentMaxId;
        activeCharId = currentMaxId;

        characterData.characterName = newCharName;

        charNameText.text = characterData.characterName;

        SpawnCharDisplay(characterData);
        SaveCharacter();

        LearnMenuManager.Instance.HidePanel();

    } // END OnNewCharacter


    #endregion


    #region DELETE CHARACTER


    // Deletes a character
    //----------------------------------------//
    public void DeleteCharacter(int charIdToDelete)
    //----------------------------------------//
    {
        // Find character to delete by id
        for (int i = 0; i < characterDatas.Length; i++)
        {
            if (characterDatas[i].characterId == charIdToDelete)
            {
                List<CharacterData> tempList = characterDatas.ToList();
                tempList.Remove(characterDatas[i]);
                characterDatas = tempList.ToArray();

                SaveCharactersFromDatas();

                if (charIdToDelete == activeCharId)
                {
                    LearnMenuManager.Instance.SetAbleToClose(false);
                }

                return;
            }
        }

        // If we made it here, something went wrong
        Debug.LogError("Oops something went wrong, can't delete!");

    } // END DeleteCharacter


    #endregion


    #region NAME CHANGE


    // Changes the character name
    //----------------------------------------//
    public void ChangeCharName(string newName)
    //----------------------------------------//
    {
        charNameText.text = newName;

    } // END ChangeCharName


    #endregion


    #region UPDATE


    // Updates all to match update files
    //----------------------------------------//
    public void UpdateRegistriesFromFiles()
    //----------------------------------------//
    {
        ArtsManager.Instance.ConsolidateUpdateRegistry();

    } // END UpdateRegistriesFromFiles


    #endregion


    #region EXIT


    // On exit button, quit game
    //----------------------------------------//
    public void OnExitButton()
    //----------------------------------------//
    {
        Application.Quit();

    } // END OnExitButton


    #endregion


} // END HeaderAndSaveManager.cs


[System.Serializable]
public class CharacterData
{

    // CharacterData contains data for saving and loading a character


    // Header
    public int characterId;
    public string characterName;

    // General
    public int currentActs;
    public int maxActs;

    public int currentComposure = 4;
    public int maxComposure = 4;
    public int tempComposure;

    public int currentDefiances;
    public int maxDefiances;

    public int composureThreshold;

    public int speedBonuses;
    public float speedMultipliers;

    // Stats and Applications
    public int[] skillScores = new int[5];
    public int[] appBonuses = new int[20];

    // Arts
    public string[] knownArtDatas;
    public ItemData[] itemDatas;

    // Specs
    public string[] knownSpecNames;
    public int[] specLevels;


} // END CharacterData.cs
