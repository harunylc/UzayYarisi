using UnityEngine;

public class PlanetGravityManager : MonoBehaviour
{
    [Header("Bulundugun sahnenin yer cekim degerini gir")]
    public float gravityScale = -10f;

    // private void Awake()
    // {
    //     Physics.gravity = new Vector2(0f, gravityScale);
    //     Debug.Log(gravityScale);
    // }
    private void Awake()
    {
        Physics2D.gravity = new Vector2(0, gravityScale);
        Debug.Log($"Yerçekimi {gravityScale} olarak ayarlandı! Mevcut değer: {Physics2D.gravity}");
    }
}
