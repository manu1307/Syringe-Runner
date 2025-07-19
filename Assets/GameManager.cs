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

    private int score = 0;         // 총 점수 (시민+적)
    private int curedCount = 0;    // 시민 치료 수
    private int tmpScore = 0;      // KillAll 버튼용 점수
    private float killMaxScore = 5f; // KillAll 활성화 점수
    private Coroutine pulseRoutine;
    private bool hasContinued = false;  // 1회만 이어하기 허용
    private Vector3 safeRespawnPoint;
    
    public Button killAllButton;
    public GameObject gameOverPanel; // UI에 패널 연결
    public Image tmpScoreGaugeImage;
    public Transform gaugeWrapperTransform; 
    
    public TMP_Text currentScoreText;
    public TMP_Text bestScoreText;

    private int bestScore = 0;
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
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
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
        
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }
        
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
            scoreText.text = $"치료: {curedCount}명"; // ✅ 시민 수 따로 표시

        if (currentScoreText != null)
            currentScoreText.text = $"점수: {score}";

        if (bestScoreText != null)
            bestScoreText.text = $"최고: {bestScore}";
    }

    public void GameOver()
    {
        Debug.Log("게임 종료");

        ShowGameOverPanel();
    }
    
    public void ShowGameOverPanel()
    {
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // 이어하기 버튼 활성화 여부 설정
        if (hasContinued)
        {
            // 이미 이어하기 한 적 있으면 버튼 숨기기
            Transform continueBtn = gameOverPanel.transform.Find("ContinueButton");
            if (continueBtn != null)
                continueBtn.gameObject.SetActive(false);
        }
        else
        {
            // 이어하기 버튼 보여주기
            Transform continueBtn = gameOverPanel.transform.Find("ContinueButton");
            if (continueBtn != null)
                continueBtn.gameObject.SetActive(true);
        }
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
    
    public void ReturnToHome()
    {
        Time.timeScale = 1f; // 혹시 멈춰있다면 다시 재개
        SceneManager.LoadScene("MainMenuScene"); // ← 시작 씬 이름으로 바꿔주세요
    }
    
    public void AddCuredCitizen()
    {
        curedCount++;
        AddScore(1);  // 점수도 1점 추가
        UpdateScoreUI();
    }
    // 테스트용
    public void OnClickContinueButton()
    {
        if (hasContinued) return;

        hasContinued = true;

        // 패널 닫고 시간 재개
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;

        // 플레이어 위치 복귀
        PlayerController.Instance.transform.position = safeRespawnPoint;

        // 잠시 무적 처리
        StartCoroutine(TemporaryInvincibility());

        // 이어하기 버튼 숨기기
        Transform continueBtn = gameOverPanel.transform.Find("ContinueButton");
        if (continueBtn != null)
            continueBtn.gameObject.SetActive(false);
    }
    public void TryContinue()
    {
        if (hasContinued) return;
        ContinueGameAfterAd();
        // 광고 플랫폼에 따라 달라짐. 예시는 Unity Ads
        // if (Advertisement.IsReady("rewardedVideo"))
        // {
        //     Advertisement.Show("rewardedVideo", new ShowOptions
        //     {
        //         resultCallback = result =>
        //         {
        //             if (result == ShowResult.Finished)
        //             {
        //                 ContinueGameAfterAd();
        //             }
        //             else
        //             {
        //                 Debug.Log("광고 실패 또는 스킵됨");
        //             }
        //         }
        //     });
        // }
    }
    
    void ContinueGameAfterAd()
    {
        hasContinued = true;

        // 게임 재개
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);

        // 이어할 위치로 되돌리기 (예: Player 원래 위치 or 안전한 위치)
        PlayerController.Instance.transform.position = safeRespawnPoint;

        // 주변 좀비 제거 또는 잠시 무적
        StartCoroutine(TemporaryInvincibility());
    }
    
    IEnumerator TemporaryInvincibility()
    {
        SpriteRenderer sr = PlayerController.Instance.GetComponent<SpriteRenderer>();
        Collider2D col = PlayerController.Instance.GetComponent<Collider2D>();

        col.enabled = false;

        float t = 0f;
        while (t < 2f)
        {
            t += 0.2f;
            sr.enabled = !sr.enabled; // 깜빡임
            yield return new WaitForSeconds(0.2f);
        }

        sr.enabled = true;
        col.enabled = true;
    }
    public void SetSafeRespawnPoint(Vector3 position)
    {
        safeRespawnPoint = position;
    }

    
}

