using UnityEngine;

public class TutorialHolder : MonoBehaviour
{
    [SerializeField] private PressTutorialAnimation _pressTutorialAnimation;
    [SerializeField] private CuttingTutorialAnimation _cuttingTutorialAnimation;
    
    private TutorialProgressData _data;

    public TutorialProgressData Data => _data;

    public void SetData(TutorialProgressData data)
    {
        _data = data;
    }

    public ITutorialAnimation CreatePressTutorial(RectTransform pressParent, Vector2 pressPosition)
    {
        var newPressAnimation = Instantiate(_pressTutorialAnimation, pressParent);
        newPressAnimation.SetPoint(pressParent, pressPosition);
        return newPressAnimation;
    }

    public ITutorialAnimation CreateCutTutorial(Vector3 lineStart, Vector3 lineEnd)
    {
        var newPressAnimation = Instantiate(_cuttingTutorialAnimation);
        newPressAnimation.Init(lineStart, lineEnd);
        return newPressAnimation;
    }
}