using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public float moveSpeed = 5f;
    public float penaltyDuration = 2f;
    // private bool isSlowed = false;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        // float currentSpeed = isSlowed ? moveSpeed * 0.5f : moveSpeed;

        Vector3 moveDir = new Vector3(moveX, moveY, 0).normalized;
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);

        // 좌우 이동 시 flipX 처리
        if (moveX > 0.1f)
            spriteRenderer.flipX = false;
        else if (moveX < -0.1f)
            spriteRenderer.flipX = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Citizen"))
        {
            // 백신이 있는지 확인
            if (VaccineManager.Instance.currentVaccine > 0)
            {
                VaccineManager.Instance.UseVaccine(1);

                Destroy(other.gameObject);
                GameManager.Instance.AddScore(1);
            }
            else
            {
                // ❗ 없으면 치료 실패 패널티 (예: 속도 감소 등)
                Debug.Log("백신 없음 → 치료 실패!");
                // 예시: PlayerController.Instance.TriggerPenalty();
            }
        }

        if (other.CompareTag("Vaccine"))
        {
            // 백신 상자는 VaccineBox.cs에서 처리
        }
    }

    // public void TriggerPenalty()
    // {
    //     if (!isSlowed)
    //         StartCoroutine(SlowDownTemporarily());
    // }
    //
    // private IEnumerator SlowDownTemporarily()
    // {
    //     isSlowed = true;
    //     yield return new WaitForSeconds(penaltyDuration);
    //     isSlowed = false;
    // }
}

