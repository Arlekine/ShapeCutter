using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UiShowingAnimation : MonoBehaviour
{
    public abstract bool IsShowed { get; }
    
    public abstract void Show();
    public abstract void Hide();

    public abstract void ShowInstantly();
    public abstract void HideInstantly();

    public void Switch()
    {
        if (IsShowed)
            Hide();
        else
            Show();
    }
}