using UnityEngine;
using UnityEngine.UI;

public class PauseButtonSounds : MonoBehaviour
{
    [SerializeField] private SoundSO buttonClickSO;
    [SerializeField] private GameplayMusicPlayer musicPlayer;

    private void OnEnable()
    {
        Button[] buttons = GetComponentsInChildren<Button>();

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PlayClickSound());
        }
    }

    private void PlayClickSound()
    {
        musicPlayer.PlaySFX(buttonClickSO.Clip);
    }
}