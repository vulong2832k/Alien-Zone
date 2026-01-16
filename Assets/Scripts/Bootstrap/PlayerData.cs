using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public string playerName;

    public int highestUnlockedMap;
    public int currentLevelIndex;
    public float playTime;
    public int totalKill;

    public int level;
    public int exp;
    public int expToNextLevel;

    public List<ItemSaveData> inventoryItems;

    public string weaponEquippedId;
    public string armorEquippedId;
}
