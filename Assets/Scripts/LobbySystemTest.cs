using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbySystemTest : MonoBehaviour
{
    public Button readyButtonP1, readyButtonP2;
    
    public void BuffButtonP1()
    {
        Debug.Log("P1 Buff");
    }
    
    public void NerfButtonP1()
    {
        Debug.Log("P1 Nerf");
    }
    
    public void BuffButtonP2()
    {
        Debug.Log("P2 Buff");
    }
    
    public void NerfButtonP2()
    {
        Debug.Log("P2 Nerf");
    }
    
    public void ReadyButtonP1()
    {
        readyButtonP1.interactable = false;
    }
    
    public void ReadyButtonP2()
    {
        readyButtonP2.interactable = false;
    }
    
    public void RightButtonP1()
    {
        Debug.Log("P1 Right");
    }
    
    public void LeftButtonP1()
    {
        Debug.Log("P1 Left");
    }
    
    public void RightButtonP2()
    {
        Debug.Log("P2 rİGHT");
    }
    
    public void LeftButtonP2()
    {
        Debug.Log("P2 Left");
    }

    private void Update()
    {
        if (readyButtonP1.interactable == false && readyButtonP2.interactable == false)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
