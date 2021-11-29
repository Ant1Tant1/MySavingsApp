
namespace Scripts.ReadWrite.Objects
{
    [System.Serializable]
    public class Users
    {
        public string userId;
        public string email;
        public string userName;
        public string password;


        private const int NB_OF_LETTERS_FOR_IDS = 3;
        private const int NB_OF_CHAR_FOR_IDS = 4;
        public Users()
        {
            this.userId = "";
            this.email = "";
            this.userName = "";
            this.password = "";
        }

        public Users(string id, string email, string userName, string password)
        {
            this.userId = id;
            this.email = email;
            this.userName = userName;
            this.password = password;
        }

        public Users(string userName)
        {
            this.userId = CreateNewId();
            this.email = "";
            this.userName = userName;
            this.password = userName + System.DateTime.Now.Year.ToString(); // default password
        }

        // Create a random ID
        // ID follow the template   -->    AAA 0000 0000    <---
        public static string CreateNewId()
        {
            string userId = "";

            // set up a random generator
            System.Random random = new System.Random();

            // Letters
            for (int i = 0; i < NB_OF_LETTERS_FOR_IDS; i++)
            {
                // random uppercase letter
                int rd = random.Next(0, 26);
                char ch = (char)('A' + rd);
                userId += ch;
            }

            // Add a space
            userId += " ";

            // Numbers
            for (int i = 0; i < NB_OF_CHAR_FOR_IDS; i++)
            {
                // random number
                int rd = random.Next(0, 9);
                userId += rd.ToString();
            }

            // Add a space
            userId += " ";

            for (int i = 0; i < NB_OF_CHAR_FOR_IDS; i++)
            {
                // random number
                int rd = random.Next(0, 9);
                userId += rd.ToString();
            }

            return userId;
        }
    }
}
