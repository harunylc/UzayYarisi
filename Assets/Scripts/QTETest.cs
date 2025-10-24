using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class QTETest : MonoBehaviour
{
    public GameObject AImage, BImage, XImage, YImage;

    private int currentIndex = 0;
    private bool QTETrigger = false;

    private GameObject[] events;
    private string[] buttons = { "A", "B", "X", "Y" };

    void Start()
    {
        events = new GameObject[] { AImage, BImage, XImage, YImage };
        HideAllKeys();
    }

    void Update()
    {
        if (QTETrigger)
        {
            string currentButton = buttons[currentIndex];
            bool pressed = false;

            switch (currentButton)
            {
                case "A":
                    pressed = Gamepad.current.buttonSouth.wasPressedThisFrame;
                    break;
                case "B":
                    pressed = Gamepad.current.buttonEast.wasPressedThisFrame;
                    break;
                case "X":
                    pressed = Gamepad.current.buttonWest.wasPressedThisFrame;
                    break;
                case "Y":
                    pressed = Gamepad.current.buttonNorth.wasPressedThisFrame;
                    break;
            }

            if (pressed)
            {
                NextKeys();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            QTETrigger = true;
            currentIndex = 0;

            RandomKeys();
            ShowCurrentKeys();
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
}
