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
    private float killMaxScore = 2f; // KillAll 활성화 점수
    
    public Button killAllButton;
    public GameObject gameOverPanel; // UI에 패널 연결
    public Image tmpScoreGaugeImage;


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
        killAllButton.gameObject.SetActive(false); // ← 버튼을 아예 숨김 상태로 시작
        killAllButton.onClick.AddListener(KillAllEnemies);
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        tmpScore += amount; 
        tmpScoreGaugeImage.fillAmount = tmpScore / killMaxScore;
        
        UpdateScoreUI();
        if (tmpScore >= killMaxScore && !killAllButton.gameObject.activeSelf)
            killAllButton.gameObject.SetActive(true);
            killAllButton.interactable = true; // ★ 버튼 색 회복 보장
            // ✅ 버튼 색상 갱신 유도
            EventSystem.current.SetSelectedGameObject(null);
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
        tmpScoreGaugeImage.fillAmount = tmpScore; 
        killAllButton.gameObject.SetActive(false);
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
}

