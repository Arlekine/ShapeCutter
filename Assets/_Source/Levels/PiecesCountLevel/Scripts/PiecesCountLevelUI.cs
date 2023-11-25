using UnityEngine;

namespace Levels
{
    public class PiecesCountLevelUI : UIGroup
    {
        [SerializeField] private LevelResultProgress _levelResultProgress;
        [SerializeField] private CounterView _counter;

        public LevelResultProgress LevelResultProgress => _levelResultProgress;
        public CounterView Counter => _counter;
    }
}