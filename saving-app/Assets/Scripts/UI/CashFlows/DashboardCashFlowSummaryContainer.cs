using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashboardCashFlowSummaryContainer : MonoBehaviour
{
    public GameObject cashFlowSummaryPrefab;

    private Dictionary<CashFlowTypes, DashboardCashFlowSummaryPrefab> cashFlowSummaryPrefabsDict = new Dictionary<CashFlowTypes, DashboardCashFlowSummaryPrefab>();

    // Start is called before the first frame update
    void Start()
    {
        // Setup
        InstantiateMonthlyCashFlows(DatesContainer.DateDisplayed);

        // Subscribe to event
        DatesContainer.DateRelatedDataUpdate += UpdateMonthlyCashFlowsValues;
        Engine.DataUpdated += UpdateMonthlyCashFlowsValues;
    }

    private void OnDisable()
    {
        DatesContainer.DateRelatedDataUpdate -= UpdateMonthlyCashFlowsValues;
        Engine.DataUpdated -= UpdateMonthlyCashFlowsValues;
    }

    // Create and set up prefabs values
    private void InstantiateMonthlyCashFlows(System.DateTime month)
    {
        // get values by cashflowtypes
        Dictionary<CashFlowTypes, float> dict = Operations.GetYearlyOrMonthlyCashFlowsTotal(month, DatesContainer.IsMonthly);


        foreach (CashFlowTypes cashFlowType in System.Enum.GetValues(typeof(CashFlowTypes)))
        {
            GameObject gameObject = Instantiate(cashFlowSummaryPrefab, transform);

            float value = 0;
            if (dict.ContainsKey(cashFlowType))
                value = dict[cashFlowType];

            cashFlowSummaryPrefabsDict[cashFlowType] = gameObject.GetComponent<DashboardCashFlowSummaryPrefab>();
            cashFlowSummaryPrefabsDict[cashFlowType].Setup(cashFlowType, value);
        }
    }

    // Destroy prefabs
    public void DestroyMonthlyCashFlows()
    {
        foreach (KeyValuePair<CashFlowTypes, DashboardCashFlowSummaryPrefab> keyValuePair in cashFlowSummaryPrefabsDict)
        {
            Destroy(keyValuePair.Value.gameObject);
        }
        cashFlowSummaryPrefabsDict.Clear();
    }

    // Update prefabs values only
    public void UpdateMonthlyCashFlowsValues()
    {
        // get values by cashflowtypes
        Dictionary<CashFlowTypes, float> dict = Operations.GetYearlyOrMonthlyCashFlowsTotal(DatesContainer.DateDisplayed, DatesContainer.IsMonthly);

        foreach (KeyValuePair<CashFlowTypes, DashboardCashFlowSummaryPrefab> keyValuePair in cashFlowSummaryPrefabsDict)
        {
            dict.TryGetValue(keyValuePair.Key, out float value);

            keyValuePair.Value.Setup(keyValuePair.Key, value);
        }
    }
}
