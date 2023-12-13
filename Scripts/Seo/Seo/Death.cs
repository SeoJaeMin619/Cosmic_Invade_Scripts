using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    [SerializeField] private NexusSO data;
    public static int currentMP;

    public RoundManager roundManager;
    public GameObject targetImageGameObject;  // 이미지가 속한 GameObject를 Inspector에서 할당하거나 Awake에서 찾을 수 있습니다.
    private Image fillImage;  // 이미지의 Image 컴포넌트
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

            // targetImageGameObject에서 Image 컴포넌트 가져오기
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

            // 200초 동안 버튼 비활성화
            StartCoroutine(FillImageOverTime(0f, 1f));

            // 60초 후에 이미지의 Fill Amount를 1에서 0으로 변경하는 코루틴 시작
            StartCoroutine(FillImageOverTime(1f, 0f, 60f));

            cooltime = true;
            yourButton.interactable = false;  // 버튼 비활성화
            Invoke("ResetCooltime", 200f);
            Invoke("EnableButton", 200f);  // 200초 후에 버튼 활성화
        

    }
    }

    private void ResetCooltime()
    {
        cooltime = false;
    }

    private void EnableButton()
    {
        yourButton.interactable = true;  // 버튼 활성화
    }

    // 이미지의 Fill Amount를 변경하는 코루틴
    IEnumerator FillImageOverTime(float startFill, float endFill, float duration = 0f)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 시간에 따라 Fill Amount를 변경
            fillImage.fillAmount = Mathf.Lerp(startFill, endFill, elapsedTime / duration);

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 최종적으로 Fill Amount를 설정
        fillImage.fillAmount = endFill;

        // 쿨타임 리셋
        ResetCooltime();
    }

    // 버튼을 비활성화하는 코루틴
    IEnumerator DisableButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Button yourButton = GetComponent<Button>();
        yourButton.interactable = false;
    }
}