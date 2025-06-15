using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int score = 0;         // 전체 점수
    private int tmpScore = 0;      // KillAll 버튼용 점수
    private float killMaxScore = 5f; // KillAll 활성화 점수
    private Coroutine pulseRoutine;
    
    public Button killAllButton;
    public GameObject gameOverPanel; // UI에 패널 연결
    public Image tmpScoreGaugeImage;
    public Transform gaugeWrapperTransform; 

    private List<Enemy> enemies = new List<Enemy>();

    [Header("UI")]
    
    public TMP_Text scoreText; // 기존 Text → TMP_Text

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        tmpScoreGaugeImage.fillAmount = 0f; // ✅ 게이지 초기화
        UpdateGaugeColor(0f);
        killAllButton.gameObject.SetActive(false); // ← 버튼을 아예 숨김 상태로 시작
        killAllButton.onClick.AddListener(KillAllEnemies);
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        tmpScore += amount; 
        
        float fill = Mathf.Clamp01(tmpScore / killMaxScore);
        tmpScoreGaugeImage.fillAmount = fill;
        UpdateGaugeColor(fill);
        
        UpdateScoreUI();
        if (tmpScore >= killMaxScore)
        {
            if (!killAllButton.gameObject.activeSelf)
            {
                killAllButton.gameObject.SetActive(true);
            }

            killAllButton.interactable = true;
            EventSystem.current.SetSelectedGameObject(null);

            // 🔥 이펙트 시작
            if (pulseRoutine == null)
                pulseRoutine = StartCoroutine(PulseEffect());
        }
            
    }
    
    public void RegisterEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    public void KillAllEnemies()
    {
        foreach (Enemy e in enemies)
        {
            if (e != null)
                e.Kill();
        }
        enemies.Clear();
        
        tmpScore = 0; // 버튼 클릭 시 tmpScore 초기화
        tmpScoreGaugeImage.fillAmount = 0f; 
        killAllButton.gameObject.SetActive(false);
        
        // 🛑 이펙트 중단
        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
            pulseRoutine = null;

            // 초기화
            killAllButton.transform.localScale = Vector3.one;
            tmpScoreGaugeImage.transform.localScale = Vector3.one;
            killAllButton.GetComponent<Image>().color = Color.red;
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $" {score}명";
        }
    }
    public void GameOver()
    {
        Debug.Log("게임 종료");
        Time.timeScale = 0f; // 게임 일시 정지

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f; // 게임 재개
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 다시 로드
    }
    private IEnumerator PulseEffect()
    {
        float t = 0f;
        Image buttonImage = killAllButton.GetComponent<Image>();
        Color originalColor = buttonImage.color;
        Color targetColor = Color.white;

        while (true)
        {
            t += Time.deltaTime * 4f;
            float alpha = Mathf.PingPong(t, 1f);

            // 버튼 색상 반짝임
            buttonImage.color = Color.Lerp(originalColor, targetColor, alpha);

            // 스케일 흔들기 (버튼 + 게이지 wrapper에 적용)
            float scale = 1f + Mathf.Sin(Time.time * 6f) * 0.05f;

            killAllButton.transform.localScale = new Vector3(scale, scale, 1f);
            if (gaugeWrapperTransform != null)
                gaugeWrapperTransform.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }
    }
    
    private void UpdateGaugeColor(float fill)
    {
        // 연두 → 노랑 → 빨강 그라데이션
        Color lowColor = new Color(0.3f, 1f, 0.4f);     // 연두
        Color midColor = new Color(1f, 0.8f, 0f);        // 노랑
        Color fullColor = new Color(1f, 0.3f, 0.3f);     // 빨강

        Color gaugeColor;

        if (fill < 0.5f)
            gaugeColor = Color.Lerp(lowColor, midColor, fill * 2f); // 0~0.5
        else
            gaugeColor = Color.Lerp(midColor, fullColor, (fill - 0.5f) * 2f); // 0.5~1

        tmpScoreGaugeImage.color = gaugeColor;
    }
}

