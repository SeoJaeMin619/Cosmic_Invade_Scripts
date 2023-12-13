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

            // 게임 일시 중지 상태에 따라 게임 로직을 정지 또는 재개할 수 있음
            Time.timeScale = isPaused ? 0 : 1;

        }
    }
    public void ResumeGame()
    {
        if (UIMenu != null)
        {
            // Resume 버튼을 눌렀을 때 Pause 판넬을 비활성화하고 게임을 재개
            isPaused = false;
            UIMenu.SetActive(false);
            Time.timeScale = 1; // 게임 로직을 다시 재개
            MenuBtn.SetActive(true);
        }
    }


}
