using UnityEngine;

public class LocalDataLoader : DataLoader
{
    private const string DataPathName = "GameData";

    protected override bool HasSavedData() => PlayerPrefs.HasKey(DataPathName);
    protected override GameData LoadData() => JsonUtility.FromJson<GameData>(PlayerPrefs.GetString(DataPathName));

    public override void Save() => PlayerPrefs.SetString(DataPathName, JsonUtility.ToJson(CurrentGameData));
}