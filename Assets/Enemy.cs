using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    private Transform player;
    public GameObject explosionEffectPrefab;
    public AudioClip explosionSound;  // ✅ 폭발 사운드 클립
    private AudioSource audioSource;
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
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = 1f; // ✅ 이 위치에 넣어주세요
    }
    public void Kill()
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
        // 폭발 사운드
        if (explosionSound != null)
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        
        // 점수 추가
        GameManager.Instance.AddScore(5);
        
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
            gm.SetSafeRespawnPoint(other.transform.position);
            gm.GameOver();
        }
    }

}
