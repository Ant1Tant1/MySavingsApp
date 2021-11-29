using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(SetTransparancy))]
public class MessageBar : Singleton<MessageBar>
    {
    public Image image;
    public TextMeshProUGUI text;
    public SetTransparancy setTransparancy;

    private Color defaultColor = new Color(50/255f, 90/255f, 125/255f, 1f);

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void DisplayMessage(string message, float timing = 3f)
    {
        // ensure the routine is not running
        StopAllCoroutines();
        Reset();

        // set message
        text.text = message;

        // change height depending of mesasge size
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, text.preferredHeight + 20);

        // set last sibling
        gameObject.transform.SetAsLastSibling();

        // set active
        gameObject.SetActive(true);

        // Start coroutine
        StartCoroutine(DisplayMessageCoroutine(timing));
    }

    public void DisplayMessage(string message, Color32 color, float timing = 3f)
    {
        // set active false
        gameObject.SetActive(false);

        // set color
        image.color = color;

        // Display message
        DisplayMessage(message, timing);
    }

    private IEnumerator DisplayMessageCoroutine(float timing)
    {
        yield return new WaitForSeconds(timing/2);

        // fade out
        setTransparancy.FadeOut(timing/2);

        // work around for text
        float t = 0;
        while (t < timing / 2)
        {
            t += Time.deltaTime;
            SetTextTransparancy();
            yield return null;
        }

        //set object inactive
        gameObject.SetActive(false);

        // Reset
        image.color = defaultColor;
        Reset();
    }

    private void Reset()
    {
        // reset transparancy
        gameObject.SetActive(false);
        setTransparancy.SetNoTransparancy();
        SetTextTransparancy();
    }

    private void SetTextTransparancy()
    {
        Color color = text.color;
        color.a = image.color.a;
        text.color = color;
    }
}
