using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ArtsManager;

public class ItemEditorPanel : MonoBehaviour
{

    // ItemEditorPanel handles the editing of an item


    #region VARIABLES


    private ItemData editingItemData;
    private ItemData oldItemData;
    [SerializeField] private CanvasGroup canvasGroup;
    private bool creatingNew = false;
    private string oldItemName;

    [SerializeField] private TMP_InputField nameInpField;
    [SerializeField] private TMP_Dropdown typeDropdown;
    [SerializeField] private Toggle passiveToggle;
    [SerializeField] private TMP_InputField tagsField;
    [SerializeField] private TMP_InputField itemDescriptionField;

    [SerializeField] private TMP_InputField weaponHitBonusField;
    [SerializeField] private TMP_InputField weaponDmgBonusField;
    [SerializeField] private TMP_InputField actCostField;
    [SerializeField] private TMP_InputField rangeField;
    [SerializeField] private TMP_Dropdown corrAppDropdown;
    [SerializeField] private TMP_Dropdown damageDieDropdown;

    [SerializeField] private TMP_InputField armorThreshBonusField;
    [SerializeField] private TMP_InputField armorCompBonusField;
    [SerializeField] private TMP_InputField armorSpeedBonusField;

    [SerializeField] private GameObject weaponsAdditionalArea;
    [SerializeField] private GameObject armorAdditionalArea;


    #endregion


    #region SHOW / HIDE


    // Shows the panel and enables editing
    //----------------------------------------//
    public void ShowPanel(ItemData dataToEdit)
    //----------------------------------------//
    {
        oldItemName = dataToEdit.itemName;
        creatingNew = false;

        oldItemData = dataToEdit;
        editingItemData = new ItemData();
        editingItemData.SetupItem(dataToEdit);

        SetUiToEditingItemData();

        canvasGroup.alpha = 0f;
        canvasGroup.LeanAlpha(1f, .3f);

    } // END ShowPanel


    // Shows the panel and enables creating new
    //----------------------------------------//
    public void ShowPanelForNewItem()
    //----------------------------------------//
    {
        creatingNew = true;

        editingItemData = new ItemData();

        SetUiToEditingItemData();

        canvasGroup.alpha = 0f;
        canvasGroup.LeanAlpha(1f, .3f);

    } // END ShowPanelForNewArt


    // Hides the panel
    //----------------------------------------//
    public void HidePanel()
    //----------------------------------------//
    {
        StartCoroutine(HidePanelCoroutine());

    } // END HidePanel


    // Hides the panel
    //----------------------------------------//
    private IEnumerator HidePanelCoroutine()
    //----------------------------------------//
    {
        canvasGroup.LeanAlpha(0f, .3f);

        yield return new WaitForSeconds(.3f);

        gameObject.SetActive(false);

    } // END HidePanelCoroutine


    #endregion


    #region CONFIRM / CANCEL


    // OnConfirm, end editing
    //----------------------------------------//
    public void OnConfirm()
    //----------------------------------------//
    {
        GetEditingItemDataFromUi();

        if (creatingNew)
        {
            InvItemsManager.Instance.EndCreateNewItem(editingItemData);
        }
        else
        {
            oldItemData = editingItemData;
            InvItemsManager.Instance.EndEditRegistryItem(oldItemName, editingItemData);
        }
        
        HidePanel();

    } // END OnConfirm


    // OnCancel, cancel editing
    //----------------------------------------//
    public void OnCancel()
    //----------------------------------------//
    {
        HidePanel();

    } // END OnCancel


    #endregion


    #region GET / SET UI


    // Sets the editing art data to match the UI
    //----------------------------------------//
    private void GetEditingItemDataFromUi()
    //----------------------------------------//
    {
        editingItemData.itemName = nameInpField.text;
        editingItemData.itemType = (InvItemsManager.ItemType)Enum.ToObject(typeof(InvItemsManager.ItemType), typeDropdown.value);
        editingItemData.passive = passiveToggle.isOn;
        editingItemData.itemTags = tagsField.text;
        editingItemData.itemDescription = itemDescriptionField.text;

        editingItemData.correspondingApp = corrAppDropdown.value;
        editingItemData.damageDie = (InvItemsManager.DamageDie)Enum.ToObject(typeof(InvItemsManager.DamageDie),damageDieDropdown.value);

        int t;
        if (Int32.TryParse(actCostField.text, out t))
        {
            editingItemData.actCost = t;
        }

        if (Int32.TryParse(rangeField.text, out t))
        {
            editingItemData.range = t;
        }

        if (Int32.TryParse(weaponHitBonusField.text, out t))
        {
            editingItemData.additionalHitMod = t;
        }

        if (Int32.TryParse(weaponDmgBonusField.text, out t))
        {
            editingItemData.additionalDamageMod = t;
        }

        if (Int32.TryParse(armorThreshBonusField.text, out t))
        {
            editingItemData.thresholdBonus = t;
        }

        if (Int32.TryParse(armorCompBonusField.text, out t))
        {
            editingItemData.maxCompBonus = t;
        }

        if (Int32.TryParse(armorSpeedBonusField.text, out t))
        {
            editingItemData.speedBonus = t;
        }

    } // END GetEditingArtDataFromUi


    // Sets the UI to match the art data we are editing
    //----------------------------------------//
    private void SetUiToEditingItemData()
    //----------------------------------------//
    {
        nameInpField.text = editingItemData.itemName;
        itemDescriptionField.text = editingItemData.itemDescription;
        passiveToggle.isOn = editingItemData.passive;
        tagsField.text = editingItemData.itemTags;
        typeDropdown.value = (int)editingItemData.itemType;
        corrAppDropdown.value = editingItemData.correspondingApp;
        damageDieDropdown.value = (int)editingItemData.damageDie;
        actCostField.text = editingItemData.actCost.ToString();
        rangeField.text = editingItemData.range.ToString();
        weaponHitBonusField.text = editingItemData.additionalHitMod.ToString();
        weaponDmgBonusField.text = editingItemData.additionalDamageMod.ToString();
        armorThreshBonusField.text = editingItemData.thresholdBonus.ToString();
        armorCompBonusField.text = editingItemData.maxCompBonus.ToString();
        armorSpeedBonusField.text = editingItemData.speedBonus.ToString();

    } // END SetUiToEditingArtData


    #endregion


    #region ON TYPE CHANGE


    // On type dropdown change, display additional type fields
    //----------------------------------------//
    public void OnTypeDropdownChange()
    //----------------------------------------//
    {
        if (typeDropdown.value == (int)InvItemsManager.ItemType.Weapon)
        {
            weaponsAdditionalArea.SetActive(true);
            armorAdditionalArea.SetActive(false);
        }
        else if (typeDropdown.value == (int)InvItemsManager.ItemType.Armor)
        {
            weaponsAdditionalArea.SetActive(false);
            armorAdditionalArea.SetActive(true);
        }
        else
        {
            weaponsAdditionalArea.SetActive(false);
            armorAdditionalArea.SetActive(false);
        }

    } // END OnTypeDropdownChange


    #endregion


} // END ItemEditorPanel.cs
