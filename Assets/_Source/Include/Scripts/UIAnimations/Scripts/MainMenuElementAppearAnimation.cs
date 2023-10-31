using DG.Tweening;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class MainMenuElementAppearAnimation : UiShowingAnimation
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _rectTransform;

    [Space] 
    [SerializeField] private float _movingOffset = 100f;
    [SerializeField] private Ease _movingEase = Ease.InOutCubic;
    [SerializeField] private float _appearTime = 0.4f;
    [SerializeField] private float _hideTime = 0.2f;

    private Vector2 _moveStartPosition;
    private Vector2 _moveEndPosition;
    private Sequence _currentSequence;
    private bool _isShowed;

    private void Start()
    {
        HideInstantly();
        _moveStartPosition = _rectTransform.anchoredPosition - Vector2.up * _movingOffset;
        _moveEndPosition = _rectTransform.anchoredPosition;
    }

    public override bool IsShowed => _isShowed;

    [EditorButton]
    public override void Show()
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();

        _isShowed = true;
        _currentSequence.AppendCallback(() =>
        {
            _rectTransform.anchoredPosition = _moveStartPosition;
        });
        
        _currentSequence.Append(_canvasGroup.DOFade(1f, _appearTime));
        _currentSequence.Join(_rectTransform.DOAnchorPos(_moveEndPosition, _appearTime));
    }

    [EditorButton]
    public override void Hide()
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();
        
        _isShowed = false;
        _currentSequence.Append(_canvasGroup.DOFade(0f, _hideTime));
    }

    public override void ShowInstantly()
    {
        _canvasGroup.alpha = 1f;
    }

    public override void HideInstantly()
    {
        _canvasGroup.alpha = 0f;
    }

    private void OnDestroy()
    {
        _currentSequence?.Kill();
    }
}