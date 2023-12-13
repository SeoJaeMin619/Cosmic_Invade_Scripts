using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuBtn : MonoBehaviour
{
    private bool isPaused = false;
    private Button PauseButton;

    public GameObject MenuBtn;
    public GameObject UIMenu;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void TogglePausePanel()
    {
        if (UIMenu != null)
        {
            isPaused = !isPaused;
            UIMenu.SetActive(true);
            MenuBtn.SetActive(false);

            // ���� �Ͻ� ���� ���¿� ���� ���� ������ ���� �Ǵ� �簳�� �� ����
            Time.timeScale = isPaused ? 0 : 1;

        }
    }
    public void ResumeGame()
    {
        if (UIMenu != null)
        {
            // Resume ��ư�� ������ �� Pause �ǳ��� ��Ȱ��ȭ�ϰ� ������ �簳
            isPaused = false;
            UIMenu.SetActive(false);
            Time.timeScale = 1; // ���� ������ �ٽ� �簳
            MenuBtn.SetActive(true);
        }
    }


}
