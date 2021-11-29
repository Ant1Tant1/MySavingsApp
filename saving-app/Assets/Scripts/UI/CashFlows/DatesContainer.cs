using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DatesContainer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI yearMonth;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private Button lastButton;
    [SerializeField]
    private Toggle yearMonthToggle;

    // events management
    public static event UpdateEventHandler DateRelatedDataUpdate;


    private static System.DateTime dateDisplayed; 
    private const int MAX_TIME_BEFORE_TODAY = 5;

    private static bool? isMonthly = null;

    public static bool IsMonthly
    {
        get
        {
            if (isMonthly == null)
                isMonthly = Convert.ToBoolean(PlayerPrefs.GetInt("IsMonthly", 0));
            return (bool)isMonthly;
        }
    }

    // static variables seem to be preserved in between instances so DateDisplayed is used
    // keep the displayed date in between scenes
    public static DateTime DateDisplayed {
        get
        {
            if (System.DateTime.Equals(dateDisplayed, new System.DateTime()))
                dateDisplayed = System.DateTime.Today;
            return dateDisplayed;
        }
        set
        {
            // get either month or year
            string dateFormat = (IsMonthly) ? "yyyy/MM" : "yyyy";

            if (dateDisplayed.ToString(dateFormat) == value.ToString(dateFormat))
            {
                // Debug.Log("Do nothing if this is the same date as been reset");
                return;
            }
            
            dateDisplayed = value;
            DateRelatedDataUpdate?.Invoke();
        }
    }

    public void OnTodayClick()
    {
        DateDisplayed = System.DateTime.Today;
    }

    public void OnNextYearClick()
    {
        if (IsMonthly)
            DateDisplayed = dateDisplayed.AddMonths(1);
        else
            DateDisplayed = dateDisplayed.AddYears(1);
    }

    public void OnLastYearClick()
    {
        if (IsMonthly)
            DateDisplayed = dateDisplayed.AddMonths(-1);
        else
            DateDisplayed = dateDisplayed.AddYears(-1);
    }

    // Start is called before the first frame update
    void Awake()
    {
        // satic varialbes like IsMonthly seem to be preserved in between instances so it will be used
        // to keep the state of the toggle
        yearMonthToggle.isOn = IsMonthly;

        // set year and month
        UpdateDate();

        // toggle
        yearMonthToggle.onValueChanged.AddListener(OnValueChange);

        // events
        DateRelatedDataUpdate += UpdateDate;
    }

    private void OnValueChange(bool b)
    {
        isMonthly = b;
        DateRelatedDataUpdate?.Invoke(); // calls related events
    }

    private void OnDisable()
    {
        DateRelatedDataUpdate -= UpdateDate;
        PlayerPrefs.SetInt("IsMonthly", Convert.ToInt16(IsMonthly));
    }

    private void UpdateDate()
    {
        // change value of dateDisplayed
        dateDisplayed = DateDisplayed;

        // set interactability
        if (IsMonthly)
        {
            nextButton.interactable = (DateDisplayed.Year < System.DateTime.Today.Year) || (DateDisplayed.Month < System.DateTime.Today.Month);
            lastButton.interactable = (DateDisplayed.Year > System.DateTime.Today.Year - MAX_TIME_BEFORE_TODAY) || (DateDisplayed.Month > System.DateTime.Today.Month);
        }
        else
        {
            nextButton.interactable = (DateDisplayed.Year < System.DateTime.Today.Year);
            lastButton.interactable = (DateDisplayed.Year > System.DateTime.Today.Year - MAX_TIME_BEFORE_TODAY);
        }

        // Update Text Displayed
        UpdateDisplayedText();
    }

    private void UpdateDisplayedText()
    {
        if (IsMonthly)
            yearMonth.text = DateDisplayed.ToString("yy/MMMM");
        else
            yearMonth.text = DateDisplayed.Year.ToString();
    }
}
