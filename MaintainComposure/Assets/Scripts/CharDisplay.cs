using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharDisplay : MonoBehaviour
{

    // CharDisplay handles the UI area for displaying a character to load


    #region VARIABLES


    [SerializeField] private TMP_Text charNameText;
    public int assocCharId { get; private set; }
    public string assocCharName { get; private set; }


    #endregion


    #region SETUP


    // Sets up the display
    //----------------------------------------//
    public void SetupDisplay(int _assocCharId, string _assocCharName)
    //----------------------------------------//
    {
        assocCharId = _assocCharId;
        assocCharName = _assocCharName;

        charNameText.text = assocCharName;

    } // END SetupDisplay


    #endregion


    #region BUTTONS


    // On open button, load character
    //----------------------------------------//
    public void OnOpenButton()
    //----------------------------------------//
    {
        HeaderAndSaveManager.Instance.LoadCharacter(assocCharId);

    } // END OnOpenButton


    // On delete button, prompt confirmation, then delete
    //----------------------------------------//
    public void OnDeleteButton()
    //----------------------------------------//
    {
        LearnMenuManager.Instance.OpenConfirmDeletePanel(assocCharId);

    } // END OnDeleteButton


    #endregion


} // END CharDisplay
