using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegistryItem : Item
{

    // RegistryItem handles an item showing up in the registry


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
        button1.color = InvItemsManager.itemTextColors[(int)itemData.itemType];
        button2.color = InvItemsManager.itemTextColors[(int)itemData.itemType];

        button1Text.color = InvItemsManager.itemBgColors[(int)itemData.itemType];
        button2Text.color = InvItemsManager.itemBgColors[(int)itemData.itemType];

    } // END SetupAdditionalColors


    #endregion


    #region BUTTONS


    // On edit, open art for editing
    //----------------------------------------//
    public void OnEditButton()
    //----------------------------------------//
    {
        InvItemsManager.Instance.BeginEditRegistryItem(itemData);

    } // END OnEditButton


    // On delete, delete art from registry
    //----------------------------------------//
    public void OnDeleteButton()
    //----------------------------------------//
    {
        OnPointerExit(null);
        InvItemsManager.Instance.RemoveRegistryItem(itemData);

    } // END OnDeleteButton


    #endregion


} // END RegistryItem.cs
