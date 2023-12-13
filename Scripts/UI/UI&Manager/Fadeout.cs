using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextFadeOut : MonoBehaviour
{
    public Text characterNameText;
    public float fadeDuration = 3f;

    private IEnumerator fadeOutCoroutine;

    void Start()
    {
        //characterNameText.enabled = false;
    }

    public void DisplayErrorMessage(string message)
    {
        characterNameText.text = message;
        characterNameText.enabled = true;

        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        fadeOutCoroutine = FadeOutText();
        StartCoroutine(fadeOutCoroutine);
    }

    private IEnumerator FadeOutText()
    {
        // yield return new WaitForSeconds(fadeDuration);

        float elapsedTime = 0f;
        Color textColor = characterNameText.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(5f, 0f, elapsedTime / fadeDuration);
            textColor.a = alpha;
            characterNameText.color = textColor;
            yield return null;
        }

        characterNameText.enabled = false;
    }
}