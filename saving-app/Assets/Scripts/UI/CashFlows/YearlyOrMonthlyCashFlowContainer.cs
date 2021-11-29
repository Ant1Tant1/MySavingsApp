using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class YearlyOrMonthlyCashFlowContainer : MonoBehaviour
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
    [SerializeField]
    private TextMeshProUGUI titleText;

    // Start is called before the first frame update
    void Start()
    {
        // ensure cashflow type is not earnings
        if (cashFlowType == CashFlowTypes.Earnings)
            throw new System.Exception("Earnings cannot be a cashflow type here");

        // set currency symbol
        currencySymbol.text = PlayerPrefs.GetString("currencySymbol", "$");

        // Subscribe to event
        DatesContainer.DateRelatedDataUpdate += UpdateValue;

        // Update value
        UpdateValue();
    }

    private void OnDisable()
    {
        DatesContainer.DateRelatedDataUpdate -= UpdateValue;
    }

    private void UpdateValue()
    {
        // update title
        if (DatesContainer.IsMonthly)
            titleText.text = "Month " + cashFlowType.ToString();
        else
            titleText.text = "Year " + cashFlowType.ToString();

        // get year or month savings
        Dictionary<CashFlowTypes, float> result = Operations.GetYearlyOrMonthlyCashFlowsTotal(DatesContainer.DateDisplayed, DatesContainer.IsMonthly);

        result.TryGetValue(CashFlowTypes.Earnings, out float earnings);
        result.TryGetValue(cashFlowType, out float cashflow);

        // set account value
        accountValue.text = cashflow.ToString("# ### ##0");

        // set percentage
        if (earnings == 0)
        {
            // set accountValue
            percentageValue.text = "N.A";
            slider.value = 0f;
        }
        else
        {
            percentageValue.text = (cashflow / earnings).ToString("##0.0 %");
            slider.value = cashflow / earnings;
        }
    }
}
