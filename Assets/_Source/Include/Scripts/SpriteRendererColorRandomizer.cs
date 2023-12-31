using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererColorRandomizer : ShapeColorHolder
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Color> _colorsVariants = new List<Color>();

    public Color Randomize()
    {
        _spriteRenderer.color = _colorsVariants.GetRandomElement();
        return _spriteRenderer.color;
    }

    public override Color GetColor()
    {
        return Randomize();
    }
}

public abstract class ShapeColorHolder : MonoBehaviour
{
    public abstract Color GetColor();
}