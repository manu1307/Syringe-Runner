using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    // Update is called once per frame

    void Update()
    {
        if (player == null) return;
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
            gm.RemoveEnemy(this);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.GameOver(); // ✅ 게임 종료 처리
            }
        }
    }
}
