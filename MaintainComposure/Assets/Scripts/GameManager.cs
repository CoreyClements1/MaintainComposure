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
    [SerializeField] private HeaderAndSaveManager headerAndSaveManager;
    [SerializeField] private LearnMenuManager learnMenuManager;


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
        learnMenuManager.SetupSingleton();

        ArtData[] testDatas = new ArtData[]
        {
            new ArtData("a Test Art", ArtsManager.ArtType.Fundamental, ArtsManager.ArtComplexity.Basic, 1, "1 meters", false, false, true, "A test ability, model \"a\""),
            new ArtData("d Test Art", ArtsManager.ArtType.Fundamental, ArtsManager.ArtComplexity.Basic, 1, "4 meters", false, false, false, "A test ability, model \"d\""),
            new ArtData("c Test Art", ArtsManager.ArtType.Fundamental, ArtsManager.ArtComplexity.Basic, 1, "3 meters", true, false, false, "A test ability, model \"c\""),
            new ArtData("b Test Art", ArtsManager.ArtType.Reactionary, ArtsManager.ArtComplexity.Advanced, 1, "2 meters", false, false, true, "A test ability, model \"b\""),
            new ArtData("e Test Art", ArtsManager.ArtType.Combat, ArtsManager.ArtComplexity.Expert, 1, "5 meters", false, false, false, "A test ability, model \"e\""),
            new ArtData("f Test Art", ArtsManager.ArtType.Reactionary, ArtsManager.ArtComplexity.Basic, 1, "6 meters", true, false, false, "A test ability, model \"f\""),
        };

        
        //artsManager.SetupArts(testDatas);

    } // END Setup


    #endregion


} // END GameManager.cs
