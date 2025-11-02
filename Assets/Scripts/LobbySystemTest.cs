// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;
//
// public class LobbySystemTest : MonoBehaviour
// {
//     [Header("UI References")]
//     public Button readyButtonP1, readyButtonP2 ,rightButtonP1, leftButtonP1, rightButtonP2, leftButtonP2;
//     public Image carImageP1, carImageP2;
//     public Sprite[] carSpritesP1, carSpritesP2;
//
//     private int carIndexP1 = 0;
//     private int carIndexP2 = 0;
//     private int maxCars = 5;
//
//     public void RightButtonP1()
//     {
//         carIndexP1 = (carIndexP1 + 1) % maxCars;
//         carImageP1.sprite = carSpritesP1[carIndexP1];
//     }
//
//     public void LeftButtonP1()
//     {
//         carIndexP1 = (carIndexP1 - 1 + maxCars) % maxCars;
//         carImageP1.sprite = carSpritesP1[carIndexP1];
//     }
//
//     public void RightButtonP2()
//     {
//         carIndexP2 = (carIndexP2 + 1) % maxCars;
//         carImageP2.sprite = carSpritesP2[carIndexP2];
//     }
//
//     public void LeftButtonP2()
//     {
//         carIndexP2 = (carIndexP2 - 1 + maxCars) % maxCars;
//         carImageP2.sprite = carSpritesP2[carIndexP2];
//     }
//
//     public void ReadyButtonP1()
//     {
//         readyButtonP1.interactable = false;
//     }
//
//     public void ReadyButtonP2()
//     {
//         readyButtonP2.interactable = false;
//     }
//
//     private void Update()
//     {
//         if (!readyButtonP1.interactable && !readyButtonP2.interactable)
//         {
//             PlayerSelectionData.player1CarIndex = carIndexP1;
//             PlayerSelectionData.player2CarIndex = carIndexP2;
//
//             SceneManager.LoadScene("SampleScene");
//         }
//     }
// }