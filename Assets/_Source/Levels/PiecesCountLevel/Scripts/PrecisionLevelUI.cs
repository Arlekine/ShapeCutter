using UnityEngine;
using UnityEngine.UI;

namespace Levels
{
    public class PrecisionLevelUI : UIGroup
    {
        [SerializeField] private FinalPercentShower _finalPercentShower;
        [SerializeField] private LevelResultProgress _progress;
        [SerializeField] private UiShowingAnimation _progressShowingAnimation;
        [SerializeField] private Button _checkButton;
        [SerializeField] private RectTransform _tutorialPoint;

        public FinalPercentShower FinalPercentShower => _finalPercentShower;
        public LevelResultProgress Progress => _progress;
        public UiShowingAnimation ProgressShowingAnimation => _progressShowingAnimation;
        public Button CheckButton => _checkButton;
        public RectTransform TutorialPoint => _tutorialPoint;
    }
}