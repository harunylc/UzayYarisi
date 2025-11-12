using UnityEngine;

public class GameplayMusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip gameplayMusicClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = gameplayMusicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.Play();
    }
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, Vector3.zero, volume);
    }

}