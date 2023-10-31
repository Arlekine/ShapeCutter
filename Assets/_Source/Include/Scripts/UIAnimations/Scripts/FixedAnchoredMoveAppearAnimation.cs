using UnityEngine;

public class FixedAnchoredMoveAppearAnimation : AnchoredMoveAppearAnimation
{
    public override Vector2 ShowPosition => _showPosition;
    public override Vector2 HidePosition => _hidePosition;

    [Space] 
    [SerializeField] private Vector2 _showPosition;
    [SerializeField] private Vector2 _hidePosition;
}