using UnityEngine;
using UnityEngine.UI;

public class OutOfScreenMoveAnimation : AnchoredMoveAppearAnimation
{
    public enum ScreenDirection
    {
        Up,
        Down,
        Right,
        Left
    }
    
    public override Vector2 ShowPosition => _centerRelativeShowPosition;
    public override Vector2 HidePosition => _centerRelativeShowPosition - GetVectorForScreenDirection(_appearDirection) * GetHideOffsetForDirection(_appearDirection);

    [Space]
    [SerializeField] private ScreenDirection _appearDirection;
    [SerializeField] private Vector2 _centerRelativeShowPosition;

    private CanvasScaler _canvasScaler;
    
    private void Awake()
    {
        _panel.anchorMax = Vector2.one * 0.5f;
        _panel.anchorMin = Vector2.one * 0.5f;

        _canvasScaler = GetComponentInParent<CanvasScaler>();
    }

    private Vector2 GetVectorForScreenDirection(ScreenDirection screenDirection)
    {
        Vector2 result = Vector2.zero;
        switch (screenDirection)
        {
            case ScreenDirection.Down:
                result = Vector2.down;
                break;
            
            case ScreenDirection.Up:
                result = Vector2.up;
                break;
            
            case ScreenDirection.Left:
                result = Vector2.left;
                break;
            
            case ScreenDirection.Right:
                result = Vector2.right;
                break;
        }

        return result;
    }

    private float GetHideOffsetForDirection(ScreenDirection screenDirection)
    {
        if (screenDirection == ScreenDirection.Right || screenDirection == ScreenDirection.Left)
            return (_canvasScaler.referenceResolution.x / 2 + _panel.sizeDelta.x / 2);
        else
            return (_canvasScaler.referenceResolution.y / 2 + _panel.sizeDelta.y / 2);
    }
}