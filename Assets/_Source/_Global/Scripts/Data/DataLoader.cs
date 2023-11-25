using System.Collections;
using System.Collections.Generic;

public abstract class DataLoader
{
    private GameData _currentGameData;

    public GameData CurrentGameData => _currentGameData;

    public GameData Load(GameData initialGameData)
    {
        if (HasSavedData())
        {
            _currentGameData = LoadData();
        }
        else
        {
            _currentGameData = initialGameData;
            Save();
        }

        return _currentGameData;
    }

    public abstract void Save();

    protected abstract bool HasSavedData();
    protected abstract GameData LoadData();
}