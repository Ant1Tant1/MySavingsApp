using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.ReadWrite.Objects;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayAllCashFlows : MonoBehaviour
{
    // UI elements
    [SerializeField]
    private GameObject cashflowPrefab;
    [SerializeField]
    private AddCashFlowDetails addCashFlowDetails;
    [SerializeField]
    private RectTransform progressBarSlider;
    private ScrollRect scrollRect;

    // list of objects 
    private List<CashFlow> cashFlowList = new List<CashFlow>();
    private CashFlowPrefab[] displayedCashflowsPrefabs;

    // cashflows dipslayed
    private const int NB_OF_DISPLAYED_CASHFLOWS = 7;
    private int startIndex = 0;

    // for sliding in between pages
    private float initPos;
    private float offset;
    private bool isMoved;

    // for progress bar
    private int nbOfPage;
    private int page;
    private float progressBarBackgroundWidth;

    // Properties
    public int StartIndex
    {
        get => startIndex;
        set
        {
            if (value > cashFlowList.Count)
                throw new System.Exception("Value can not be smaller than 0 or larger than cashFlowList.Count");

            if (value < 0)
                startIndex = 0;
            else
                startIndex = value;

            Display();
        }
    }

    public int Page
    { 
        get => page;
        set
        {
            progressBarSlider.localPosition = new Vector3(
                        value * progressBarSlider.sizeDelta.x - progressBarBackgroundWidth/2, 
                        progressBarSlider.localPosition.y,
                        progressBarSlider.localPosition.z
            );
            page = value;
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        // initialize rectTransform
        scrollRect = transform.parent.parent.GetComponent<ScrollRect>();
        scrollRect.onValueChanged.AddListener(ScrollRectListener);

        // Subscribe to event
        DatesContainer.DateRelatedDataUpdate += UpdateDateCashFlows;
        Engine.DataUpdated += UpdateDataCashFlows;

        // display cashflow list
        UpdateDateCashFlows();

        // get init position
        initPos = transform.localPosition.x;
        offset = GetComponent<RectTransform>().sizeDelta.x / 10;
    }

    private void OnDisable()
    {
        DatesContainer.DateRelatedDataUpdate -= UpdateDateCashFlows;
        Engine.DataUpdated -= UpdateDataCashFlows;
        DestroyAll();
    }

    private void UpdateDateCashFlows()
    {
        // get ordered by date cashflows details 
        cashFlowList = Operations.GetYearlyOrMonthlyAllCashFlowsTransactions(DatesContainer.DateDisplayed, DatesContainer.IsMonthly);
        cashFlowList.Reverse();
        StartIndex = 0;

        // compute data for progress bar
        progressBarBackgroundWidth = progressBarSlider.parent.GetComponent<RectTransform>().sizeDelta.x;
        nbOfPage = Mathf.CeilToInt((float)cashFlowList.Count / (float)NB_OF_DISPLAYED_CASHFLOWS);
        progressBarSlider.sizeDelta = new Vector2(progressBarBackgroundWidth / nbOfPage, progressBarSlider.sizeDelta.y);
        Page = 0;
    }

    private void UpdateDataCashFlows()
    {
        // get ordered by date cashflows details 
        cashFlowList = Operations.GetYearlyOrMonthlyAllCashFlowsTransactions(DatesContainer.DateDisplayed, DatesContainer.IsMonthly);
        cashFlowList.Reverse();
        Display();
    }


    private void Display()
    {
        if (displayedCashflowsPrefabs == null)
            displayedCashflowsPrefabs = new CashFlowPrefab[NB_OF_DISPLAYED_CASHFLOWS];

        for (int i = 0; i < NB_OF_DISPLAYED_CASHFLOWS; i++) 
        {
            // instantiate objects if needed (first time)
            if (displayedCashflowsPrefabs[i] == null)
            {
                GameObject gameObject = Instantiate(cashflowPrefab, transform);
                displayedCashflowsPrefabs[i] = (gameObject.GetComponent<CashFlowPrefab>());
            }

            // hide object if we got over the number of cashflows
            if (i + startIndex >= cashFlowList.Count)
            {
                displayedCashflowsPrefabs[i].gameObject.SetActive(false);
                continue;
            }

            // Get cashflow type and subtype
            CashFlowTypes type = (CashFlowTypes)cashFlowList[i + startIndex].typeId;
            CashFlowSubCategories subCategory = Engine.CashFlowSubCategories;
            Dictionary<int, string> subtype = subCategory.GetRelevantDict(type);

            // setup
            displayedCashflowsPrefabs[i].Setup(type, subtype[cashFlowList[i + startIndex].subtypeId], cashFlowList[i + startIndex].date.ToString("yyyy/MMM/dd"), cashFlowList[i + startIndex].value);
            Button b = displayedCashflowsPrefabs[i].editButton;
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => addCashFlowDetails.EditCashFlow(cashFlowList[b.transform.parent.GetSiblingIndex() + startIndex]));
            displayedCashflowsPrefabs[i].gameObject.SetActive(true); //if it has been hidden before
        }
    }

    private void DestroyAll()
    {
        if (displayedCashflowsPrefabs == null)
            return;

        foreach (CashFlowPrefab prefab in displayedCashflowsPrefabs)
                Destroy(prefab.gameObject);

        displayedCashflowsPrefabs = null;
    }

    public void ScrollRectListener(Vector2 value)
    {
        if (isMoved)
            return;

        // going right
        if (transform.localPosition.x < initPos - offset & startIndex + NB_OF_DISPLAYED_CASHFLOWS < cashFlowList.Count)
        {
            StartIndex += NB_OF_DISPLAYED_CASHFLOWS;
            Page += 1;
            StartCoroutine(WaitCoroutine());
        }

        // going left
        if (transform.localPosition.x > initPos + offset & startIndex != 0)
        {
            StartIndex -= NB_OF_DISPLAYED_CASHFLOWS;
            Page -= 1;
            StartCoroutine(WaitCoroutine());
        }
    }

    private IEnumerator WaitCoroutine()
    {
        isMoved = true;
        yield return new WaitForSeconds(0.3f);
        isMoved = false;
    }

}
