using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

public class LocalizationSystem
{
    private static Dictionary<string, string> LocalizationDictionary = new Dictionary<string, string>(){ { "ru", "Russian" }, {"en", "English"}};

    public LocalizationSystem(string startingLanguage)
    {
        var lang = "English";
        if (LocalizationDictionary.ContainsKey(startingLanguage))
            lang = LocalizationDictionary[startingLanguage];
        else
            Debug.LogWarning($"Unexpected language - {startingLanguage}, setting to - English");

        if (LocalizationManager.HasLanguage(lang))
            LocalizationManager.CurrentLanguage = lang;
        else
            Debug.LogError($"Unexpected Language - {lang} - set up the {nameof(LocalizationDictionary)}");
    }
}
