using UnityEngine;

public static class CarSelectionLocker
{
    private static bool carsAreLocked = false;
    private static int lockedCarIndexP1 = 0;
    private static int lockedCarIndexP2 = 0;
    
    public static void LockSelections(int carIndexP1, int carIndexP2)
    {
        carsAreLocked = true;
        lockedCarIndexP1 = carIndexP1;
        lockedCarIndexP2 = carIndexP2;
    }

    public static void UnlockSelections()
    {
        carsAreLocked = false;
    }
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