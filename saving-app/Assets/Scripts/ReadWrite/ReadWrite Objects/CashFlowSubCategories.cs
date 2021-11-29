using System.Collections.Generic;

namespace Scripts.ReadWrite.Objects
{
    [System.Serializable]
    public class CashFlowSubCategories
    {
        public const int NB_MAX_OF_CATEGORIES = 5;

        //private const int NB_MAX_OF_EARNINGS_CATEGORIES = 5;
        //private const int NB_MAX_OF_EXPENSES_CATEGORIES = 5;
        //private const int NB_MAX_OF_SAVINGS_CATEGORIES = 5;

        private Dictionary<int, string> earnings;
        private Dictionary<int, string> expenses;
        private Dictionary<int, string> savings;

        public CashFlowSubCategories()
        {
            this.earnings = new Dictionary<int, string>();
            this.expenses = new Dictionary<int, string>();
            this.savings = new Dictionary<int, string>();

        }

        public CashFlowSubCategories(Dictionary<int, string> savings, Dictionary<int, string> expenses, Dictionary<int, string> earnings)
        {
            this.earnings = new Dictionary<int, string>(earnings);
            this.expenses = new Dictionary<int, string>(expenses);
            this.savings = new Dictionary<int, string>(savings);
        }

        public CashFlowSubCategories(Dictionary<string, string> savings, Dictionary<string, string> expenses, Dictionary<string, string> earnings)
        {
            this.earnings = new Dictionary<int, string>((IDictionary<int, string>)earnings);
            this.expenses = new Dictionary<int, string>((IDictionary<int, string>)expenses);
            this.savings = new Dictionary<int, string>((IDictionary<int, string>)savings);
        }

        public Dictionary<int, string> Earnings { get => earnings; }
        public Dictionary<int, string> Expenses { get => expenses; }
        public Dictionary<int, string> Savings { get => savings; }

        public Dictionary<int, string> GetRelevantDict(CashFlowTypes cashFlowType)
        {
            switch(cashFlowType)
            {
                case CashFlowTypes.Earnings:
                    return new Dictionary<int, string>(this.Earnings);
                case CashFlowTypes.Expenses:
                    return new Dictionary<int, string>(this.Expenses);
                case CashFlowTypes.Savings:
                    return new Dictionary<int, string>(this.Savings);
                default:
                    throw new System.Exception("Non Implemented Cashflow Type");
            }
        }

        public void SetRelevantDict(CashFlowTypes cashFlowType, Dictionary<int, string> dict)
        {
            switch (cashFlowType)
            {
                case CashFlowTypes.Earnings:
                    earnings = new Dictionary<int, string>(dict);
                    break;
                case CashFlowTypes.Expenses:
                    expenses = new Dictionary<int, string>(dict);
                    break;
                case CashFlowTypes.Savings:
                    savings = new Dictionary<int, string>(dict);
                    break;
                default:
                    throw new System.Exception("Non Implemented Cashflow Type");
            }
        }
    }
}
