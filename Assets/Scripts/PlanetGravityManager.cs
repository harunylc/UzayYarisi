using UnityEngine;

public class PlanetGravityManager : MonoBehaviour
{
    [Header("Bulundugun sahnenin yer cekim degerini gir")]
    public float gravityScale = -10f;

    private void Awake()
    {
        Physics2D.gravity = new Vector2(0, gravityScale);
    }
}
