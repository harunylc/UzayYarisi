// PlayerSelectionData.cs
public static class PlayerSelectionData
{
    // Seçilen araba görsellerinin indeksi (Zaten kullaniyordunuz)
    public static int player1CarIndex = 0;
    public static int player2CarIndex = 0;

    // YENİ EKLENTİLER: PointManager'dan kaydedilecek nihai araç özellikleri
    // PointManager'da bu alanlara kayit yapildigi icin tanimli olmalari gerekir.
    public static float player1FinalHız = 0f;
    public static float player1FinalFren = 0f;
    public static float player1FinalNitro = 0f;
    public static float player1FinalYoltutus = 0f;
    public static float player1FinalAgırlık = 0f;
    
    public static float player2FinalHız = 0f;
    public static float player2FinalFren = 0f;
    public static float player2FinalNitro = 0f;
    public static float player2FinalYoltutus = 0f;
    public static float player2FinalAgırlık = 0f;
}