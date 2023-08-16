using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RegistryArt : Art
{

    // RegistryArt is an art with additional functionality for registry control


    #region VARIABLES


    [SerializeField] private Image button1;
    [SerializeField] private TMP_Text button1Text;
    [SerializeField] private Image button2;
    [SerializeField] private TMP_Text button2Text;


    #endregion


    #region COLORS


    // Sets up additional colors
    //----------------------------------------//
    public override void SetupAdditionalColors()
    //----------------------------------------//
    {
        button1.color = ArtsManager.artTextColors[(int)artData.artType];
        button2.color = ArtsManager.artTextColors[(int)artData.artType];

        button1Text.color = ArtsManager.artBgColors[(int)artData.artType];
        button2Text.color = ArtsManager.artBgColors[(int)artData.artType];

    } // END SetupAdditionalColors


    #endregion


    #region BUTTONS


    // On edit, open art for editing
    //----------------------------------------//
    public void OnEditButton()
    //----------------------------------------//
    {
        ArtsManager.Instance.BeginEditingRegistryArt(artData);

    } // END OnEditButton


    // On delete, delete art from registry
    //----------------------------------------//
    public void OnDeleteButton()
    //----------------------------------------//
    {
        OnPointerExit(null);
        ArtsManager.Instance.DeleteArtFromRegistry(artData);

    } // END OnDeleteButton


    #endregion


} // END RegistryArt.cs
