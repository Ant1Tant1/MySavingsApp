using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Public Enumerations

// Scenes
public enum Scenes
{
    Login = 0, 
    Dashboard = 1,
    Savings = 2,
    Expenses = 3,
    Settings = 4,
    Cashflows = 5,
}


// Serialization types are the type of "Tables" that will be saved in JSON
public enum SerializationTypes
{
    Users = 0,
    CashFlows = 1,
    Savings = 2,
    CashFlowSubCategories = 3,
}

// Three basics cashflow types
public enum CashFlowTypes
{
    Earnings = 0,
    Expenses = 1,
    Savings = 2
}

public enum ReadWriteMethod
{
    Json = 0,
    DB = 1,
    API = 2,
}