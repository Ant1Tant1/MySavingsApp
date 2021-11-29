using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : Singleton<SaveData>
{
    // counters
    private int counterCashFlows = 0;
    private int counterSavings = 0;
    private const int COUNTER_MAX_VALUE = 10;

    private void Start()
    {
        // Subscribe
        Subscribe();
    }

    private void Subscribe()
    { 
        // Subscribe to event
        Engine.DataUpdated += SaveCashFlowData;
    }

    private void Unsubscribe()
    {
        Engine.DataUpdated -= SaveCashFlowData;
    }


    public void SaveCashFlowData()
    {
        // increase counter
        counterCashFlows += 1;

        // save to json
        if (counterCashFlows >= COUNTER_MAX_VALUE)
        {
            Engine.Save(Engine.CashFlows.Values, SerializationTypes.CashFlows);
            counterCashFlows = 0;
        }
    }


    private void OnApplicationQuit()
    {
        // unsubscribe
        Unsubscribe();

        // check whether things needs to be saved
        if (counterCashFlows != 0)
            Engine.Save(Engine.CashFlows.Values, SerializationTypes.CashFlows);

        if (counterSavings != 0)
            Engine.Save(Engine.Savings, SerializationTypes.Savings);

        // save subcategories
        Engine.Save(Engine.CashFlowSubCategories, SerializationTypes.CashFlowSubCategories);
    }

}
