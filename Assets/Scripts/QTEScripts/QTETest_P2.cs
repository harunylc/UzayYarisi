using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class QTETest_P2 : MonoBehaviour
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

    [Header("Meteor Settings")]
    public GameObject meteorPrefab;
    public Transform meteorSpawnPoint;
    public Transform player;
    public float meteorSpeed = 6f;
    public GameObject explosionPrefab;

    private GameObject[] events;
    private string[] buttons = { "A", "B", "X", "Y" };
    private int currentIndex = 0;

    private bool countdownActive = false;
    private bool QTETrigger = false;
    private bool QTECompleted = false;
    private bool meteorSpawned = false;

    private void Start()
    {
        events = new GameObject[] { AImage, BImage, XImage, YImage };
        HideAllKeys();

        if (attentionImage != null)
            attentionImage.SetActive(false);

        if (countdownSlider != null)
        {
            countdownSlider.maxValue = countdownTime;
            countdownSlider.value = countdownTime;
            countdownSlider.gameObject.SetActive(false);
        }

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player2")?.transform;
    }

    private void Update()
    {
        if (QTECompleted) return;

        if (!QTETrigger)
        {
            bool playerInArea = Physics2D.OverlapBox(raycastPosition, raycastSize, 0f, playerLayer);
            if (attentionImage != null)
                attentionImage.SetActive(playerInArea);
        }

        if (countdownActive && countdownSlider != null)
        {
            countdownSlider.value -= Time.deltaTime;
            if (countdownSlider.value <= 0f)
            {
                countdownSlider.value = 0f;
                countdownActive = false;

                if (!meteorSpawned)
                {
                    meteorSpawned = true;
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
                NextKeys();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (QTECompleted) return;

        if (other.CompareTag("Player2"))
        {
            if (attentionImage != null)
                attentionImage.SetActive(false);

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

            meteorSpawned = false;
        }
    }

    private void ShowCurrentKeys()
    {
        HideAllKeys();
        events[currentIndex].SetActive(true);
    }

    private void NextKeys()
    {
        currentIndex++;
        if (currentIndex < events.Length)
            ShowCurrentKeys();
        else
        {
            HideAllKeys();
            QTETrigger = false;
            QTECompleted = true;

            if (countdownSlider != null)
                countdownSlider.gameObject.SetActive(false);
        }
    }

    private void HideAllKeys()
    {
        foreach (var img in events)
            if (img != null)
                img.SetActive(false);
    }

    private void RandomKeys()
    {
        for (int i = 0; i < events.Length; i++)
        {
            int rand = Random.Range(i, events.Length);
            (events[i], events[rand]) = (events[rand], events[i]);
            (buttons[i], buttons[rand]) = (buttons[rand], buttons[i]);
        }
    }

    private void OnQTEFailedByTime()
    {
        HideAllKeys();
        QTETrigger = false;
        QTECompleted = true;

        if (countdownSlider != null)
            countdownSlider.gameObject.SetActive(false);

        // Eğer oyuncuda shield varsa meteor spawn etme
        if(player != null && player.GetComponentInChildren<Shield>() != null)
            return;

        SpawnMeteor();
    }

    private void SpawnMeteor()
    {
        if(player == null) return;

        GameObject meteor = Instantiate(meteorPrefab, meteorSpawnPoint.position, Quaternion.identity);
        MeteorFollow2 mf = meteor.GetComponent<MeteorFollow2>() ?? meteor.AddComponent<MeteorFollow2>();
        mf.target = player;
        mf.speed = meteorSpeed;
        mf.explosionPrefab = explosionPrefab;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(raycastPosition, raycastSize);
    }
}

public class MeteorFollow2 : MonoBehaviour
{
    public Transform target;
    public float speed = 6f;
    public GameObject explosionPrefab;

    private void Update()
    {
        if (target == null) return;

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Transform root = other.transform.root;
        if (root.CompareTag("Player2"))
        {
            // Shield varsa meteor yok olur ama oyuncuya zarar gelmez
            if(root.GetComponentInChildren<Shield>() != null)
            {
                Destroy(gameObject);
                return;
            }

            // Patlama
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, root.position, Quaternion.identity);
                Destroy(explosion, 3f);
            }

            Destroy(root.gameObject); // oyuncu yok olur
            Destroy(gameObject);      // meteor yok olur
        }
    }
}
