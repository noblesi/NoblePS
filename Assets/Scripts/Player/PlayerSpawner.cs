using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoint;

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if(GameObject.FindGameObjectWithTag("Player") == null)
        {
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

            GameManager gameManager = GameManager.Instance;
            PlayerData playerData = gameManager.GetPlayerData();

            PlayerFSM playerFSM = player.GetComponent<PlayerFSM>();
            if(playerFSM != null)
            {
                playerFSM.SetPlayerData(playerData);
                Debug.Log("플레이어가 스폰되고 데이터가 초기화되었습니다.");
            }

        }
    }
}
