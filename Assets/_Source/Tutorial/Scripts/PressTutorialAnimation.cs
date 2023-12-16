using UnityEngine;

public class PressTutorialAnimation : MonoBehaviour, ITutorialAnimation
{
    [SerializeField] private RectTransform _hand;
    [SerializeField] private PulsingAnimation _animation;

    public void SetPoint(RectTransform parent, Vector2 anchoredPosition)
    {
        var transform = GetComponent<RectTransform>();
        transform.parent = parent;
        transform.anchoredPosition = anchoredPosition;
    }

    public void Play()
    {
        _animation.Play(_hand);
    }

    public void StopAndDestroy()
    {
        Destroy(gameObject);
    }
}