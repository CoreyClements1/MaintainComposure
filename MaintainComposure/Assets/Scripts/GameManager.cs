using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // GameManager manages the game


    #region VARIABLES


    [SerializeField] private GeneralAreaManager generalAreaManager;
    [SerializeField] private SkillsManager skillsManager;
    [SerializeField] private ArtsManager artsManager;
    [SerializeField] private InvItemsManager invItemsManager;
    [SerializeField] private HeaderAndSaveManager headerAndSaveManager;
    [SerializeField] private LearnMenuManager learnMenuManager;
    [SerializeField] private TooltipManager tooltipManager;


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
        headerAndSaveManager.SetupSingleton();
        headerAndSaveManager.InitialLoad();

        generalAreaManager.SetupSingleton();
        skillsManager.SetupSingleton();
        artsManager.SetupSingleton();

        invItemsManager.SetupSingleton();

        learnMenuManager.SetupSingleton();
        tooltipManager.SetupSingleton();

    } // END Setup


    #endregion


} // END GameManager.cs
