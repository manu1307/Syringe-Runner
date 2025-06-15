using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenSpawner : MonoBehaviour
{
    public GameObject[] citizenPrefabs;  // 시민 프리팹들
    public float spawnInterval = 1f;
    public float minSpeed = 1.0f;
    public float maxSpeed = 3.0f;

    private float timer = 0f;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCitizen();
            timer = 0f;
        }
    }

    void SpawnCitizen()
    {
        if (citizenPrefabs.Length == 0) return;
        
        int index = Random.Range(0, citizenPrefabs.Length);

        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        float spawnX = Random.Range(
            cam.transform.position.x - camHalfWidth + 0.5f,
            cam.transform.position.x + camHalfWidth - 0.5f
        );

        float spawnY = Random.Range(
            cam.transform.position.y - camHalfHeight + 0.5f,
            cam.transform.position.y + camHalfHeight - 0.5f
        );

        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);
        GameObject newCitizen = Instantiate(citizenPrefabs[index], spawnPos, Quaternion.identity);

        // 속도 랜덤 설정
        Citizen citizenScript = newCitizen.GetComponent<Citizen>();
        if (citizenScript != null)
        {
            float randomSpeed = Random.Range(minSpeed, maxSpeed);
            citizenScript.moveSpeed = randomSpeed;
        }
    }
}

