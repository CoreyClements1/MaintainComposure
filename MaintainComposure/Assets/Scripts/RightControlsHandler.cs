using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RightControlsHandler : MonoBehaviour
{

    // RightControlsHandler handles the right panel control section


    #region VARIABLES


    [SerializeField] private TMP_InputField compInpField;
    [SerializeField] private TMP_InputField actInpField;


    #endregion


    #region BUTTONS


    // Heals player
    //----------------------------------------//
    public void OnHealComp()
    //----------------------------------------//
    {
        GeneralAreaManager.Instance.AddToCurrentComposure(Int32.Parse(compInpField.text));

    } // END OnHealComp


    // Damages composure
    //----------------------------------------//
    public void OnDmgComp()
    //----------------------------------------//
    {
        GeneralAreaManager.Instance.AddToCurrentComposure(-Int32.Parse(compInpField.text));

    } // END OnDmgComp


    // Recovers acts
    //----------------------------------------//
    public void OnRecoverAct()
    //----------------------------------------//
    {
        GeneralAreaManager.Instance.AddToCurrentActs(Int32.Parse(actInpField.text));

    } // END OnRecoverAct


    // Uses acts
    //----------------------------------------//
    public void OnUseAct()
    //----------------------------------------//
    {
        GeneralAreaManager.Instance.AddToCurrentActs(-Int32.Parse(actInpField.text));

    } // END OnUseAct


    // New turn, recover acts
    //----------------------------------------//
    public void OnNewTurn()
    //----------------------------------------//
    {
        GeneralAreaManager.Instance.ResetActs();

    } // END OnNewTurn


    // Break, recover composure
    //----------------------------------------//
    public void OnBreak()
    //----------------------------------------//
    {
        GeneralAreaManager.Instance.ResetActs();
        GeneralAreaManager.Instance.ResetCurrentComposure();

    } // END OnBreak


    // Rest, recover composure and defiances
    //----------------------------------------//
    public void OnRest()
    //----------------------------------------//
    {
        GeneralAreaManager.Instance.ResetActs();
        GeneralAreaManager.Instance.ResetCurrentComposure();
        GeneralAreaManager.Instance.ResetDefiances();

    } // END OnRest


    #endregion


} // END RightControlsHandler.cs
