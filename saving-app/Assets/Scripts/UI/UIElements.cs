using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElements : Singleton<UIElements>
{
    [Header("Earnings")]
    public Color earningsColorDark;
    public Color earningsColorBright;
    public Sprite earningsLogo;
    [Header("Expenses")]
    public Color expensesColorDark;
    public Color expensesColorBright;
    public Sprite expensesLogo;
    [Header("Savings")]
    public Color savingsColorDark;
    public Color savingsColorBright;
    public Sprite savingsLogo;

    public Color GetBrightColor(CashFlowTypes cashFlowType)
    {
        switch(cashFlowType)
        {
            case CashFlowTypes.Earnings:
                return earningsColorBright;
            case CashFlowTypes.Expenses:
                return expensesColorBright;
            case CashFlowTypes.Savings:
                return savingsColorBright;
            default:
                throw new System.Exception("Non implemented cashflow type");
        }
    }

    public Color GetDarkColor(CashFlowTypes cashFlowType)
    {
        switch (cashFlowType)
        {
            case CashFlowTypes.Earnings:
                return earningsColorDark;
            case CashFlowTypes.Expenses:
                return expensesColorDark;
            case CashFlowTypes.Savings:
                return savingsColorDark;
            default:
                throw new System.Exception("Non implemented cashflow type");
        }
    }

    public Sprite GetLogo(CashFlowTypes cashFlowType)
    {
        switch (cashFlowType)
        {
            case CashFlowTypes.Earnings:
                return earningsLogo;
            case CashFlowTypes.Expenses:
                return expensesLogo;
            case CashFlowTypes.Savings:
                return savingsLogo;
            default:
                throw new System.Exception("Non implemented cashflow type");
        }
    }
}
