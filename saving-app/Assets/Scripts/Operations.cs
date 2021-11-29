using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Scripts.ReadWrite.Objects;

public class Operations : MonoBehaviour
{
    // ************************** TOTAL **************************
    public static float GetAvailableCash()
    {
        Dictionary<CashFlowTypes, float> result = GetTotalCashFlows();

        result.TryGetValue(CashFlowTypes.Earnings, out float earnings);
        result.TryGetValue(CashFlowTypes.Expenses, out float expenses);
        result.TryGetValue(CashFlowTypes.Savings, out float savings);

        return earnings - expenses - savings;
    }

    public static float GetTotalCashFlowPercentage(CashFlowTypes cashFlowType)
    {
        if (cashFlowType == CashFlowTypes.Earnings)
            throw new System.Exception("You should not ask for the percentage of earnings");

        Dictionary<CashFlowTypes, float> result = GetTotalCashFlows();

        result.TryGetValue(CashFlowTypes.Earnings, out float earnings);
        result.TryGetValue(cashFlowType, out float cashflow);

        if (earnings == 0)
            return 0;

        return cashflow / earnings;
    }

    private static Dictionary<CashFlowTypes, float> GetTotalCashFlows()
    {
        // Compute total cashflow
        Dictionary<CashFlowTypes, float> result = Engine.CashFlows.Values.GroupBy(i => i.typeId)
            .Select(i => new
            {
                typeId = i.Key,
                value = i.Where(j => j.userId == Engine.UserId
                    ).Sum(k => k.value)
            }
            ).ToDictionary(i => (CashFlowTypes)i.typeId, i => i.value);

        return result;
    }

    public static float GetTotalCashFlows(CashFlowTypes cashFlowType)
    {
        Dictionary<CashFlowTypes, float> result = GetTotalCashFlows();

        result.TryGetValue(cashFlowType, out float res);
        return res;
    }

    // ************************** YEARLY **************************
    public static Dictionary<CashFlowTypes, float> GetYearlyOrMonthlyCashFlowsTotal(System.DateTime datetime, bool isMonthly = false)
    {
        // get either month or year
        string dateFormat = (isMonthly) ? "yyyy/MM" : "yyyy";

        // compute cashflows
        Dictionary<CashFlowTypes, float> result = Engine.CashFlows.Values.GroupBy(i => i.typeId)
            .Select(i => new
            {
                typeId = i.Key,
                value = i.Where(
                    j => j.date.ToString(dateFormat) == datetime.ToString(dateFormat)
                    && j.userId == Engine.UserId
                    ).Sum(k => k.value)
            }
            ).ToDictionary(i => (CashFlowTypes)i.typeId, i => i.value);

        return result;
    }

    public static float GetYearlyOrMonthlyCashFlowsTotal(System.DateTime datetime, CashFlowTypes cashFlowType, bool isMonthly = false)
    {
        GetYearlyOrMonthlyCashFlowsTotal(datetime, isMonthly).TryGetValue(cashFlowType, out float result);
        return result;
    }

    public static List<CashFlow> GetYearlyOrMonthlyAllCashFlowsTransactions(System.DateTime datetime, bool isMonthly = false)
    {
        // get either month or year
        string dateFormat = (isMonthly) ? "yyyy/MM" : "yyyy";

        return Engine.CashFlows.Values.
            Where(
                    i => i.userId == Engine.UserId 
                    && i.date.ToString(dateFormat) == datetime.ToString(dateFormat)
            ).
            OrderBy(i => i.date).
            ToList();
    }

    public static Dictionary<int, float> GetYearlyOrMonthlyCashFlowsSubtypes(System.DateTime datetime, CashFlowTypes cashFlowTypes, bool isMonthly = false)
    {
        // get either month or year
        string dateFormat = (isMonthly) ? "yyyy/MM" : "yyyy";

        // compute cashflows
        Dictionary<int, float> result = Engine.CashFlows.Values.GroupBy(i => i.subtypeId)
            .Select(i => new
            {
                subtypeId = i.Key,
                value = i.Where(
                    j => j.date.ToString(dateFormat) == datetime.ToString(dateFormat)
                    && j.userId == Engine.UserId
                    && j.typeId == (int) cashFlowTypes
                    ).Sum(k => k.value)
            }
            ).ToDictionary(i => i.subtypeId, i => i.value);

        return result;
    }

    public static float GetYearlyOrMonthlyCashFlowsSubtypes(System.DateTime datetime, CashFlowTypes cashFlowType, int cashFlowSubtype, bool isMonthly = false)
    {
        GetYearlyOrMonthlyCashFlowsSubtypes(datetime, cashFlowType, isMonthly).TryGetValue(cashFlowSubtype, out float result);
        return result;
    }





}
