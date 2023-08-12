using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // Item manages a displayed item


    #region VARIABLES


    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private Image bgImg;
    [SerializeField] private Toggle favToggle;
    [SerializeField] private Image favBg;
    [SerializeField] private Image favCheck;

    protected ItemData itemData;


    #endregion


    #region SETUP


    // Sets up item
    //----------------------------------------//
    public void SetupItem(ItemData data)
    //----------------------------------------//
    {
        itemData = data;

        nameText.text = itemData.itemName;

        string type = "";

        // Type
        switch (itemData.itemType)
        {
            case InvItemsManager.ItemType.Weapon:
                type += "Weapon";
                break;
            case InvItemsManager.ItemType.Armor:
                type += "Armor";
                break;
            case InvItemsManager.ItemType.Consumable:
                type += "Consumable";
                break;
            case InvItemsManager.ItemType.Other:
                type += "Other";
                break;
        }

        // Passive
        if (itemData.passive)
        {
            type += " (passive)";
        }

        typeText.text = type;

        // Favorite
        if (favToggle.gameObject.activeSelf)
        {
            if (itemData.favorited)
            {
                favToggle.isOn = true;
            }
            else
            {
                favToggle.isOn = false;
            }
        }

        // Color
        nameText.color = InvItemsManager.itemTextColors[(int)itemData.itemType];
        typeText.color = InvItemsManager.itemTextColors[(int)itemData.itemType];

        favBg.color = InvItemsManager.itemTextColors[(int)itemData.itemType];
        favCheck.color = InvItemsManager.itemTextColors[(int)itemData.itemType];

        bgImg.color = InvItemsManager.itemBgColors[(int)itemData.itemType];

        SetupAdditionalColors();

    } // END SetupArt


    // Sets up additional colors, if necessary (to be overridden)
    //----------------------------------------//
    public virtual void SetupAdditionalColors()
    //----------------------------------------//
    {


    } // END SetupAdditionalColors


    #endregion


    #region FAV TOGGLE


    // OnFavToggle, save art as favorite
    //----------------------------------------//
    public void OnFavToggle()
    //----------------------------------------//
    {
        itemData.favorited = favToggle.isOn;

    } // END OnFavToggle


    #endregion


    #region POINTER ENTER / EXIT


    // OnPointerEnter, show tooltip of art
    //----------------------------------------//
    public void OnPointerEnter(PointerEventData eventData)
    //----------------------------------------//
    {
        TooltipManager.Instance.ShowTooltip(itemData);

    } // END OnPointerEnter


    // OnPointerExit, hide tooltip of art
    //----------------------------------------//
    public void OnPointerExit(PointerEventData eventData)
    //----------------------------------------//
    {
        TooltipManager.Instance.HideTooltip();

    } // END OnPointerExit


    #endregion


} // END Item.cs
