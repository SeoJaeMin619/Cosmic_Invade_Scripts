using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Slider loadingSlider;
    public string nextSceneName; // 다음 씬의 이름을 지정

    private void Start()
    {
        // 로딩 코루틴 시작
        StartCoroutine(LoadingCoroutine());
    }

    IEnumerator LoadingCoroutine()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);
        asyncOperation.allowSceneActivation = false;

        float elapsedTime = 0f;
        float duration = 5f; // 로딩 시간 (초)

        while (elapsedTime < duration)
        {
            // 슬라이더 값을 시간에 따라 증가시켜 로딩 효과 구현
            loadingSlider.value = elapsedTime / duration;

            // 경과 시간 증가
            elapsedTime += Time.deltaTime;

            // 한 프레임 기다리기
            yield return null;
        }

        // 비동기 작업이 거의 완료되면 다음 씬으로 이동
        loadingSlider.value = 1f;
        asyncOperation.allowSceneActivation = true;
    }
}


