
namespace Scripts.ReadWrite.Objects
{
    [System.Serializable]
    public class CashFlow
    {
        public int cashflowId;
        public string userId;
        public int typeId;
        public int subtypeId;
        public float value;
        public System.DateTime date;

        public CashFlow()
        {
            this.cashflowId = -1;
            this.userId = "";
            this.typeId = -1;
            this.subtypeId = -1;
            this.value = -1.0f;
            this.date = System.DateTime.Now;
        }

        public CashFlow(int cashflowId, string userId, int typeId, int subtypeId, float value, System.DateTime date)
        {
            this.cashflowId = cashflowId;
            this.userId = userId;
            this.typeId = typeId;
            this.subtypeId = subtypeId;
            this.value = value;
            this.date = date;
        }

        public CashFlow(int cashflowId, string userId, int typeId, int subtypeId, float value, string date)
        {
            this.cashflowId = cashflowId;
            this.userId = userId;
            this.typeId = typeId;
            this.subtypeId = subtypeId;
            this.value = value;
            this.date = System.DateTime.Parse(date);
        }
    }
}
