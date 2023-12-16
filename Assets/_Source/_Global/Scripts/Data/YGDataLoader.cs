using YG;

public class YGDataLoader : DataLoader
{
    private const string DataPathName = "GameData";

    protected override bool HasSavedData() => YandexGame.savesData.GameData != null;
    protected override GameData LoadData() => YandexGame.savesData.GameData;

    public override void Save()
    {
        if (CurrentGameData != null && YandexGame.savesData.GameData != CurrentGameData)
            YandexGame.savesData.GameData = CurrentGameData;

        YandexGame.SaveProgress();
    }
}