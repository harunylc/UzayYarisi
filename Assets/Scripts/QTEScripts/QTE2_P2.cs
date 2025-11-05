using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class QTE2_P2 : MonoBehaviour
{
    [Header("QTE Buttons")]
    public GameObject AImage, BImage, XImage, YImage;

    [Header("Raycast Area")]
    public Vector2 raycastPosition;
    public Vector2 raycastSize = new Vector2(2f, 2f);
    public LayerMask playerLayer;

    [Header("Attention UI")]
    public GameObject attentionImage;

    [Header("Countdown Slider")]
    public Slider countdownSlider;
    public float countdownTime = 2f;

    [Header("Diken Settings")]
    public GameObject dikenPrefab;

    public float zeminY = 0f;

    [Header("Patlama Ayarları")]
    public GameObject explosionPrefab;
    private Transform player;

    private GameObject[] events;
    private string[] buttons = { "A", "B", "X", "Y" };
    private int currentIndex = 0;

    private bool countdownActive = false;
    private bool QTETrigger = false;
    private bool QTECompleted = false;
    private bool dikenSpawned = false;

    void Start()
    {
        events = new GameObject[] { AImage, BImage, XImage, YImage };
        HideAllKeys();

        if (attentionImage != null)
        {
            attentionImage.SetActive(false);
        }

        if (countdownSlider != null)
        {
            countdownSlider.maxValue = countdownTime;
            countdownSlider.value = countdownTime;
            countdownSlider.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (QTECompleted) return;

        if (!QTETrigger)
        {
            bool playerInArea = Physics2D.OverlapBox(raycastPosition, raycastSize, 0f, playerLayer) != null;
            if (attentionImage != null)
            {
                attentionImage.SetActive(playerInArea);
            }
        }

        if (countdownActive && countdownSlider != null)
        {
            countdownSlider.value -= Time.deltaTime;
            if (countdownSlider.value <= 0f)
            {
                countdownSlider.value = 0f;
                countdownActive = false;

                if (!dikenSpawned)
                {
                    dikenSpawned = true;
                    OnQTEFailedByTime();
                }
            }
        }

        if (QTETrigger)
        {
            string currentButton = buttons[currentIndex];
            bool pressed = false;

            switch (currentButton)
            {
                case "A": pressed = Gamepad.current?.buttonSouth.wasPressedThisFrame ?? false; break;
                case "B": pressed = Gamepad.current?.buttonEast.wasPressedThisFrame ?? false; break;
                case "X": pressed = Gamepad.current?.buttonWest.wasPressedThisFrame ?? false; break;
                case "Y": pressed = Gamepad.current?.buttonNorth.wasPressedThisFrame ?? false; break;
            }

            if (pressed)
            {
                NextKeys();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (QTETrigger || QTECompleted)
        {
            return;
        }

        Transform rootPlayer = other.transform.root;
        if (rootPlayer.CompareTag("Player2"))
        {
            player = rootPlayer;

            if (attentionImage != null)
            {
                attentionImage.SetActive(false);
            }

            QTETrigger = true;
            currentIndex = 0;
            RandomKeys();
            ShowCurrentKeys();

            if (countdownSlider != null)
            {
                countdownSlider.gameObject.SetActive(true);
                countdownSlider.value = countdownTime;
                countdownActive = true;
            }

            dikenSpawned = false;
        }
    }

    void ShowCurrentKeys()
    {
        HideAllKeys();
        events[currentIndex].SetActive(true);
    }

    void NextKeys()
    {
        currentIndex++;
        if (currentIndex < events.Length)
        {
            ShowCurrentKeys();
        }
        else
        {
            HideAllKeys();
            QTETrigger = false;
            QTECompleted = true;

            if (countdownSlider != null)
            {
                countdownSlider.gameObject.SetActive(false);
            }

        }
    }

    void HideAllKeys()
    {
        foreach (var img in events)
        {
            if (img != null)
            {
                img.SetActive(false);
            }
        }
    }

    void RandomKeys()
    {
        for (int i = 0; i < events.Length; i++)
        {
            int rand = Random.Range(i, events.Length);
            (events[i], events[rand]) = (events[rand], events[i]);
            (buttons[i], buttons[rand]) = (buttons[rand], buttons[i]);
        }
    }

    void OnQTEFailedByTime()
    {
        HideAllKeys();
        QTETrigger = false;
        QTECompleted = true;

        if (countdownSlider != null)
        {
            countdownSlider.gameObject.SetActive(false);
        }

        SpawnDikenAndExplode();
    }

    void SpawnDikenAndExplode()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player2");
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
        }

        if (player == null)
        {
            return;
        }

        if (dikenPrefab != null)
        {
            Vector3 spawnPos = player.position;
            spawnPos.y = zeminY;
            Instantiate(dikenPrefab, spawnPos, Quaternion.identity);
        }

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, player.position, Quaternion.identity);
            Destroy(explosion, 3f);
        }

        Destroy(player.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(raycastPosition, raycastSize);
    }
}
