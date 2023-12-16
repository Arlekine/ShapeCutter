using UnityEngine;
using UnityEngine.UI;

public class BackgroundAnimation : MonoBehaviour
{
    [SerializeField] private Vector2 scrollSpeed;
    [SerializeField] private RawImage renderer;

    void Update()
    {
        float x = Mathf.Repeat(Time.time * scrollSpeed.x, 1);
        float y = Mathf.Repeat(Time.time * scrollSpeed.y, 1);

        Vector2 offset = new Vector2(x, y);

        var rect = renderer.uvRect;
        rect.position = offset;
        renderer.uvRect = rect;
    }
}
