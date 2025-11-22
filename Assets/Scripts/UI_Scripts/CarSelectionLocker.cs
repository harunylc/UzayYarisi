using UnityEngine;

public static class CarSelectionLocker
{
    // Bu değişken 'static' olduğu için, sahneler değişse bile değerini korur.
    private static bool carsAreLocked = false;
    private static int lockedCarIndexP1 = 0;
    private static int lockedCarIndexP2 = 0;
    
    // Arabaları kilitleyen fonksiyon
    public static void LockSelections(int carIndexP1, int carIndexP2)
    {
        carsAreLocked = true;
        lockedCarIndexP1 = carIndexP1;
        lockedCarIndexP2 = carIndexP2;
        Debug.Log($"Arabalar kilitlendi! P1: {carIndexP1}, P2: {carIndexP2}");
    }

    // Yeni bir oyuna başlandığında kilidi açan fonksiyon
    public static void UnlockSelections()
    {
        carsAreLocked = false;
        Debug.Log("Araba kilitleri açıldı.");
    }
    
    // Diğer script'lerin kullanacağı sorgulama fonksiyonları
    public static bool AreCarsLocked()
    {
        return carsAreLocked;
    }

    public static int GetLockedCarIndexP1()
    {
        return lockedCarIndexP1;
    }

    public static int GetLockedCarIndexP2()
    {
        return lockedCarIndexP2;
    }
}