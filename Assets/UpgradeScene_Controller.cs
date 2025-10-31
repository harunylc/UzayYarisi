using UnityEngine;

public class UpgradeScene_Controller : MonoBehaviour
{
    //UPGRADE EKRANINDA İKİSİDE HAZIR VERİNCE KOŞULUNDA ÇAĞIR
    void ContinueGameFlow()
    {
        SceneFlowManager manager = FindObjectOfType<SceneFlowManager>();
        if (manager != null)
        {
            manager.LoadNextLevelOrEndGame();
        }
    }
}
