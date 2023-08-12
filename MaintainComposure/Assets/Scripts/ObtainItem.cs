using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObtainItem : Item
{

    // ObtainItem handles the obtainable arts in the inventory obtain panel


    #region VARIABLES


    [SerializeField] private Image buttonImg;
    [SerializeField] private TMP_Text buttonText;


    #endregion


    #region COLORS


    // Sets up additional colors
    //----------------------------------------//
    public override void SetupAdditionalColors()
    //----------------------------------------//
    {
        buttonImg.color = InvItemsManager.itemTextColors[(int)itemData.itemType];

        buttonText.color = InvItemsManager.itemBgColors[(int)itemData.itemType];

    } // END SetupAdditionalColors


    #endregion


    #region ON OBTAIN


    // Obtain item through item manager
    //----------------------------------------//
    public void OnObtainButton()
    //----------------------------------------//
    {
        InvItemsManager.Instance.ObtainItem(itemData);

    } // END OnObtainButton


    #endregion


} // END ObtainItem.cs
