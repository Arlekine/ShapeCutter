using System;
using System.Collections;
using System.Collections.Generic;
using Levels;
using UnityEngine;

[Serializable]
public class GameData
{
    public int CurrentLevel = 0;
    public int CurrentStage = 0;

    public TutorialProgressData TutorialProgress = new TutorialProgressData();
    public SettingsData Settings = new SettingsData();
    public List<StageData> StagesDatas = new List<StageData>();
}