using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SettingsUI : MonoBehaviour
{
    public GameObject settingsPanel;

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;   // ⏸ หยุดเวลาเกม
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;   // ▶ เล่นต่อ
    }
}
