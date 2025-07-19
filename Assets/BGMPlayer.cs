using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    private static BGMPlayer Instance;
    private AudioSource audioSource;
    void Awake()
    {
        // 중복 방지
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);  // 씬 전환 시에도 파괴되지 않게 함

        audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}