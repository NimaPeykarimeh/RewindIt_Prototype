using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelManager : MonoBehaviour
{
    [SerializeField] Slider xSensitivitySlider;
    [SerializeField] Slider ySensitivitySlider;

    [Space]

    [SerializeField] TextMeshProUGUI xSenText;
    [SerializeField] TextMeshProUGUI ySenText;

    [SerializeField] int xSenValue;
    [SerializeField] int ySenValue;
    
    private void Update()
    {
        ySensitivitySlider.onValueChanged.AddListener(delegate { ChangeSensitivity(); });
        xSensitivitySlider.onValueChanged.AddListener(delegate { ChangeSensitivity(); });
    }
    private void OnEnable()
    {
        xSensitivitySlider.value = SettingsManager.instance.xSenValue;
        ySensitivitySlider.value = SettingsManager.instance.ySenValue;

        xSenText.SetText(xSensitivitySlider.value.ToString());
        ySenText.SetText(ySensitivitySlider.value.ToString());
    }
    void ChangeSensitivity()
    {
        SettingsManager.instance.xSenValue = (int)xSensitivitySlider.value;
        SettingsManager.instance.ySenValue = (int)ySensitivitySlider.value;

        xSenText.SetText(xSensitivitySlider.value.ToString());
        ySenText.SetText(ySensitivitySlider.value.ToString());
    }
}
