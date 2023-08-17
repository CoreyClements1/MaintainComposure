using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvItemsManager : MonoBehaviour
{

    // InvItemsManager manages the inventory panel and the items within


    #region ENUMS AND STATIC


    [System.Serializable]
    public enum ItemType
    {
        Weapon,
        Armor,
        Consumable,
        Other
    }

    [System.Serializable]
    public enum DamageDie
    {
        d4,
        d6,
        d8,
        d10,
        d12,
        d20
    }

    public static Color[] itemTextColors = new Color[]
    {
        new Color(241f/255f, 135f/255f, 146f/255f), // Weapon
        new Color(125f/255f, 204f/255f, 224f/255f), // Armor
        new Color(243f/255f, 127f/255f, 157f/255f), // Consumable
        new Color(183f/255f, 183f/255f, 183f/255f), // Other
    };

    public static Color[] itemBgColors = new Color[]
    {
        new Color(49f/255f, 6f/255f, 11f/255f), // Weapon
        new Color(9f/255f, 42f/255f, 56f/255f), // Armor
        new Color(51f/255f, 6f/255f, 26f/255f), // Consumable
        new Color(34f/255f, 34f/255f, 34f/255f), // Other
    };


    #endregion


    #region VARIABLES


    public static InvItemsManager Instance { get; private set; }

    // UI components
    [SerializeField] private TMP_Dropdown invTypeDropdown;
    [SerializeField] private TMP_Dropdown invSortDropdown;
    [SerializeField] private Toggle invPassiveToggle;
    [SerializeField] private Transform invContentParent;
    [SerializeField] private Item invItemPrefab;

    [SerializeField] private TMP_Dropdown registryTypeDropdown;
    [SerializeField] private TMP_Dropdown registrySortDropdown;
    [SerializeField] private Transform registryContentParent;
    [SerializeField] private Item registryItemPrefab;

    [SerializeField] private TMP_Dropdown addItemTypeDropdown;
    [SerializeField] private TMP_Dropdown addItemSortDropdown;
    [SerializeField] private Transform addItemContentParent;
    [SerializeField] private Item addItemPrefab;

    // Lists
    private List<ItemData> registryItemDatasAlpha = new List<ItemData>();
    private List<ItemData> obtainedItemDatasAlpha = new List<ItemData>();
    private List<ItemData> obtainableItemDatasAlpha = new List<ItemData>();

    private List<List<ItemData>> registryItemDatasByType = new List<List<ItemData>>();
    private List<List<ItemData>> obtainedItemDatasByType = new List<List<ItemData>>();
    private List<List<ItemData>> obtainableItemDatasByType = new List<List<ItemData>>();

    // TXTs
    [SerializeField] private TextAsset itemsRegistryTxt;
    [SerializeField] private ItemEditorPanel itemEditorPanel;


    #endregion


    #region SETUP


    // Sets up singleton
    //----------------------------------------//
    public void SetupSingleton()
    //----------------------------------------//
    {
        if (InvItemsManager.Instance == null)
        {
            InvItemsManager.Instance = this;
        }
        else
        {
            if (InvItemsManager.Instance != this)
            {
                Destroy(this);
            }
        }

        LoadRegistry();

    } // END SetupSingleton


    // Loads a character's items
    public void SetupObtainedItems(ItemData[] charObtainedItems)
    {
        int numTypes = Enum.GetNames(typeof(ItemType)).Length;

        // Clear old arts
        foreach (Transform child in invContentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        obtainedItemDatasAlpha.Clear();
        obtainedItemDatasByType.Clear();

        // Create new List List
        for (int i = 0; i < numTypes; i++)
        {
            obtainedItemDatasByType.Add(new List<ItemData>());
        }

        if (charObtainedItems != null)
        {
            // Add all datas to respective type
            foreach (ItemData data in charObtainedItems)
            {
                obtainedItemDatasByType[(int)data.itemType].Add(data);
            }

            obtainedItemDatasAlpha = charObtainedItems.ToList();
            ReorderLists(obtainedItemDatasAlpha, obtainedItemDatasByType);
        }

        // Display all
        invTypeDropdown.value = 0;
        invSortDropdown.value = 0;
        invPassiveToggle.isOn = true;
        OnInvValueChange();

    } // END SetupObtainedItems


    // Sets up items from scratch (empty)
    //----------------------------------------//
    public void SetupItemsFromScratch()
    //----------------------------------------//
    {
        int numTypes = Enum.GetNames(typeof(ItemType)).Length;

        // Clear old arts
        foreach (Transform child in invContentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        obtainedItemDatasAlpha.Clear();
        obtainedItemDatasByType.Clear();

        // Create new List List
        for (int i = 0; i < numTypes; i++)
        {
            obtainedItemDatasByType.Add(new List<ItemData>());
        }

        // Display all
        invTypeDropdown.value = 0;
        invSortDropdown.value = 0;
        invPassiveToggle.isOn = true;
        OnInvValueChange();

    } // END SetupItemsFromScratch


    #endregion


    #region ADDING, REMOVING, SORTING LISTS


    // Adds an item to lists
    //----------------------------------------//
    private void AddItemToLists(ItemData dataToAdd, List<ItemData> alphaList, List<List<ItemData>> byTypeList, bool reSort)
    //----------------------------------------//
    {
        alphaList.Add(dataToAdd);
        byTypeList[(int)dataToAdd.itemType].Add(dataToAdd);

        if (reSort)
        {
            ReorderLists(alphaList, byTypeList);
        }

    } // END AddItemToList


    // Removes an item from lists
    //----------------------------------------//
    private void RemoveItemFromLists(ItemData dataToRemove, List<ItemData> alphaList, List<List<ItemData>> byTypeList, bool reSort)
    //----------------------------------------//
    {
        alphaList.Remove(dataToRemove);
        byTypeList[(int)dataToRemove.itemType].Remove(dataToRemove);

        if (reSort)
        {
            ReorderLists(alphaList, byTypeList);
        }

    } // END RemoveItemFromLists


    // Reorders lists
    //----------------------------------------//
    private void ReorderLists(List<ItemData> alphaList, List<List<ItemData>> byTypeList)
    //----------------------------------------//
    {
        alphaList = alphaList.OrderBy(t => t.itemName).ToList();

        for (int i = 0; i < byTypeList.Count; i++)
        {
            byTypeList[i] = byTypeList[i].OrderBy(t => t.itemName).ToList();
        }

    } // END ReorderLists


    #endregion


    #region DISPLAY


    // Displays items by type
    //----------------------------------------//
    private void DisplayByType(List<List<ItemData>> itemsToDisplay, Transform contentParent, Item prefabToSpawn, bool displayPassive)
    //----------------------------------------//
    {
        foreach (Transform child in contentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Spawn arts and display
        foreach (List<ItemData> itemDataList in itemsToDisplay)
        {
            foreach (ItemData data in itemDataList)
            {
                if (!displayPassive)
                {
                    if (data.passive == false)
                    {
                        Item newItem = GameObject.Instantiate(prefabToSpawn);
                        newItem.SetupItem(data);
                        newItem.transform.SetParent(contentParent);
                    }
                }
                else
                {
                    Item newItem = GameObject.Instantiate(prefabToSpawn);
                    newItem.SetupItem(data);
                    newItem.transform.SetParent(contentParent);
                }
            }
        }

    } // END DisplayInvByType


    // Displays items alphabetically
    //----------------------------------------//
    private void DisplayAlpha(List<ItemData> itemsToDisplay, Transform contentParent, Item prefabToSpawn, bool displayPassive)
    //----------------------------------------//
    {
        foreach (Transform child in contentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (ItemData data in itemsToDisplay)
        {
            if (!displayPassive)
            {
                if (data.passive == false)
                {
                    Item newItem = GameObject.Instantiate(prefabToSpawn);
                    newItem.SetupItem(data);
                    newItem.transform.SetParent(contentParent);
                }
            }
            else
            {
                Item newItem = GameObject.Instantiate(prefabToSpawn);
                newItem.SetupItem(data);
                newItem.transform.SetParent(contentParent);
            }
        }

    } // END DisplayInvAlpha


    // Displays items of a certain type
    //----------------------------------------//
    private void DisplayOfType(List<List<ItemData>> itemsToDisplay, ItemType typeToDisplay, Transform contentParent, Item prefabToSpawn, bool displayPassive)
    //----------------------------------------//
    {
        foreach (Transform child in contentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (ItemData data in itemsToDisplay[(int)typeToDisplay])
        {
            if (!displayPassive)
            {
                if (data.passive == false)
                {
                    Item newItem = GameObject.Instantiate(prefabToSpawn);
                    newItem.SetupItem(data);
                    newItem.transform.SetParent(contentParent);
                }
            }
            else
            {
                Item newItem = GameObject.Instantiate(prefabToSpawn);
                newItem.SetupItem(data);
                newItem.transform.SetParent(contentParent);
            }
        }

    } // END DisplayInvOfType


    #endregion


    #region INVENTORY


    // Gets items as array
    //----------------------------------------//
    public ItemData[] GetItemsAsArr()
    //----------------------------------------//
    {
        return obtainedItemDatasAlpha.ToArray();

    } // END GetItemsAsArr


    // On inventory value change, display inventory again
    //----------------------------------------//
    public void OnInvValueChange()
    //----------------------------------------//
    {
        switch (invTypeDropdown.value)
        {
            case 0:
                if (invSortDropdown.value == 0)
                {
                    DisplayByType(obtainedItemDatasByType, invContentParent, invItemPrefab, invPassiveToggle.isOn);
                }
                else
                {
                    DisplayAlpha(obtainedItemDatasAlpha, invContentParent, invItemPrefab, invPassiveToggle.isOn);
                }
                break;
            case 1:
                // TODO
                break;
            default:
                DisplayOfType(obtainedItemDatasByType, (ItemType)Enum.ToObject(typeof(ItemType), invTypeDropdown.value - 2), invContentParent, invItemPrefab, invPassiveToggle.isOn);
                break;
        }

    } // END OnInvValueChange


    // Adds an item to the inventory
    //----------------------------------------//
    public void AddInvItem(ItemData addedItem)
    //----------------------------------------//
    {
        AddItemToLists(addedItem, obtainedItemDatasAlpha, obtainedItemDatasByType, true);
        OnInvValueChange();

    } // EMD AddInvItem


    // Removes an item from the inventory
    //----------------------------------------//
    public void RemoveInvItem(ItemData addedItem)
    {
        RemoveItemFromLists(addedItem, obtainedItemDatasAlpha, obtainedItemDatasByType, true);
        OnInvValueChange();

    } // END RemoveInvItem


    #endregion


    #region REGISTRY DISPLAY


    // On registry header value change, display accordingly
    //----------------------------------------//
    public void OnRegistryHeaderValueChange()
    //----------------------------------------//
    {
        switch (registryTypeDropdown.value)
        {
            case 0:
                if (registrySortDropdown.value == 0)
                {
                    DisplayByType(registryItemDatasByType, registryContentParent, registryItemPrefab, true);
                }
                else
                {
                    DisplayAlpha(registryItemDatasAlpha, registryContentParent, registryItemPrefab, true);
                }
                break;
            default:
                DisplayOfType(registryItemDatasByType, (ItemType)Enum.ToObject(typeof(ItemType), registryTypeDropdown.value - 1), registryContentParent, registryItemPrefab, true);
                break;
        }

    } // END OnRegistryHeaderValueChange


    // Adds an item to the registry
    //----------------------------------------//
    public void AddRegistryItem(ItemData addedItem)
    //----------------------------------------//
    {
        AddItemToLists(addedItem, registryItemDatasAlpha, registryItemDatasByType, true);
        SaveRegistry();
        LoadRegistry();
        OnRegistryHeaderValueChange();

    } // EMD AddRegistryItem


    // Removes an item from the registry
    //----------------------------------------//
    public void RemoveRegistryItem(ItemData addedItem)
    {
        RemoveItemFromLists(addedItem, registryItemDatasAlpha, registryItemDatasByType, true);
        SaveRegistry();
        LoadRegistry();
        OnRegistryHeaderValueChange();

    } // END RemoveRegistryItem


    // Begins creating a new item in registry
    //----------------------------------------//
    public void BeginCreateNewItem()
    //----------------------------------------//
    {
        itemEditorPanel.gameObject.SetActive(true);
        itemEditorPanel.ShowPanelForNewItem();

    } // END BeginCreateNewItem


    // Ends creating new item
    //----------------------------------------//
    public void EndCreateNewItem(ItemData newItem)
    //----------------------------------------//
    {
        AddRegistryItem(newItem);

    } // END EndCreateNewItem


    // begins editing an item in the registry
    //----------------------------------------//
    public void BeginEditRegistryItem(ItemData itemToEdit)
    //----------------------------------------//
    {
        itemEditorPanel.gameObject.SetActive(true);
        itemEditorPanel.ShowPanel(itemToEdit);

    } // END BeginEditRegistryItem


    // Ends editing registry item
    //----------------------------------------//
    public void EndEditRegistryItem(string nameToReplace, ItemData replacingData)
    //----------------------------------------//
    {
        for (int i = 0; i < registryItemDatasAlpha.Count; i++)
        {
            if (registryItemDatasAlpha[i].itemName == nameToReplace)
            {
                registryItemDatasAlpha[i] = replacingData;
                SaveRegistry();
                LoadRegistry();
                OnRegistryHeaderValueChange();
                return;
            }
        }

        Debug.LogError("Oops died when trying to replace art");

    } // END EndEditRegistryItem


    #endregion


    #region REGISTRY SAVE / LOAD


    // Loads items from registry
    //----------------------------------------//
    private void LoadRegistry()
    //----------------------------------------//
    {
        registryItemDatasAlpha.Clear();
        registryItemDatasByType.Clear();

        // Load saved characters into characterDatas
        ItemData[] loadedItems = JsonHelper.FromJsonArray<ItemData>(itemsRegistryTxt.text);

        if (loadedItems == null)
        {
            registryItemDatasAlpha = new List<ItemData>();
        }
        else
        {
            registryItemDatasAlpha = loadedItems.ToList();
        }

        if (registryItemDatasAlpha != null)
        {
            registryItemDatasAlpha = registryItemDatasAlpha.OrderBy(t => t.itemName).ToList();

            int numTypes = Enum.GetNames(typeof(ItemType)).Length;

            // Create new List List
            for (int i = 0; i < numTypes; i++)
            {
                registryItemDatasByType.Add(new List<ItemData>());
            }

            // Add all datas to respective type
            foreach (ItemData data in registryItemDatasAlpha)
            {
                registryItemDatasByType[(int)data.itemType].Add(data);
            }

            for (int i = 0; i < registryItemDatasByType.Count; i++)
            {
                registryItemDatasByType[i] = registryItemDatasByType[i].OrderBy(t => t.itemName).ToList();
            }
        }

    } // END LoadRegistry


    // Saves item registry
    //----------------------------------------//
    private void SaveRegistry()
    //----------------------------------------//
    {
        // Save as JSON
        FileHelper.SaveAsJsonArray<ItemData>("ItemRegistry", registryItemDatasAlpha.ToArray(), false);

    } // END SaveRegistry


    // Consolidates items from update txt into registry
    //----------------------------------------//
    private void ConsolidateUpdateRegistry()
    //----------------------------------------//
    {
        ItemData[] updateRegistryDatas = JsonHelper.FromJsonArray<ItemData>(itemsRegistryTxt.text);

        foreach (ItemData data in updateRegistryDatas)
        {
            bool inRegistry = false;
            for (int i = 0; i < registryItemDatasAlpha.Count; i++)
            {
                if (data.itemName == registryItemDatasAlpha[i].itemName)
                {
                    registryItemDatasAlpha[i] = data;
                    inRegistry = true;
                    break;
                }
            }

            if (!inRegistry)
            {
                registryItemDatasAlpha.Add(data);
            }
        }

        SaveRegistry();

    } // END ConsolidateUpdateRegistry


    #endregion


    #region ADD ITEM DISPLAY


    // Obtains item
    //----------------------------------------//
    public void ObtainItem(ItemData itemData)
    //----------------------------------------//
    {
        AddInvItem(itemData);

        if (addItemContentParent.gameObject.activeSelf)
        {
            DisplayObtainable();
        }

    } // END ObtainItem


    // Unobtains item
    //----------------------------------------//
    public void UnobtainItem(ItemData itemData)
    //----------------------------------------//
    {
        RemoveInvItem(itemData);

        if (addItemContentParent.gameObject.activeSelf)
        {
            DisplayObtainable();
        }

    } // END UnobtainItem


    // Displays obtainable items
    //----------------------------------------//
    public void DisplayObtainable()
    //----------------------------------------//
    {
        switch (addItemTypeDropdown.value)
        {
            case 0:
                if (addItemSortDropdown.value == 0)
                {
                    DisplayByType(registryItemDatasByType, addItemContentParent, addItemPrefab, true);
                }
                else
                {
                    DisplayAlpha(registryItemDatasAlpha, addItemContentParent, addItemPrefab, true);
                }
                break;
            case 1:
                // TODO
                break;
            default:
                DisplayOfType(registryItemDatasByType, (ItemType)Enum.ToObject(typeof(ItemType), addItemTypeDropdown.value - 1), addItemContentParent, addItemPrefab, true);
                break;
        }

    } // END DisplayObtainable


    #endregion


} // END InvItemsManager.cs


[System.Serializable]
public class ItemData
{

    // ItemData contains data for an item


    #region VARIABLES


    public string itemName = "";
    public InvItemsManager.ItemType itemType = InvItemsManager.ItemType.Other;
    public string itemDescription = "";
    public string itemTags = "";
    public bool passive = false;
    public bool favorited = false;

    // Weapon
    public int actCost = 1;
    public int range = 2;
    public int correspondingApp = 0;
    public InvItemsManager.DamageDie damageDie = InvItemsManager.DamageDie.d4;
    public int additionalHitMod = 0;
    public int additionalDamageMod = 0;

    // Armor
    public int thresholdBonus = 0;
    public int maxCompBonus = 0;
    public int speedBonus = 0;


    #endregion


    #region SETUP


    // Sets up item from another item
    //----------------------------------------//
    public void SetupItem(ItemData itemToCopy)
    //----------------------------------------//
    {
        itemName = itemToCopy.itemName;
        itemType = itemToCopy.itemType;
        itemDescription = itemToCopy.itemDescription;
        itemTags = itemToCopy.itemTags;
        passive = itemToCopy.passive;
        favorited = itemToCopy.favorited;

        actCost = itemToCopy.actCost;
        range = itemToCopy.range;
        correspondingApp = itemToCopy.correspondingApp;
        damageDie = itemToCopy.damageDie;
        additionalHitMod= itemToCopy.additionalHitMod;
        additionalDamageMod= itemToCopy.additionalDamageMod;
        thresholdBonus = itemToCopy.thresholdBonus;
        maxCompBonus = itemToCopy.maxCompBonus;
        speedBonus = itemToCopy.speedBonus;

    }// END SetupItem


    // Sets up item as weapon
    //----------------------------------------//
    public void SetupItem(string _itemName, InvItemsManager.ItemType _itemType, string _itemDescription, string _itemTags, bool _passive, bool _favorited)
    //----------------------------------------//
    {
        itemName = _itemName;
        itemType = _itemType;
        itemDescription= _itemDescription;
        itemTags = _itemTags;
        passive = _passive;
        favorited = _favorited;

    } // END SetupItemAsWeapon


    // Sets up item as weapon
    //----------------------------------------//
    public void SetupAsWeapon(string _itemName, InvItemsManager.ItemType _itemType, string _itemDescription, string _itemTags, bool _passive, bool _favorited, int _actCost, int _range, int _correspondingApp, InvItemsManager.DamageDie _damageDie, int _additionalHitMod, int _additionalDamageMod)
    //----------------------------------------//
    {
        SetupItem(_itemName, _itemType, _itemDescription, _itemTags, _passive, _favorited);

        actCost = _actCost;
        range = _range;
        correspondingApp = _correspondingApp;
        damageDie = _damageDie;
        additionalHitMod = _additionalHitMod;
        additionalDamageMod = _additionalDamageMod;

    } // END SetupItemAsWeapon


    // Sets up item as armor
    //----------------------------------------//
    public void SetupAsArmor(string _itemName, InvItemsManager.ItemType _itemType, string _itemDescription, string _itemTags, bool _passive, bool _favorited, int _thresholdBonus, int _maxCompBonus, int _speedBonus)
    //----------------------------------------//
    {
        SetupItem(_itemName, _itemType, _itemDescription, _itemTags, _passive, _favorited);

        thresholdBonus = _thresholdBonus;
        maxCompBonus = _maxCompBonus;
        speedBonus = _speedBonus;

    } // END SetupAsArmor


    #endregion


} // END ItemData
