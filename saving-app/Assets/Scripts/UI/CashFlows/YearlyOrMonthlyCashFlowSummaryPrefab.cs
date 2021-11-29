using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class YearlyOrMonthlyCashFlowSummaryPrefab : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currencySymbol;
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI valueText;
    [SerializeField]
    private Slider slider;

    private void Start()
    {
        // set currency symbol
        currencySymbol.text = PlayerPrefs.GetString("currencySymbol", "$");
    }

    // Start is called before the first frame update
    public void Setup(string savingName, float yearToDatevalue, float yearToDatePercentage)
    {
        title.text = savingName;
        valueText.text = yearToDatevalue.ToString("### ##0");

        if (float.IsNaN(yearToDatePercentage))
            slider.value = 0;
        else
            slider.value = yearToDatePercentage;
    }
}
