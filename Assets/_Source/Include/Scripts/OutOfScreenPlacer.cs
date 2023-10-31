using UnityEngine;

[DefaultExecutionOrder(int.MinValue)]
public class OutOfScreenPlacer : MonoBehaviour
{
    private enum ScreenBorder
    {
        Up,
        Left,
        Right,
        Down
    }

    [SerializeField] private ScreenBorder _border;
    
    [SerializeField] private Vector2 _borderCenterOffsetNormalized;
    [SerializeField] private float _zPosition;
    [SerializeField] private bool _update;


    private void Awake()
    {
        Place();
        enabled = _update;
    }

    private void Update()
    {
        Place();
    }

    private void Place()
    {
        Vector3 borderPoint = new Vector3();
        
        switch (_border)
        {
            case ScreenBorder.Down:

                borderPoint.x = Screen.width * 0.5f;

                break;

            case ScreenBorder.Up:

                borderPoint.x = Screen.width * 0.5f;
                borderPoint.y = Screen.height;

                break;

            case ScreenBorder.Left:

                borderPoint.y = Screen.height * 0.5f;

                break;

            case ScreenBorder.Right:

                borderPoint.x = Screen.width;
                borderPoint.y = Screen.height * 0.5f;

                break;
        }

        borderPoint.x += _borderCenterOffsetNormalized.x * Screen.width;
        borderPoint.y += _borderCenterOffsetNormalized.y * Screen.height;

        var worldPoint = Camera.main.ScreenToWorldPoint(borderPoint);
        worldPoint.z = _zPosition;
        transform.position = worldPoint;
    }
}
