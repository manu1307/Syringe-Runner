using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider volumeSlider;
    private AudioSource bgm;

    void Start()
    {
        bgm = GameObject.Find("BGMPlayer")?.GetComponent<AudioSource>();
        if (bgm != null && volumeSlider != null)
        {
            volumeSlider.value = bgm.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void HideSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void SetVolume(float value)
    {
        if (bgm != null)
            bgm.volume = value;
    }
}