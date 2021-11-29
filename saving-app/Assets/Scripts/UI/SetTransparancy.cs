using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SetTransparancy : MonoBehaviour
{
    [SerializeField]
    private Image image;

    private void SetImageTransparancy(float transparancy)
    {
        Color color = image.color;
        color.a = transparancy;
        image.color = color;
    }

    public float GetImageTransparancy()
    {
        return image.color.a;
    }

    public void SetFullTransparancy()
    {
        SetImageTransparancy(0);
    }

    public void SetNoTransparancy()
    {
        SetImageTransparancy(1);
    }

    public void FadeIn(float timing)
    {
        // if you have no transparancy, set the image transparant
        if(GetImageTransparancy() == 1f)
        {
            SetFullTransparancy();
        }

        // stop all ongoing coroutines
        StopAllCoroutines();

        // start coroutine
        StartCoroutine(AppearCoroutine(timing));
    }

    public void FadeOut(float timing)
    {
        // if you have full transparancy, set the image opaque
        if (GetImageTransparancy() == 0f)
        {
            SetNoTransparancy();
        }

        // stop all ongoing coroutines
        StopAllCoroutines();

        // start coroutine
        StartCoroutine(DisappearCoroutine(timing));
    }

    private IEnumerator AppearCoroutine(float timing)
    {
        float t = GetImageTransparancy();
        while (t < timing)
        {
            t += Time.deltaTime;
            SetImageTransparancy(t / timing);
            yield return null;
        }
        SetNoTransparancy();
    }

    private IEnumerator DisappearCoroutine(float timing)
    {
        float t = GetImageTransparancy();
        while (t < timing)
        {
            t += Time.deltaTime;
            SetImageTransparancy(1 - t / timing);
            yield return null;
        }
        SetFullTransparancy();
    }
}
