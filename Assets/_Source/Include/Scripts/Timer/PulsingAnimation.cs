using DG.Tweening;
using UnityEngine;

public sealed class PulsingAnimation : MonoBehaviour
{
    [Space]
    [SerializeField] private float _bigScale;
    [SerializeField] private float _smallScale;
    [SerializeField] private float _circleTime;

    [Space]
    [SerializeField] private Transform _initialTarget;

    private Sequence _currentSequence;

    private void Start()
    {
        if (_initialTarget != null)
            Play(_initialTarget);
    }

    public bool IsPlaying() => _currentSequence.IsActive() && _currentSequence.IsPlaying();

    public Sequence Play(Transform target)
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(target.transform.DOScale(_bigScale, _circleTime * 0.5f));
        _currentSequence.Append(target.transform.DOScale(_smallScale, _circleTime * 0.5f));
        _currentSequence.SetLoops(-1);

        return _currentSequence;
    }
    
    private void OnDestroy()
    {
        _currentSequence?.Kill();
    }
}