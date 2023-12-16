using DG.Tweening;
using UnityEngine;

public class CuttingTutorialAnimation : MonoBehaviour, ITutorialAnimation
{
    [SerializeField] private Transform _handTransform;
    [SerializeField] private SpriteRenderer _handRenderer;
    [SerializeField] private LineRenderer _lineRenderer;

    [Space]
    [SerializeField] private float _handClickScale = 0.8f;
    [SerializeField] private float _handClickTime;

    [Space]
    [SerializeField] private float _moveTime;

    private Sequence _currentAnimation;
    private Vector3 _startPoint;
    private Vector3 _toPoint;

    public void Init(Vector3 startPoint, Vector3 toPoint)
    {
        _startPoint = startPoint;
        _toPoint = toPoint;
        _lineRenderer.positionCount = 0;
    }

    public void Play()
    {
        _currentAnimation?.Kill();
        _currentAnimation = DOTween.Sequence();

        _handTransform.position = _startPoint;
        _lineRenderer.positionCount = 2;
        _lineRenderer.useWorldSpace = true;

        _currentAnimation.AppendCallback(() =>
        {
            _lineRenderer.SetPosition(0, _startPoint);
            _lineRenderer.SetPosition(1, _startPoint);

            var color = _handRenderer.color;
            color.a = 0;
            _handRenderer.color = color;
        });

        _currentAnimation.Append(_handRenderer.DOFade(1f, 0.3f));
        _currentAnimation.Append(_handTransform.DOScale(_handClickScale, _handClickTime * 0.5f).SetEase(Ease.InCubic));

        _currentAnimation.Append(DOTween.To(() => _startPoint, pos =>
        {
            _handTransform.position = pos;
            _lineRenderer.SetPosition(1, pos);
        }, _toPoint, _moveTime));

        _currentAnimation.Append(_handTransform.DOScale(1.2f, _handClickTime * 0.5f).SetEase(Ease.InCubic));
        _currentAnimation.Join(_handRenderer.DOFade(0f, 0.3f));
        _currentAnimation.SetLoops(-1);
    }

    [EditorButton]
    public void StopAndDestroy()
    {
        _currentAnimation?.Kill();
        _handRenderer.DOFade(0f, 0.3f);

        Color lineColorStart, lineColorEnd;
        lineColorStart = lineColorEnd= _lineRenderer.colorGradient.colorKeys[0].color;
        lineColorEnd.a = 0;

        _lineRenderer.DOColor(new Color2(lineColorStart, lineColorStart), new Color2(lineColorEnd, lineColorEnd), 0.3f);

        Destroy(gameObject, 0.4f);
    }
}