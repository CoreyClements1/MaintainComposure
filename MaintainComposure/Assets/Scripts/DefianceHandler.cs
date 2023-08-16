using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefianceHandler : MonoBehaviour
{

    // DefianceHandler handles a displayed defiance


    #region VARIABLES


    public Toggle defianceToggle;


    #endregion


    #region ON CHANGE


    // Notify general area on change
    //----------------------------------------//
    public void OnToggleValChange()
    //----------------------------------------//
    {
        GeneralAreaManager.Instance.UpdateDefiancesToDisplayed();

    } // END OnToggleValChange


    #endregion


} // END DefianceHandler
