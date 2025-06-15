using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = Random.insideUnitCircle * 5f;
        GameObject newEnemy = Instantiate(enemyPrefab, new Vector3(spawnPos.x, spawnPos.y, 0f), Quaternion.identity);

        // ✅ 여기에서 GameManager에 적 등록
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            Enemy enemyScript = newEnemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                gm.RegisterEnemy(enemyScript);
            }
        }
    }
}
