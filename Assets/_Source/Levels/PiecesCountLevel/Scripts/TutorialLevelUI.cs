using UnityEngine;
using UnityEngine.UI;

namespace Levels
{
    public class TutorialLevelUI : UIGroup
    {
        [SerializeField] private Button _endTutorialButton;
        [SerializeField] private RectTransform _pressTutorialPoint;

        public Button EndTutorialButton => _endTutorialButton;
        public RectTransform PressTutorialPoint => _pressTutorialPoint;
    }
}