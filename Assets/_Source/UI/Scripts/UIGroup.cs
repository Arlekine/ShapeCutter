using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGroup : MonoBehaviour
{
    [SerializeField] private UiShowingAnimation _showAnimation;

    public void Show() =>  _showAnimation.Show();

    public void Hide() => _showAnimation.Hide();
}
