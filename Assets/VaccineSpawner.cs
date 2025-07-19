using UnityEngine;

public class VaccineSpawner : MonoBehaviour
{
    public GameObject vaccinePrefab;
    public float spawnXRange = 3f;
    
    // 유니티 에디터에서 세팅
    public float spawnYMin = -5f;
    public float spawnYMax = 0f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        float interval = GameDifficultyController.Instance.vaccineSpawnInterval;

        if (timer >= interval)
        {
            SpawnVaccine();
            timer = 0f;
        }
    }

    void SpawnVaccine()
    {
        
        float randX = Random.Range(-spawnXRange, spawnXRange);
        float randY = Random.Range(spawnYMin, spawnYMax); // Y도 랜덤
        Vector3 spawnPos = new Vector3(randX, randY, 0f);
        Instantiate(vaccinePrefab, spawnPos, Quaternion.identity);
    }
}