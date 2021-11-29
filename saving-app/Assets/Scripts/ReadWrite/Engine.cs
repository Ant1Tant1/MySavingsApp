using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scripts.ReadWrite.JSON;
using Scripts.ReadWrite.Objects;

public delegate void UpdateEventHandler<T>(T obj);
public delegate void UpdateEventHandler();


public class Engine: MonoBehaviour
{
    // Events management
    public static event UpdateEventHandler DataUpdated;

    // user ID used during the whole session
    private static string userId;
    private static string userName;
    private static string mailAddress;

    // json serialized objects
    private static Dictionary<int, CashFlow> cashFlows;
    private static List<Savings> savings;
    private static CashFlowSubCategories cashFlowSubCategories;

    // read write json instance
    private static IReadWriteData readWrite;
    private static ReadWriteMethod readWriteMethod;


    // ********** PROPERTIES **********
    public static CashFlowSubCategories CashFlowSubCategories {
        get
        {
            if (cashFlowSubCategories == null)
                cashFlowSubCategories = Read<CashFlowSubCategories>(SerializationTypes.CashFlowSubCategories);

            return cashFlowSubCategories;
        } 
    }

    public static Dictionary<int, CashFlow> CashFlows {
        get
        {
            if (cashFlows == null)
            {
                try
                {
                    cashFlows = ReadList<CashFlow>(SerializationTypes.CashFlows).ToDictionary(i => i.cashflowId, i => i);
                }
                catch
                {
                    cashFlows = new Dictionary<int, CashFlow>();
                }
            }

            return cashFlows;
        }
    }

    public static List<Savings> Savings
    {
        get
        {
            if (savings == null)
                savings = ReadList<Savings>(SerializationTypes.Savings);

            return savings;
        }
    }

    public static string UserId {
        get
        {
            if (userId == null)
                return "Default User ID";
            return userId;
        }
        set => userId = value;
    }

    public static string UserName
    {
        get
        {
            if (userName == null)
                return "No user name";
            return userName;
        }
        set => userName = value;
    }
    
    public static string MailAddress
    {
        get
        {
            if (mailAddress == null)
                return "no_mail_address@its_missing.sad";
            return mailAddress;
        }
        set => mailAddress = value;
    }

    public static IReadWriteData ReadWrite {
        get
        {
            if (readWrite == null)
            {
                switch (readWriteMethod)
                {
                    case ReadWriteMethod.Json:
                        return new ReadWriteJSON();

                    case ReadWriteMethod.API:
                    case ReadWriteMethod.DB:
                    default:
                        return new ReadWriteJSON();
                }
            }
            return readWrite;
        }
    }

    // ********** METHODS FOR SAVING / READING **********
    public static void Save<T>(T obj, SerializationTypes serializationType)
    {
        bool success = ReadWrite.Save(obj, serializationType);
    }

    public static void Save<T>(List<T> objList, SerializationTypes serializationType)
    {
        bool success = ReadWrite.Save(objList, serializationType);
    }

    public static List<T> ReadList<T>(SerializationTypes serializationType) where T : new()
    {
        // Load list
        return ReadWrite.ReadList<T>(serializationType);
    }

    public static T Read<T>(SerializationTypes serializationType) where T : new()
    {
        // Load list
        return ReadWrite.Read<T>(serializationType);
    }

    public static string GetPath(SerializationTypes serializationType)
    {
        return ReadWrite.GetPath(serializationType);
    }

    // ******************** EVENTS RELATED METHODS ********************
    public static void UpdateCashFlows()
    {
        DataUpdated?.Invoke();
    }
}
