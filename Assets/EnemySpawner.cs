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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector2 playerPos = player.transform.position;

        // 플레이어 기준에서 일정 거리 떨어진 랜덤 방향
        float distance = Random.Range(2f, 8f); // 최소 2~최대 8유닛 떨어진 곳
        float angle = Random.Range(0f, 360f);
        Vector2 offset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * distance;

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
