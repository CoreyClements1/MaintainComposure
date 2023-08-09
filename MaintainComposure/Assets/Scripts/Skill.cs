using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Skill : MonoBehaviour
{

    // Skill handles a single skill and its applications


    #region VARIABLES


    public string skillName;
    [SerializeField] private TMP_Text skillScoreText;
    [System.NonSerialized]
    public int skillScore = 10;
    public SkillApplication[] applications = new SkillApplication[4];


    #endregion


    #region NUMBERS


    // Adds to the skill score
    //----------------------------------------//
    public void AddToSkillScore(int numToAdd)
    //----------------------------------------//
    {
        skillScore += numToAdd;

        foreach(SkillApplication application in applications)
        {
            application.UpdateSkillScore(skillScore);
        }

        UpdateText();

    } // END AddToSkillScore


    // Adds to an application
    //----------------------------------------//
    public void AddToApp(string app, int numToAdd)
    //----------------------------------------//
    {
        foreach (SkillApplication a in applications)
        {
            if (a.applicationName == app)
            {
                a.AddBonuses(numToAdd);
                break;
            }
        }

    } // END AddToApp


    #endregion


    #region TEXT


    // Updates the text of skill score
    //----------------------------------------//
    public void UpdateText()
    //----------------------------------------//
    {
        skillScoreText.text = "" + skillScore;

    } // END UpdateText


    #endregion


} // END SkillManager.cs
