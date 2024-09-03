using UnityEngine;

public class GameManager : MonoBehaviour
{
    private DataManager dataManager;

    private void OnApplicationQuit()
    {
        dataManager.SaveAllData();
    }
}
