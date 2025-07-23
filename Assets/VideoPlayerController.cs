using UnityEngine;
using UnityEngine.Video;

public class BackgroundVideo : MonoBehaviour
{
    public VideoClip videoClip;

    void Start()
    {
        VideoPlayer vp = gameObject.AddComponent<VideoPlayer>();
        vp.clip = videoClip;
        vp.renderMode = VideoRenderMode.CameraFarPlane;
        vp.targetCamera = Camera.main; // 이게 핵심!
        vp.isLooping = true;
        vp.playOnAwake = true;
    }
}