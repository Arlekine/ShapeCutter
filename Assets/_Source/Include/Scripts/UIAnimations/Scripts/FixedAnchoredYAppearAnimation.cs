using UnityEngine;

public class FixedAnchoredYAppearAnimation : AnchoredMoveAppearAnimation
{
    public override Vector2 ShowPosition => new Vector2(RectTransform.anchoredPosition.x, _showPosition);
    public override Vector2 HidePosition => new Vector2(RectTransform.anchoredPosition.x, _hidePosition);

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