using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class YearlyOrMonthlyCashFlowsDetailsContainer : MonoBehaviour
{
    [SerializeField]
    private CashFlowTypes cashFlowType;

    [SerializeField]
    private GameObject cashflowsSummaryPrefab;

    private Dictionary<int, YearlyOrMonthlyCashFlowSummaryPrefab> cashflowsSummaryPrefabDict = new Dictionary<int, YearlyOrMonthlyCashFlowSummaryPrefab>();
    private Dictionary<int, string> cashflowSubCategoryDict;

    // Start is called before the first frame update
    void Start()
    {
        // ensure cashflow type is not earnings
        if (cashFlowType == CashFlowTypes.Earnings)
            throw new System.Exception("Earnings cannot be a cashflow type here");

        // setup and creates prefabs
        Setup();

        // Subscribe to event
        DatesContainer.DateRelatedDataUpdate += UpdateYearlyCashFlowValues;
    }

    private void OnDisable()
    {
        DatesContainer.DateRelatedDataUpdate -= UpdateYearlyCashFlowValues;
        DestroyPrefabs();
    }

    private void DestroyPrefabs()
    {
        foreach (KeyValuePair<int, YearlyOrMonthlyCashFlowSummaryPrefab> keyValuePair in cashflowsSummaryPrefabDict)
        {
            Destroy(keyValuePair.Value.gameObject);
        }
        cashflowsSummaryPrefabDict.Clear();
    }

    // Update prefabs values
    private void UpdateYearlyCashFlowValues()
    {
        // get values by subcaterory integeter
        Dictionary<int, float> dict = Operations.GetYearlyOrMonthlyCashFlowsSubtypes(DatesContainer.DateDisplayed, cashFlowType, DatesContainer.IsMonthly);
        float yearToDateCashflows = dict.Sum(i => i.Value);

        foreach (KeyValuePair<int, YearlyOrMonthlyCashFlowSummaryPrefab> keyValuePair in cashflowsSummaryPrefabDict)
        {
            dict.TryGetValue(keyValuePair.Key, out float value);

            keyValuePair.Value.Setup(cashflowSubCategoryDict[keyValuePair.Key], value, value / yearToDateCashflows);
        }
    }

    // Setup and creates objects
    private void Setup()
    {
        // get values by subcaterory integeter
        Dictionary<int, float> dict = Operations.GetYearlyOrMonthlyCashFlowsSubtypes(DatesContainer.DateDisplayed, cashFlowType, DatesContainer.IsMonthly);
        float yearToDateCashflows = dict.Sum(i => i.Value);

        // get dictionnary of keys from engine class
        cashflowSubCategoryDict = Engine.CashFlowSubCategories.GetRelevantDict(cashFlowType);

        foreach (int i in cashflowSubCategoryDict.Keys)
        {
            dict.TryGetValue(i, out float value);

            InstantiatePrefabs(i, value, value / yearToDateCashflows);
        }
    }

    // instantiate Prefabs
    private void InstantiatePrefabs(int subcategory, float yearToDateValue, float yearToDatePercentage)
    {
        GameObject gameObject = Instantiate(cashflowsSummaryPrefab, transform);
        cashflowsSummaryPrefabDict[subcategory] = gameObject.GetComponent<YearlyOrMonthlyCashFlowSummaryPrefab>();
        cashflowsSummaryPrefabDict[subcategory].Setup(cashflowSubCategoryDict[subcategory], yearToDateValue, yearToDatePercentage);
    }
}
