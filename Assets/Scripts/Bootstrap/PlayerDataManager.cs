using UnityEngine;
using System.Collections.Generic;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    private const string SLOT_PREFIX = "PLAYER_DATA_";
    private const int MAX_SLOT = 5;

    public string CurrentSlotId { get; private set; }
    public PlayerData CurrentData { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }
    public string GetSlotId(int index)
    {
        return $"slot_{index}";
    }
    public List<string> SlotIds
    {
        get
        {
            List<string> slots = new();
            for (int i = 0; i < MAX_SLOT; i++)
            {
                string id = GetSlotId(i);
                if (PlayerPrefs.HasKey(SLOT_PREFIX + id))
                    slots.Add(id);
            }
            return slots;
        }
    }

    public PlayerData LoadSlot(string slotId)
    {
        string key = SLOT_PREFIX + slotId;
        if (!PlayerPrefs.HasKey(key)) return null;

        string json = PlayerPrefs.GetString(key);
        return JsonUtility.FromJson<PlayerData>(json);
    }
    public bool SetCurrentSlot(string slotId)
    {
        if (!PlayerPrefs.HasKey(SLOT_PREFIX + slotId))
            return false;

        CurrentSlotId = slotId;
        return LoadCurrent();
    }

    public void CreateNewPlayer(string slotId, string playerName)
    {
        string key = SLOT_PREFIX + slotId;
        if (PlayerPrefs.HasKey(key)) return;

        CurrentSlotId = slotId;
        CurrentData = new PlayerData
        {
            playerName = playerName,

            highestUnlockedMap = 0,
            currentLevelIndex = 0,

            level = 1,
            exp = 0,
            expToNextLevel = 100,

            totalKill = 0,
            playTime = 0,

            inventoryItems = new List<ItemSaveData>(),

            weaponEquippedId = "",
            armorEquippedId = ""
        };

        Save();
    }
    public void SetCurrentLevel(int levelIndex)
    {
        if (CurrentData == null) return;

        CurrentData.currentLevelIndex = levelIndex;
        Save();
    }
    public void Save()
    {
        if (string.IsNullOrEmpty(CurrentSlotId) || CurrentData == null)
            return;

        string json = JsonUtility.ToJson(CurrentData);
        PlayerPrefs.SetString(SLOT_PREFIX + CurrentSlotId, json);
        PlayerPrefs.Save();
    }
    public void SaveInventory(List<InventorySlot> slots)
    {
        if (CurrentData == null) return;

        if (CurrentData.inventoryItems == null)
            CurrentData.inventoryItems = new List<ItemSaveData>();

        CurrentData.inventoryItems.Clear();

        foreach (var slot in slots)
        {
            if (slot.IsEmpty) continue;

            CurrentData.inventoryItems.Add(new ItemSaveData
            {
                itemId = slot.itemName.itemId,
                amount = slot.amount
            });
        }

        Save();
    }

    private void ValidateData()
    {
        if (CurrentData.inventoryItems == null)
            CurrentData.inventoryItems = new List<ItemSaveData>();

        if (CurrentData.level <= 0)
            CurrentData.level = 1;

        if (CurrentData.expToNextLevel <= 0)
            CurrentData.expToNextLevel = 100;

        if (CurrentData.weaponEquippedId == null)
            CurrentData.weaponEquippedId = "";

        if (CurrentData.armorEquippedId == null)
            CurrentData.armorEquippedId = "";
    }

    public List<ItemSaveData> GetSavedInventory()
    {
        if (CurrentData == null) return new List<ItemSaveData>();
        if (CurrentData.inventoryItems == null)
            CurrentData.inventoryItems = new List<ItemSaveData>();

        return CurrentData.inventoryItems;
    }
    private bool LoadCurrent()
    {
        string key = SLOT_PREFIX + CurrentSlotId;
        if (!PlayerPrefs.HasKey(key)) return false;

        string json = PlayerPrefs.GetString(key);
        CurrentData = JsonUtility.FromJson<PlayerData>(json);

        ValidateData();
        return true;
    }
    public string GetFirstEmptySlot()
    {
        for (int i = 0; i < MAX_SLOT; i++)
        {
            string slotId = GetSlotId(i);
            if (!PlayerPrefs.HasKey(SLOT_PREFIX + slotId))
            {
                return slotId;
            }
        }
        return null;
    }
    public void ClearAllData()
    {
        for (int i = 0; i < MAX_SLOT; i++)
        {
            string slotID = GetSlotId(i);
            string key = SLOT_PREFIX + slotID;

            if(PlayerPrefs.HasKey(key))
                PlayerPrefs.DeleteKey(key);
        }

        PlayerPrefs.Save();

        CurrentSlotId = null;
        CurrentData = null;
        }
}
