using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{

    // SkillsManager manages all skills


    #region VARIABLES


    public static SkillsManager Instance { get; private set; }
    public Skill[] skills = new Skill[5];


    #endregion


    #region MONOBEHAVIOUR AND SETUP


    // Sets up singleton
    //----------------------------------------//
    public void SetupSingleton()
    //----------------------------------------//
    {
        if (SkillsManager.Instance == null)
        {
            SkillsManager.Instance = this;
        }
        else
        {
            if (SkillsManager.Instance != this)
            {
                Destroy(this);
            }
        }

    } // END SetupSingleton


    // Sets up skills
    //----------------------------------------//
    public void SetupSkills(int[] skillScores, int[] appBonuses)
    //----------------------------------------//
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].skillScore = skillScores[i];
            skills[i].UpdateText();

            for (int j = 0; j < 4; j++)
            {
                skills[i].applications[j].UpdateSkillScore(skillScores[i]);
                skills[i].applications[j].applicationBonuses = appBonuses[(i * 4) + j];
                skills[i].applications[j].UpdateText();
            }
        }

    } // END SetupSkills


    // Sets up skills from scratch (all 0's)
    //----------------------------------------//
    public void SetupSkillsFromScratch()
    //----------------------------------------//
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].skillScore = 0;
            skills[i].UpdateText();

            for (int j = 0; j < 4; j++)
            {
                skills[i].applications[j].UpdateSkillScore(0);
                skills[i].applications[j].applicationBonuses = 0;
                skills[i].applications[j].UpdateText();
            }
        }

    } // END SetupSkillsFromScratch


    #endregion


    #region ADD TO SKILL / APP


    // Adds to a skill
    //----------------------------------------//
    public void AddToSkill(string skill, int numToAdd)
    //----------------------------------------//
    {
        foreach(Skill s in skills) 
        { 
            if (s.skillName == skill)
            {
                s.AddToSkillScore(numToAdd);
                break;
            }
        }

    } // END AddToSkill


    // Adds to an application
    //----------------------------------------//
    public void AddToApp(string app, int numToAdd)
    //----------------------------------------//
    {
        switch (app)
        {
            case "Force":
            case "Smash":
            case "Throw":
            case "Wrestle":
                skills[0].AddToApp(app, numToAdd);
                break;

            case "Draw":
            case "Jump":
            case "Run":
            case "Sneak":
                skills[1].AddToApp(app, numToAdd);
                break;

            case "Analyze":
            case "Deduce":
            case "Learn":
            case "Know":
                skills[2].AddToApp(app, numToAdd);
                break;

            case "Feel":
            case "Hear":
            case "React":
            case "See":
                skills[3].AddToApp(app, numToAdd);
                break;

            case "Charm":
            case "Glean":
            case "Manipulate":
            case "Persuade":
                skills[4].AddToApp(app, numToAdd);
                break;

            default:
                Debug.LogError("No application \"" + app + "\" found");
                break;
        }

    } // END AddToApp


    #endregion


} // END SkillsManager.cs
