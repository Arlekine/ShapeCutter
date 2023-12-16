using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageLockedPanel : MonoBehaviour
{
    private const string FirstStarKey = "StageLocked_Current";
    private const string SecondStarKey = "LockedStage_Need";

    [SerializeField] private UiShowingAnimation _showingAnimation;
    [SerializeField] private TMP_Text _firstStarsLine;
    [SerializeField] private TMP_Text _secondStarsLine;

    [Space]
    [SerializeField] private Button _homeButton;

    public Action OnHome;

    public void Show(int currentStars, int neededStars)
    {
        _showingAnimation.Show();
        _firstStarsLine.text = String.Format(LocalizationManager.GetTermTranslation(FirstStarKey), currentStars);
        _secondStarsLine.text = String.Format(LocalizationManager.GetTermTranslation(SecondStarKey), neededStars);
    }

    public void Hide()
    {
        _showingAnimation.Hide();
    }

    private void OnEnable()
    {
        _homeButton.onClick.AddListener(OnHomeClick);
    }

    private void OnDisable()
    {
        _homeButton.onClick.RemoveListener(OnHomeClick);
    }

    private void OnHomeClick()
    {
        OnHome?.Invoke();
    }
}