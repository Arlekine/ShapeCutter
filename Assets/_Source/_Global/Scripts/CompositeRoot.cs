using System.Collections;
using System.Collections.Generic;
using Levels;
using UISystem;
using UnityEngine;

public class CompositeRoot : MonoBehaviour, IUISpawner
{
    [SerializeField] private Level _levelToTest;
    [SerializeField] private UI _ui;

    [Header("UI")]

    private Level _currentLevel;

    public void Start()
    {
        _currentLevel = Instantiate(_levelToTest);
        _currentLevel.Init(_ui);
        _currentLevel.Completed += OnLevelCompleted;
    }

    private void OnLevelCompleted(LevelResult result)
    {
        print(result);
    }
}
