using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs;
    public Transform[] spawnPoints;

    public void SpawnMonsters()
    {
        foreach(Transform spawnPoint in spawnPoints)
        {
            int randomIndex = Random.Range(0, monsterPrefabs.Length);
            GameObject selectedMonsterPrefab = monsterPrefabs[randomIndex];

            Instantiate(selectedMonsterPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
