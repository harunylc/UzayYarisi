using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SoundSO menuMusicSO;

    private void Start()
    {
        AudioManager.Instance.SoundToPlay(menuMusicSO);
    }
}