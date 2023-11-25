using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "Level_", menuName = "DATA/Level")]
    public class Level : ScriptableObject
    {
        public enum State
        {
            Closed = -1,
            Opened = 0,
            Passed = 1,
            PassedGood = 2,
            PassedPerfect = 3
        }

        [SerializeField] private LevelRoot _levelPrefab;
        [SerializeField] private Sprite _icon;

        private State _currentState = State.Closed;

        public LevelRoot LevelPrefab => _levelPrefab;
        public Sprite Icon => _icon;

        public State CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }

        public int EarnedStars => Mathf.Max((int)CurrentState, 0);
    }
}