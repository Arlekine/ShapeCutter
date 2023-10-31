using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RendererDataHelper : MonoBehaviour
{
    [EditorButton]
    private void GetSquare()
    {
        var size = GetComponent<Renderer>().bounds.size;

        print($"Size — {size}");
        print($"Square — {size.x * size.y}");
    }
}
