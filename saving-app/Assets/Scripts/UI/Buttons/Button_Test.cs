using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.ReadWrite.Objects;
using Scripts.ReadWrite.JSON;

public class Button_Test : MonoBehaviour
{
   public void OnClick()
    {
        ReadWriteJSON readWriteJSON = new ReadWriteJSON();

        // Create list of user
        List<Users> userList = new List<Users>();
        userList.Add(new Users("00", "", "antoine", "abcde"));
        userList.Add(new Users("01", "", "kim", "abcde"));

        // Create list of cashflows
        List<CashFlow> cashFlows = new List<CashFlow>();
        cashFlows.Add(new CashFlow(0, "00", (int)CashFlowTypes.Expenses, 0, 150, System.DateTime.Today));
        cashFlows.Add(new CashFlow(1, "01", (int)CashFlowTypes.Expenses, 2, 1000, System.DateTime.Today));

        // Create list of savings
        List<Savings> savings = new List<Savings>();
        savings.Add(new Savings("00", System.DateTime.Now, 1, 1000, 1150));
        savings.Add(new Savings("01", System.DateTime.Now, 0, 500, 1000));


        // Create list of savings
        CashFlowSubCategories cashFlowSubCategories = (new CashFlowSubCategories(
            new Dictionary<int, string>(){
                {0, "Questrade"},
                {1, "Bitcoin"},
                {2, "Ethereum"}
            },
            new Dictionary<int, string>(){
                {0, "Restaurant"},
                {1, "Clothes"},
                {2, "Groceries"},
                {3, "Tech"},
                {4, "Photography"},
                {5, "Holidays"}
            },
            new Dictionary<int, string>(){
                {0, "Work"},
                {1, "Other"},
                {2, "Resale"}
            }
            )
        );

        //readWriteJSON.Save<UsersJSON>(userList, SerializationTypes.Users);
        //readWriteJSON.Save<CashFlowJSON>(cashFlows, SerializationTypes.CashFlows);
        //readWriteJSON.Save<SavingsJSON>(savings, SerializationTypes.Savings);
        //readWriteJSON.Save<CashFlowSubCategoriesJSON>(cashFlowSubCategories, SerializationTypes.CashFlowSubCategories);


        List<Users> myUsers = null;
        cashFlowSubCategories = null;
        myUsers = readWriteJSON.ReadList<Users>(SerializationTypes.Users);
        cashFlowSubCategories = readWriteJSON.Read<CashFlowSubCategories>(SerializationTypes.CashFlowSubCategories);

        Debug.Log(myUsers[0].userName);
        Debug.Log(myUsers[1].userName);
        //Debug.Log(cashFlowSubCategories.expenses);
    }
}
