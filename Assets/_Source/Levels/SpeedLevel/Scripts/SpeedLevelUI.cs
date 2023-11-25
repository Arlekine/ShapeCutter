using UnityEngine;
using UnityEngine.UI;

namespace Levels
{
    public class SpeedLevelUI : UIGroup
    {
        [SerializeField] private WarningTimerView _timerView;
        [SerializeField] private ProgressionView _progressionView;
        [SerializeField] private PercentProgressView _percentProgressShower;
        [SerializeField] private LevelResultProgress _progress;
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private UiShowingAnimation _pauseScreen;

        public WarningTimerView TimerView => _timerView;
        public ProgressionView ProgressionView => _progressionView;
        public PercentProgressView FinalPercentShower => _percentProgressShower;
        public LevelResultProgress Progress => _progress;
        public Button ResetButton => _resetButton;
        public Button PauseButton => _pauseButton;
        public Button ResumeButton => _resumeButton;
        public UiShowingAnimation PauseScreen => _pauseScreen;
    }
}