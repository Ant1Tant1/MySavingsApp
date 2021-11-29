using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CashflowsComparison : MonoBehaviour
{
    [SerializeField]
    private CashFlowTypes cashFlowType;
    [SerializeField]
    private GameObject barPrefab;

    private const int NB_BARS = 4;
    private BarPrefab[] barPrefabArray = new BarPrefab[NB_BARS];
    private float[] values = new float[NB_BARS];
    private string[] dates = new string[NB_BARS];

    // Start is called before the first frame update
    void Start()
    {
        // ensure cashflow type is not earnings
        if (cashFlowType == CashFlowTypes.Earnings)
            throw new System.Exception("Earnings cannot be a cashflow type here");

        // Subscribe to event
        DatesContainer.DateRelatedDataUpdate += UpdateValue;

        // Update value
        CreatePrefabs();
        UpdateValue();
    }

    private void OnDisable()
    {
        DatesContainer.DateRelatedDataUpdate -= UpdateValue;
    }

    private void CreatePrefabs()
    {
        for (int i = 0; i < NB_BARS; i++)
        {
            GameObject gameObject = Instantiate(barPrefab, transform);
            barPrefabArray[i] = gameObject.GetComponent<BarPrefab>();
        }
    }

    private void DestroyPrefabs()
    {
        for (int i = 0; i < NB_BARS; i++)
        {
            Destroy(barPrefabArray[i].gameObject);
        }
        barPrefabArray = null;
    }


    private void UpdateValue()
    {
        // get values 
        for (int i = 0; i < NB_BARS; i++)
        {
            // get relevant date
            System.DateTime date;
            if (DatesContainer.IsMonthly)
            {
                date = DatesContainer.DateDisplayed.AddMonths(-NB_BARS + 1 + i);
                dates[i] = date.ToString("MMM");
            }
            else
            {
                date = DatesContainer.DateDisplayed.AddYears(-NB_BARS + 1 + i);
                dates[i] = date.ToString("yyyy");
            }

            // get year or month savings
            Dictionary<CashFlowTypes, float> result = Operations.GetYearlyOrMonthlyCashFlowsTotal(date, DatesContainer.IsMonthly);
            result.TryGetValue(CashFlowTypes.Expenses, out float val);
            values[i] = val;
        }

        float max = values.Max();

        // update prefabs
        for (int i = 0; i < NB_BARS; i++)
        {
            barPrefabArray[i].Setup(dates[i], values[i], values[i] / max);
        }
    }
}
