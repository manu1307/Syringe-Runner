using UnityEngine;

public class HowToPlayPanelController : MonoBehaviour
{
    public GameObject howToPlayPanel;

    public void ShowPanel()
    {
        howToPlayPanel.SetActive(true);
    }

    public void HidePanel()
    {
        howToPlayPanel.SetActive(false);
    }
}