using UnityEngine;
using UnityEngine.UI;

public class MenuButtonSounds : MonoBehaviour
{
    [SerializeField] private SoundSO buttonClickSO;

    private void Start()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PlayClickSound());
        }
    }

    private void PlayClickSound()
    {
        AudioManager.Instance.SoundToPlay(buttonClickSO);
    }
}