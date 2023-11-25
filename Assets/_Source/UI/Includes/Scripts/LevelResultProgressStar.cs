using UnityEngine;

public class LevelResultProgressStar : MonoBehaviour
{
    [SerializeField] private UiShowingAnimation _animation;

    private RectTransform _rectTransform;
    private float _targetProgress;
    private bool _isShowed;

    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            return _rectTransform;
        }
    }

    public void Init(float targetProgress, float parentWidth)
    {
        _targetProgress = targetProgress;
        RectTransform.anchoredPosition = new Vector2(parentWidth * targetProgress, RectTransform.anchoredPosition.y);

        _animation.HideInstantly();
    }

    public void UpdateSelection(float currentProgress)
    {
        if (currentProgress >= _targetProgress && _isShowed == false)
        {
            _isShowed = true;
            _animation.Show();
        }
        else if (currentProgress < _targetProgress && _isShowed == true)
        {
            _isShowed = false;
            _animation.Hide();
        }
    }
}