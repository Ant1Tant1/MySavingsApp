
namespace Scripts.ReadWrite.Objects
{ 
    [System.Serializable]
    public class Savings
    {
        public string userId;
        public System.DateTime date;
        public int subtypeId;
        public float value;
        public float actual_value;

        public Savings()
        {
            this.userId = "";
            this.date = System.DateTime.Now;
            this.subtypeId = -1;
            this.value = -1;
            this.actual_value = -1;
        }

        public Savings(string userId, System.DateTime date, int subtypeId, float value, float actual_value)
        {
            this.userId = userId;
            this.date = date;
            this.subtypeId = subtypeId;
            this.value = value;
            this.actual_value = actual_value;
        }

        public Savings(string userId, string date, int subtypeId, float value, float actual_value)
        {
            this.userId = userId;
            this.date = System.DateTime.Parse(date);
            this.subtypeId = subtypeId;
            this.value = value;
            this.actual_value = actual_value;
        }
    }
}
