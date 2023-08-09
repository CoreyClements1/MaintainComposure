using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // GameManager manages the game


    #region VARIABLES


    [SerializeField] private GeneralAreaManager generalAreaManager;
    [SerializeField] private SkillsManager skillsManager;


    #endregion


    #region MONOBEHAVIOUR AND SETUP


    // Start
    //----------------------------------------//
    void Start()
    //----------------------------------------//
    {
        SetupGame();

    } // END Start


    // Sets up game
    //----------------------------------------//
    private void SetupGame()
    //----------------------------------------//
    {
        generalAreaManager.SetupSingleton();
        generalAreaManager.SetupAttributes();

        skillsManager.SetupSingleton();
        skillsManager.SetupSkills();

    } // END Setup


    #endregion


    #region SAVING


    // Saves current character
    //----------------------------------------//
    public void SaveCharacter()
    //----------------------------------------//
    {
        CharacterData characterData = new CharacterData();

        // Header
        characterData.characterId = 0; // TODO
        characterData.characterName = "Character Name"; // TODO

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
        for (int i = 0; i < 5; i ++ )
        {
            characterData.skillScores[i] = SkillsManager.Instance.skills[i].skillScore;

            for (int j = 0; j < 4; j++)
            {
                characterData.appBonuses[(i * 4) + j] = SkillsManager.Instance.skills[i].applications[j].applicationBonuses;
            }
        }

        string json = JsonHelper.ToJson<CharacterData>(characterData, true);
        FileHelper.ExportToTxt("Character" + characterData.characterId, json);

    } // END SaveCharacter


    #endregion


} // END GameManager.cs


public class CharacterData
{

    // CharacterData contains data for saving and loading a character


    // Header
    public int characterId;
    public string characterName;

    // General
    public int currentActs;
    public int maxActs;

    public int currentComposure;
    public int maxComposure;
    public int tempComposure;

    public int currentDefiances;
    public int maxDefiances;

    public int composureThreshold;

    public int speedBonuses;
    public float speedMultipliers;

    // Stats and Applications
    public int[] skillScores = new int[5];
    public int[] appBonuses = new int[20];


} // END CharacterData.cs
