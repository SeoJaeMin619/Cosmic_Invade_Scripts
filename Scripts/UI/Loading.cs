using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Slider loadingSlider;
    public string nextSceneName; // ���� ���� �̸��� ����

    private void Start()
    {
        // �ε� �ڷ�ƾ ����
        StartCoroutine(LoadingCoroutine());
    }

    IEnumerator LoadingCoroutine()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);
        asyncOperation.allowSceneActivation = false;

        float elapsedTime = 0f;
        float duration = 5f; // �ε� �ð� (��)

        while (elapsedTime < duration)
        {
            // �����̴� ���� �ð��� ���� �������� �ε� ȿ�� ����
            loadingSlider.value = elapsedTime / duration;

            // ��� �ð� ����
            elapsedTime += Time.deltaTime;

            // �� ������ ��ٸ���
            yield return null;
        }

        // �񵿱� �۾��� ���� �Ϸ�Ǹ� ���� ������ �̵�
        loadingSlider.value = 1f;
        asyncOperation.allowSceneActivation = true;
    }
}


