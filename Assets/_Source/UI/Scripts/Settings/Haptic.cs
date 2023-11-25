using MoreMountains.NiceVibrations;

public static class Haptic
{
    public static bool IsHapticActive;

    public static void VibrateLight()
    {
        if (IsHapticActive)
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
    }

    public static void VibrateMedium()
    {
        if (IsHapticActive)
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
    }

    public static void VibrateHeavy()
    {
        if (IsHapticActive)
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
    }
}