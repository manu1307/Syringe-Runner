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
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Awake()
    {
        Instance = this;
    }

    void FixedUpdate()  // ← Update 대신 FixedUpdate 사용
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = new Vector2(moveX, moveY).normalized;
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        // Sprite 반전
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
                GameManager.Instance.AddCuredCitizen();
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
}

