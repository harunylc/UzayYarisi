using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class QTETest : MonoBehaviour
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

    [Header("QTE Settings")]
    public float timeLimit = 2f;

    private GameObject[] events;
    private string[] buttons = { "A", "B", "X", "Y" };
    private int currentIndex = 0;
    private float timer = 0f;
    private bool countdownActive = false;
    private bool QTETrigger = false;
    private bool QTECompleted = false;
    public GameObject Meteor;

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
        if (QTECompleted)
            return;

        if (!QTETrigger)
        {
            bool playerInArea = Physics2D.OverlapBox(raycastPosition, raycastSize, 0f, playerLayer) != null;
            if (attentionImage != null)
            {
                attentionImage.SetActive(playerInArea);
            }
        }

        if (countdownActive)
        {
            countdownSlider.value -= Time.deltaTime;
            if (countdownSlider.value <= 0f)
            {
                countdownSlider.value = 0f;
            }
        }

        if (QTETrigger)
        {
            timer += Time.deltaTime;
            string currentButton = buttons[currentIndex];
            bool pressed = false;

            switch (currentButton)
            {
                case "A": pressed = Gamepad.current.buttonSouth.wasPressedThisFrame; break;
                case "B": pressed = Gamepad.current.buttonEast.wasPressedThisFrame; break;
                case "X": pressed = Gamepad.current.buttonWest.wasPressedThisFrame; break;
                case "Y": pressed = Gamepad.current.buttonNorth.wasPressedThisFrame; break;
            }

            if (pressed)
                NextKeys();

            if (timer > timeLimit)
            {
                HideAllKeys();
                QTETrigger = false;
                QTECompleted = true;
                Destroy(this.gameObject);
                countdownSlider.gameObject.SetActive(false);
                Debug.Log("Kaybettin!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (QTECompleted)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            if (attentionImage != null)
            {
                attentionImage.SetActive(false);
            }

            QTETrigger = true;
            timer = 0f;
            currentIndex = 0;
            RandomKeys();
            ShowCurrentKeys();

            if (countdownSlider != null)
            {
                countdownSlider.gameObject.SetActive(true);
                countdownSlider.value = countdownTime;
                countdownActive = true;
            }
            Meteor.SetActive(true);
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
            Destroy(this.gameObject);
            countdownSlider.gameObject.SetActive(false);
            Debug.Log("Başardın!");
        }
    }

    void HideAllKeys()
    {
        foreach (var img in events)
        {
            img.SetActive(false);
        }
    }

    void RandomKeys()
    {
        for (int i = 0; i < events.Length; i++)
        {
            int rand = Random.Range(i, events.Length);
            var tempImg = events[i];
            events[i] = events[rand];
            events[rand] = tempImg;

            var tempKey = buttons[i];
            buttons[i] = buttons[rand];
            buttons[rand] = tempKey;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(raycastPosition, raycastSize);
    }
}
