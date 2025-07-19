using System.Collections;
using TMPro;
using UnityEngine;

public class GameDifficultyController : MonoBehaviour
{
    public static GameDifficultyController Instance;
    public float elapsedTime = 0f;
    public GameObject dangerBorderPanel;
    public TMP_Text dangerText;
    public TMP_Text difficultyText;
    private int difficultyLevel = 1;

    [Header("초기 리스폰 간격")]
    public float vaccineSpawnInterval = 1.5f;
    public float zombieSpawnInterval = 5f;
    public float citizenSpawnInterval = 2.5f;

    [Header("하한값")]
    public float minZombieInterval = 1f;
    public float maxVaccineInterval = 3f;
    public float maxCitizenInterval = 3f;

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // 매 10초마다 난이도 조정
        if (elapsedTime % 10f < Time.deltaTime)
        {
            AdjustDifficulty();
        }
    }
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void AdjustDifficulty()
    {
        difficultyLevel++;
        // 좀비: 점점 빠르게
        zombieSpawnInterval = Mathf.Max(minZombieInterval, zombieSpawnInterval - 0.3f);

        // 백신: 점점 느리게
        vaccineSpawnInterval = Mathf.Min(maxVaccineInterval, vaccineSpawnInterval + 0.5f);

        // 시민: 약간 빠르게
        citizenSpawnInterval = Mathf.Min(maxCitizenInterval, citizenSpawnInterval + 0.2f);

        Debug.Log($"[난이도 증가] 백신:{vaccineSpawnInterval}, 좀비:{zombieSpawnInterval}, 시민:{citizenSpawnInterval}");
        
        // difficultyText.text = $"⚠ 난이도: {difficultyLevel}단계";
        StartCoroutine(FlashDifficulty());
        StartCoroutine(ShowDangerEffect());  // ← 여기에 추가
    }
    

    IEnumerator ShowDangerEffect()
    {
        dangerBorderPanel.SetActive(true);
        dangerText.gameObject.SetActive(true);

        CanvasGroup dangerCanvas = dangerBorderPanel.GetComponent<CanvasGroup>();
        if (dangerCanvas == null)
            dangerCanvas = dangerBorderPanel.AddComponent<CanvasGroup>();

        dangerCanvas.alpha = 0f;

        // Fade In
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            dangerCanvas.alpha = Mathf.Lerp(0f, 0.4f, t);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // Fade Out
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            dangerCanvas.alpha = Mathf.Lerp(0.4f, 0f, t);
            yield return null;
        }

        dangerBorderPanel.SetActive(false);
        dangerText.gameObject.SetActive(false);
    }
    IEnumerator FlashDifficulty()
    {
        difficultyText.color = Color.red;
        difficultyText.fontSize = 50;

        yield return new WaitForSeconds(1f);

        difficultyText.color = Color.white;
        difficultyText.fontSize = 36;
    }

}
