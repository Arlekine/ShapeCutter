using UISystem;
using UnityEngine;

namespace Levels
{
    public class TimedCuttingLevelRoot : PiecesCountLevelRoot
    {
        [Space]
        [SerializeField] private Timer _timer;
        [SerializeField] private WarningTimerView _timerView;
        [SerializeField] private int _timeForLevel;
        [SerializeField] private int _warningStart;

        public override void Init(UI ui, TutorialHolder tutorial)
        {
            base.Init(ui, tutorial);

            var timerView = ui.CreateUIElement(_timerView, UIType.Level, this);

            _timer.StartTimer(_timeForLevel, OnTimerExpired);
            _timer.Pause();
            timerView.Init(_timer, _warningStart);
        }

        protected override void OnLevelStart()
        {
            base.OnLevelStart();
            _timer.Continue();
        }

        private void OnTimerExpired()
        {
            _lineInput.Disable();
            _linecastCutter.enabled = false;
            _lineView.enabled = false;

            Completed?.Invoke(LevelResult.Loose);
        }
    }
}