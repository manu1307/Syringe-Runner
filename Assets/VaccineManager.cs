using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VaccineManager : MonoBehaviour
{
    public static VaccineManager Instance;

    [Header("Vaccine Settings")]
    public int maxVaccine = 10;
    public int currentVaccine = 0;

    [Header("UI")]
    public TMP_Text vaccineText; // 백신 수 표시 텍스트
    // public Image vaccineBar; // 백신 게이지 바 (optional)

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddVaccine(int amount)
    {
        currentVaccine = Mathf.Min(currentVaccine + amount, maxVaccine);
        UpdateUI();
    }

    public bool UseVaccine(int amount)
    {
        if (currentVaccine >= amount)
        {
            currentVaccine -= amount;
            UpdateUI();
            return true;
        }
        else
        {
            // 실패 처리 가능: 속도 감소, 경고 등
            return false;
        }
    }

    private void UpdateUI()
    {
        if (vaccineText != null)
            vaccineText.text = $"{currentVaccine} / {maxVaccine}";

        // if (vaccineBar != null)
        //     vaccineBar.fillAmount = (float)currentVaccine / maxVaccine;
    }
}
