using UnityEngine;

public class FixedAnchoredXAppearAnimation : AnchoredMoveAppearAnimation
{
    public override Vector2 ShowPosition => new Vector2(_showPosition, RectTransform.anchoredPosition.y);
    public override Vector2 HidePosition => new Vector2(_hidePosition, RectTransform.anchoredPosition.y);

    [Space]
    [SerializeField] private float _showPosition;
    [SerializeField] private float _hidePosition;

    private RectTransform _rectTransform;

    private RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            return _rectTransform;
        }
    }
}