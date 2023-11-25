using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "Stage_", menuName = "DATA/Stage")]
    public class Stage : ScriptableObject
    {
        [SerializeField] private List<Level> _levels = new List<Level>();
        [SerializeField] private int _starsToOpenStage;
        
        private bool _isOpened;

        public List<Level> Levels => _levels;
        public bool IsOpened => _isOpened;
        public int StarsToOpenStage => _starsToOpenStage;

        public StageData GetLevelsData()
        {
            var data = new StageData();
            foreach (var level in Levels)
            {
                data.LevelsData.Add(level.CurrentState);
            }

            return data;
        }

        public bool SetData(StageData data)
        {
            var count = data.LevelsData.Count >= Levels.Count ? data.LevelsData.Count : Levels.Count;

            for (int i = 0; i < count; i++)
            {
                Levels[i].CurrentState = data.LevelsData[i];
            }

            return data.LevelsData.Count == Levels.Count;
        }

        public int GetStarsSum() => Levels.Sum(level => level.EarnedStars);

        public void UpdateStars(int stars)
        {
            _isOpened = stars >= _starsToOpenStage;
        }
    }
}