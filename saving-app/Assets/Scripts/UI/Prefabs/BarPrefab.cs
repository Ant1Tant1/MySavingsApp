using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BarPrefab : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI dateText;
    [SerializeField]
    private TextMeshProUGUI value;
    [SerializeField]
    private Slider slider;

    public void Setup(string datetime, float value, float sliderValue)
    {
        dateText.text = datetime;
        this.value.text = value.ToString("### ##0");
        this.slider.value = sliderValue;
    }
}
