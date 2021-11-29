using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TotalCashFlowContainer : MonoBehaviour
{
    [SerializeField]
    private CashFlowTypes cashFlowType;
    [SerializeField]
    private TextMeshProUGUI currencySymbol;
    [SerializeField]
    private TextMeshProUGUI accountValue;
    [SerializeField]
    private TextMeshProUGUI percentageValue;
    [SerializeField]
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        // ensure cashflow type is not earnings
        if (cashFlowType == CashFlowTypes.Earnings)
            throw new System.Exception("Earnings cannot be a cashflow type here");

        // set currency symbol
        currencySymbol.text = PlayerPrefs.GetString("currencySymbol", "$");

        // Update value
        UpdateValue();
    }

    private void UpdateValue()
    {
        float value = Operations.GetTotalCashFlows(cashFlowType);
        float perc = Operations.GetTotalCashFlowPercentage(cashFlowType);

        // set accountValue
        accountValue.text = value.ToString("# ### ##0");

        // set percentage values
        percentageValue.text = perc.ToString("##0.0 %");
        slider.value = perc;
    }
}
