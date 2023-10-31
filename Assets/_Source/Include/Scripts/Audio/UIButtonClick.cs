using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonClick : MonoBehaviour
{
    [SerializeField] private AudioManager.StaticSound _clickSound;

    private Button _button;

    private Button Button
    {
        get
        {
            if (_button == null)
                _button = GetComponent<Button>();

            return _button;
        }
    }

    private void OnEnable()
    {
        Button.onClick.AddListener(PlaySound);
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(PlaySound);
    }

    private void PlaySound()
    {
        AudioManager.PlaySound(_clickSound);
    }
}
