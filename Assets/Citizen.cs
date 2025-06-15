using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour
{ 
   
    public int scoreValue = 10; // 치료 시 얻는 점수
    
    public float moveSpeed = 1.5f;
    private Vector2 moveDir;

    public float directionChangeInterval = 3f;
    private float timer;

    private Camera cam;
    private float camWidth, camHeight;
    
    private SpriteRenderer spriteRenderer;  // flipX용
    void Start()
    {
        cam = Camera.main;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 가져오기
        
        ChooseNewDirection();
    }
    void Update()
    {
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);

        
        // 좌우 이동 방향에 따라 스프라이트 반전
        if (moveDir.x > 0.1f)
            spriteRenderer.flipX = false;
        else if (moveDir.x < -0.1f)
            spriteRenderer.flipX = true;

        timer += Time.deltaTime;
        if (timer >= directionChangeInterval)
        {
            ChooseNewDirection();
            timer = 0f;
        }
        CheckBoundsAndReflect();
    }
    
    void ChooseNewDirection()
    {
        // 랜덤 방향: X는 -1~1, Y는 -0.2~(-1)로 살짝 아래쪽 유지
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, -0.2f);
        moveDir = new Vector2(x, y).normalized;
    }
    void CheckBoundsAndReflect()
    {
        Vector3 pos = transform.position;
        float halfW = camWidth / 2f;
        float halfH = camHeight / 2f;

        bool reflected = false;

        // 왼쪽/오른쪽 벽
        if (pos.x < cam.transform.position.x - halfW || pos.x > cam.transform.position.x + halfW)
        {
            float newX = -Mathf.Sign(moveDir.x) * Random.Range(0.3f, 1f);
            float newY = Random.Range(-1f, 1f);
            moveDir = new Vector2(newX, newY).normalized;
            reflected = true;
        }

        // 위/아래 벽
        if (pos.y < cam.transform.position.y - halfH || pos.y > cam.transform.position.y + halfH)
        {
            float newX = Random.Range(-1f, 1f);
            float newY = -Mathf.Sign(moveDir.y) * Random.Range(0.3f, 1f);
            moveDir = new Vector2(newX, newY).normalized;
            reflected = true;
        }

        if (reflected)
        {
            timer = 0f; // 방향 바뀌었으니 타이머 초기화
        }
    }
    public void OnPlayerTouch()
    {
        bool success = VaccineManager.Instance.UseVaccine(1);
        if (success)
        {
            GameManager.Instance.AddScore(scoreValue); // 점수 증가
            Destroy(gameObject); // 시민 제거
        }
        else
        {
            // PlayerController.Instance.TriggerPenalty(); // 실패 시 페널티
        }
    }
}

