using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    [SerializeField] private NexusSO data;
    public static int currentMP;

    public RoundManager roundManager;
    public GameObject targetImageGameObject;  // �̹����� ���� GameObject�� Inspector���� �Ҵ��ϰų� Awake���� ã�� �� �ֽ��ϴ�.
    private Image fillImage;  // �̹����� Image ������Ʈ
    private Button yourButton;
    private bool isInitialized = false;
    private bool cooltime = false;

    private void Awake()
    {
        if (!isInitialized)
        {
            yourButton = GetComponent<Button>();
            yourButton.onClick.AddListener(DeathBtn);
            isInitialized = true;

            // targetImageGameObject���� Image ������Ʈ ��������
            if (targetImageGameObject != null)
            {
                fillImage = targetImageGameObject.GetComponent<Image>();
            }
        }
    }

    private void DeathBtn()
    {
        if (!cooltime)
        {
            roundManager.DeathUnits();
            cooltime = true;

            // 200�� ���� ��ư ��Ȱ��ȭ
            StartCoroutine(FillImageOverTime(0f, 1f));

            // 60�� �Ŀ� �̹����� Fill Amount�� 1���� 0���� �����ϴ� �ڷ�ƾ ����
            StartCoroutine(FillImageOverTime(1f, 0f, 60f));

            cooltime = true;
            yourButton.interactable = false;  // ��ư ��Ȱ��ȭ
            Invoke("ResetCooltime", 200f);
            Invoke("EnableButton", 200f);  // 200�� �Ŀ� ��ư Ȱ��ȭ
        

    }
    }

    private void ResetCooltime()
    {
        cooltime = false;
    }

    private void EnableButton()
    {
        yourButton.interactable = true;  // ��ư Ȱ��ȭ
    }

    // �̹����� Fill Amount�� �����ϴ� �ڷ�ƾ
    IEnumerator FillImageOverTime(float startFill, float endFill, float duration = 0f)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // �ð��� ���� Fill Amount�� ����
            fillImage.fillAmount = Mathf.Lerp(startFill, endFill, elapsedTime / duration);

            // ��� �ð� ������Ʈ
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // ���������� Fill Amount�� ����
        fillImage.fillAmount = endFill;

        // ��Ÿ�� ����
        ResetCooltime();
    }

    // ��ư�� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    IEnumerator DisableButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Button yourButton = GetComponent<Button>();
        yourButton.interactable = false;
    }
}