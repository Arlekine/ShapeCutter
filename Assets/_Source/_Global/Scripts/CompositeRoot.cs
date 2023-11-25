using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Levels;
using UISystem;
using UnityEngine;
using UnityEngine.UI;

public class CompositeRoot : MonoBehaviour, IUISpawner
{
    private const int MaxStarsForLevel = 3;

    [SerializeField] private UI _ui;
    [SerializeField] private List<Stage> _stages = new List<Stage>();

    [Header("UI-Header")]
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _homeButton;
    
    [Header("UI-Level")]
    [SerializeField] private WinPanel _winPanelPrefab;
    [SerializeField] private LoosePanel _loosePanel;

    [Header("UI-Main")] 
    [SerializeField] private LevelsMenu _levelMenuPrefab;
    [SerializeField] private SettingsView _settingsView;

    [Header("UI-MainMenu")]
    [SerializeField] private UIFadeShowAnimation _mainMenu;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _levelMenuButton;
    [SerializeField] private Button _cuttingModeButton;

    [Header("Sounds")]
    [SerializeField] private SoundPlayer _backMusic;
    [SerializeField] private SoundPlayer _winSound;
    [SerializeField] private SoundPlayer _loseSound;

    private LevelRoot _currentLevelRoot;
    private Level _currentLevel;

    private LevelsMenu _levelMenu;

    private DataLoader _dataLoader;
    private Settings _settings;
    private AudioSource _backMusicSource;

    private float _defaultMusicVolume;
    private float _silentMusicVolume = 0.025f;

    public void Start()
    {
        _defaultMusicVolume = _backMusic.Volume;
        _backMusicSource = _backMusic.Play();
        
        var defaultGameData = new GameData();

        for (int i = 0; i < _stages.Count; i++)
        {
            var stageData = _stages[i].GetLevelsData();
            if (i == 0)
            {
                if (stageData.LevelsData[0] == Level.State.Closed)
                    stageData.LevelsData[0] = Level.State.Opened;
            }

            defaultGameData.StagesDatas.Add(stageData);
        }

        _dataLoader = new LocalDataLoader();
        var data = _dataLoader.Load(defaultGameData);

        _settings = new Settings(data.Settings);
        _settingsView.Init(_settings);

        for (int i = 0; i < _stages.Count; i++)
        {
            if (data.StagesDatas.Count <= i)
                data.StagesDatas.Add(_stages[i].GetLevelsData());
            else
                _stages[i].SetData(data.StagesDatas[i]);
        }

        _loosePanel.OnRestart += RestartCurrentLevel;
        _winPanelPrefab.OnRestart += RestartCurrentLevel;

        _winPanelPrefab.OnNext += OnNextLevel;

        _restartButton.onClick.AddListener(RestartCurrentLevel);
        _homeButton.onClick.AddListener(OpenMainMenu);

        _settings.SoundChanged += (state) => _dataLoader.Save();
        _settings.HapticChanged += (state) => _dataLoader.Save();

        _continueButton.onClick.AddListener(Continue);
        _levelMenuButton.onClick.AddListener(OpenLevelMenu);
        //_cuttingModeButton.onClick.AddListener(Continue);
    }

    private void Continue()
    {
        var stageIndex = _dataLoader.CurrentGameData.CurrentStage;
        var levelIndex = _dataLoader.CurrentGameData.CurrentLevel;
        
        _mainMenu.Hide();
        OpenLevel(_stages[stageIndex].Levels[levelIndex]);
    }

    private void OpenFreeCuttingLevel()
    {

    }

    private void OpenMainMenu()
    {
        _mainMenu.Show();
        DestroyCurrentLevel();
    }

    private (int earned, int max) CalculateStars()
    {
        var earnedStars = 0;
        var maxStars = 0;

        foreach (var stage in _stages)
        {
            foreach (var level in stage.Levels)
            {
                earnedStars += level.EarnedStars;
                maxStars += MaxStarsForLevel;
            }
        }

        foreach (var stage in _stages)
        {
            stage.UpdateStars(earnedStars);
        }

        return (earnedStars, maxStars);
    }

    [EditorButton]
    private void OpenLevelMenu()
    {
        var stars = CalculateStars();
        
        _mainMenu.Hide();

        _levelMenu = _ui.CreateUIElement(_levelMenuPrefab, UIType.MainMenu, this);
        _levelMenu.Open(_stages, stars.earned, stars.max);
        _levelMenu.Closed += OnLevelMenuClosed;
        _levelMenu.LevelSelected += OpenLevelFromMenu;
    }

    private void OnLevelMenuClosed()
    {
        _mainMenu.Show();
        CloseLevelMenu();
    }

    private void CloseLevelMenu()
    {
        _levelMenu.Closed -= OnLevelMenuClosed;
        _levelMenu.LevelSelected -= OpenLevelFromMenu;

        _levelMenu.Close();
        _ui.DestroyElement(_levelMenu, 0.3f, this);
    }

    private void RestartCurrentLevel()
    {
        CreateLevel(_currentLevel.LevelPrefab);
    }

    private void OnNextLevel()
    {
        var next = GetNextLevel(_currentLevel);

        if (next.level != null)
            OpenLevel(next.level);

        //else - show "you beat the game" menu
    }

    private (Level level, Stage stage) GetNextLevel(Level currentLevel)
    {
        var levelsStage = GetLevelsStageIndex(currentLevel);

        var levelIndex = _stages[levelsStage].Levels.IndexOf(currentLevel);

        if (levelIndex >= _stages[levelsStage].Levels.Count - 1)
        {
            if (levelsStage >= _stages.Count)
                return (null, null);
            
            return (_stages[levelsStage + 1].Levels[0], _stages[levelsStage + 1]);
        }
        
        levelIndex++;
        return (_stages[levelsStage].Levels[levelIndex], _stages[levelsStage]);
    }

    private void OpenLevelFromMenu(Level level)
    {
        CloseLevelMenu();
        OpenLevel(level);
    }

    private void OpenLevel(Level level)
    {
        var stageIndex = GetLevelsStageIndex(level);

        _dataLoader.CurrentGameData.CurrentStage = stageIndex;
        _dataLoader.CurrentGameData.CurrentLevel = _stages[stageIndex].Levels.IndexOf(level);
        _dataLoader.Save();

        _currentLevel = level;
        CreateLevel(level.LevelPrefab);
    }

    private void DestroyCurrentLevel()
    {
        if (_currentLevelRoot != null)
        {
            _ui.ClearComponentsForSpawner(_currentLevelRoot);
            Destroy(_currentLevelRoot.gameObject);

            _loosePanel.Hide();
            _winPanelPrefab.Hide();
        }
    }

    private int GetLevelsStageIndex(Level level)
    {
        int stage = -1;

        for (int i = 0; i < _stages.Count; i++)
        {
            if (_stages[i].Levels.Contains(level))
                stage = i;
        }

        if (stage == -1)
            throw new ArgumentException($"{level.name} is level without stage");

        return stage;
    }

    private void CreateLevel(LevelRoot levelRoot)
    {
        _backMusicSource.DOFade(_defaultMusicVolume, 0.3f);
        DestroyCurrentLevel();

        _currentLevelRoot = Instantiate(levelRoot);
        _currentLevelRoot.Init(_ui);
        _currentLevelRoot.Completed += OnLevelCompleted;
    }

    private void SaveLevels(Stage currentStage, Level currentLevel)
    {
        _dataLoader.CurrentGameData.StagesDatas = new List<StageData>();
        
        for (int i = 0; i < _stages.Count; i++)
        {
            _dataLoader.CurrentGameData.StagesDatas.Add(_stages[i].GetLevelsData());
        }

        _dataLoader.CurrentGameData.CurrentStage = _stages.IndexOf(currentStage);
        _dataLoader.CurrentGameData.CurrentLevel = currentStage.Levels.IndexOf(currentLevel);

        _dataLoader.Save();
    }

    private void OnLevelCompleted(LevelResult result)
    {
        _currentLevel.CurrentState = (Level.State)Mathf.Max((int)result, (int)_currentLevel.CurrentState);

        var next = GetNextLevel(_currentLevel);

        if (result != LevelResult.Loose && next.level != null && next.level.CurrentState == Level.State.Closed)
            next.level.CurrentState = Level.State.Opened;
        
        if (result != LevelResult.Loose)
            SaveLevels(next.stage, next.level);

        _backMusicSource.DOFade(_silentMusicVolume, 0.3f);
        if (result == LevelResult.Loose)
        {
            _loseSound.Play();
            _loosePanel.Show();
        }
        else
        {
            _winSound.Play();
            _winPanelPrefab.Show(result);
        }
    }
}
