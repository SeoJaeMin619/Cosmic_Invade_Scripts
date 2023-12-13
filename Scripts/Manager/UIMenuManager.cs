using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SoundManager;

public class UIMenuManager : MonoBehaviour
{
    private bool isPaused = false;



    [Header("[Onoff]")]
    public Button PauseButton;
    public GameObject PauseMenuBtn;
    public GameObject UIMenu;
    public GameObject SettingPanel;

    [Header("[UISettingMenuBtn")]
    public GameObject ResumeBtn;
    public GameObject SettingBtn;
    public GameObject MainMenuBtn;
    public GameObject ExitBtn;
    [Header("[slider]")]
    public Slider masterVolumeSlider; // 이 슬라이더에 사용자의 마스터 볼륨 설정 값을 반영
    public Slider sfxVolumeSlider;    // 효과음 볼륨 슬라이더
    public Slider bgVolumeSlider;     // 배경음악 볼륨 슬라이더

    [Header("[Button]")]
    public Button ResumeButton;
    public Button SettingButton;
    public Button MainMenuButton;
    public Button ExitButton;
    public Button ApplyButton;


    [Header("[Slider]")]
    private float defaultMasterVolume = 0.5f;
    private float defaultSFXVolume = 0.5f;
    private float defaultBGMVolume = 0.5f;
    // Start is called before the first frame update
    private Button[] settingButtons;

    [Header("[GameOver]")]
    public Button MainBtnGameOver;
    public Button ExitBtnGameOver;
    public GameObject GameOverPanel;
    public CanvasGroup gameOverCanvasGroup;

    [Header("[Resolutions]")]
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    FullScreenMode screenMode;
    private Resolution currentResolution;


    void Start()
    {
        SettingButton.onClick.AddListener(OnSettingBtn);
        PauseButton.onClick.AddListener(TogglePausePanel);
        ResumeButton.onClick.AddListener(ResumeGame);
        MainMenuButton.onClick.AddListener(MainMenu);
        ExitButton.onClick.AddListener(Exit);

        ApplyButton.onClick.AddListener(OnCloseSettingBtn);
        
        
        if (MainBtnGameOver != null)
            MainBtnGameOver.onClick.AddListener(MainMenu);
        else
            Debug.Log("UIMenuManager에 MainBtnGameOver 누락");

        if (ExitBtnGameOver != null)
            ExitBtnGameOver.onClick.AddListener(Exit);
        else
            Debug.Log("UIMenuManager에 ExitBtnGameOver 누락");

        if (resolutionDropdown != null && fullscreenToggle != null)
        {
            InitUI();
        }


        if (ApplyButton != null)
        {
            ApplyButton.onClick.AddListener(ApplyResolution);
        }
        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.AddListener(delegate { FullScreentoggle(fullscreenToggle.isOn); });

            // Playerprefs에서 저장된 값을 불러와서 FullScreenMode 설정
            FullScreenMode loadedFullscreenMode = LoadFullscreenMode();

            // FullScreenMode에 따라 toggle 설정
            fullscreenToggle.isOn = loadedFullscreenMode == FullScreenMode.FullScreenWindow;

            // FullScreenMode 설정
            FullScreentoggle(fullscreenToggle.isOn);
        }
        if (resolutionDropdown != null)
        {
            resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(); });

        }
        //ShowGameOverPanel();

        if (GameOverPanel != null)
        {
            gameOverCanvasGroup = GameOverPanel.GetComponent<CanvasGroup>();
            if (gameOverCanvasGroup == null)
            {

                gameOverCanvasGroup = GameOverPanel.AddComponent<CanvasGroup>();
            }


            gameOverCanvasGroup.alpha = 0f;

            MainBtnGameOver.onClick.AddListener(() => StartCoroutine(FadeInGameOverPanel(() => MainMenu())));
            ExitBtnGameOver.onClick.AddListener(() => StartCoroutine(FadeInGameOverPanel(() => Exit())));
        }

    }

    // Update is called once per frame
    public void TogglePausePanel()
    {
        if (UIMenu != null)
        {
            isPaused = !isPaused;
            UIMenu.SetActive(true);
            PauseMenuBtn.SetActive(false);

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
            PauseMenuBtn.SetActive(true);
        }
    }

    private void Update()
    {
        if (SettingPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnCloseSettingBtn();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                TogglePausePanel();
            }
        }
    }



    public void OnSettingBtn()
    {
        ResumeBtn.SetActive(false);
        SettingBtn.SetActive(false);
        MainMenuBtn.SetActive(false);
        ExitBtn.SetActive(false);
        UIMenu.SetActive(false);
        SettingPanel.SetActive(true);

        // PlayerPrefs에서 설정 불러오기
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", defaultMasterVolume);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", defaultSFXVolume);
        bgVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", defaultBGMVolume);
    }

    public void OnCloseSettingBtn()
    {
        // 설정을 저장
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("BGMVolume", bgVolumeSlider.value);
        PlayerPrefs.Save();

        SoundManager.instance.UpdateAllAudioVolumes();
        //SoundManager.instance.SaveSoundSettings();
        isPaused = false;
        Time.timeScale = 1; // 게임 로직을 다시 재개
        // 세팅 메뉴를 닫고 원래 버튼을 다시 활성화
        ResumeBtn.SetActive(true);
        SettingBtn.SetActive(true);
        MainMenuBtn.SetActive(true);
        ExitBtn.SetActive(true);
        UIMenu.SetActive(false);
        SettingPanel.SetActive(false);
        PauseMenuBtn.SetActive(true);
    }

    public void UICancel()
    {
        ResumeBtn.SetActive(true);
        SettingBtn.SetActive(true);
        MainMenuBtn.SetActive(true);
        ExitBtn.SetActive(true);

        SettingPanel.SetActive(false);
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        bgVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
        SoundManager.instance.Onplay(0);
    }

    public void Exit()
    {
        Debug.Log("게임을 종료합니다.");
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
        }
    }


    IEnumerator FadeInGameOverPanel(System.Action callback)
    {
        float fadeDuration = 1f; // 페이드 인 효과의 속도를 조절하려면 이 값을 조정.

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            gameOverCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameOverCanvasGroup.alpha = 1f;

        isPaused = true;
        Time.timeScale = isPaused ? 0 : 1;
        callback?.Invoke();
    }

    public void ShowGameOverPanel()
    {



        if (GameOverPanel != null)
        {
            GameOverPanel.SetActive(true);
            // 게임오버 패널의 CanvasGroup을 사용하여 페이드 인 효과 적용
            StartCoroutine(FadeInGameOverPanel(() =>
            {
                // 게임오버 패널이 나타난 후에 할 작업들을 추가.
                //다른 UI 업데이트, 소리 재생 등

            }));
        }
    }

    void InitUI()
    {
        // PlayerPrefs에 저장된 해상도와 풀스크린 모드를 불러오고, 저장된 값이 없는 경우에는 기본 해상도와 풀스크린 모드로 설정
        Resolution savedResolution = LoadResolution();
        FullScreenMode savedScreenMode = LoadFullscreenMode();
        bool savedFullscreenToggleState = LoadFullscreenToggleState();

        if (savedResolution.width != 0 && savedResolution.height != 0)
        {
            currentResolution = savedResolution;
        }
        else
        {
            // 기본 해상도 설정 (1920x1080)
            currentResolution = new Resolution { width = 1920, height = 1080 };
        }

        if (resolutionDropdown != null)
        {
            InitializeResolutionDropdown();
        }

        // 풀스크린 토글 상태 설정
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = savedFullscreenToggleState;
        }
    }


    void FullScreentoggle(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    bool LoadFullscreenToggleState()
    {
        // PlayerPrefs에서 풀스크린 토글 상태를 불러옴
        return PlayerPrefs.GetInt("FullscreenToggleState", 1) == 1 ? true : false;
    }

    private Dictionary<string, int> resolutionIndexMap = new Dictionary<string, int>();  // 해상도 문자열과 인덱스 매핑
    private int[] customWidths = { 720, 1280, 1440, 1600, 1768, 1920 };
    private int[] customHeights = { 480, 720, 900, 900, 992, 1080 };

    void InitializeResolutionDropdown()
    {
        Resolution[] resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        resolutionIndexMap.Clear();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolutionString = resolutions[i].width + "x" + resolutions[i].height;

            // 중복된 해상도를 필터링
            if (!resolutionIndexMap.ContainsKey(resolutionString))
            {
                resolutionIndexMap.Add(resolutionString, i);

                // 커스텀 해상도에 해당하면 드롭다운에 추가
                if (IsCustomResolution(resolutions[i]))
                {
                    resolutionDropdown.options.Add(new Dropdown.OptionData(resolutionString));
                }

                if (resolutions[i].width == currentResolution.width && resolutions[i].height == currentResolution.height)
                {
                    resolutionDropdown.value = resolutionDropdown.options.Count - 1; // 현재 해상도의 인덱스 설정
                }
            }
        }

        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution()
    {
        int selectedResolutionIndex = resolutionDropdown.value;

        if (selectedResolutionIndex >= 0 && selectedResolutionIndex < resolutionDropdown.options.Count)
        {
            string selectedResolutionString = resolutionDropdown.options[selectedResolutionIndex].text;

            if (resolutionIndexMap.TryGetValue(selectedResolutionString, out int originalIndex))
            {
                Resolution selectedResolution = Screen.resolutions[originalIndex];



                currentResolution = selectedResolution;

                // 선택한 해상도로 드롭다운 설정
                SetDropdownToResolution(selectedResolution);
            }
            else
            {
                Debug.LogError("Failed to find original index for selected resolution.");
            }
        }
    }

    // 원하는 해상도인지 확인하는 함수
    bool IsCustomResolution(Resolution resolution)
    {
        for (int i = 0; i < customWidths.Length; i++)
        {
            if (resolution.width == customWidths[i] && resolution.height == customHeights[i])
            {
                return true;
            }
        }
        return false;
    }



    public void ApplyResolution()
    {
        SaveResolution(currentResolution);

        // 선택한 해상도와 풀스크린 모드로 화면 변경
        Screen.SetResolution(currentResolution.width, currentResolution.height, screenMode);
    }

    public void CancelResolution()
    {
        // PlayerPrefs에서 저장된 해상도와 풀스크린 모드를 불러오고 적용
        Resolution savedResolution = LoadResolution();
        FullScreenMode savedFullscreenMode = LoadFullscreenMode();

        if (savedResolution.width != 0 && savedResolution.height != 0)
        {
            // PlayerPrefs에서 불러온 값으로 해상도를 설정
            Screen.SetResolution(savedResolution.width, savedResolution.height, savedFullscreenMode);
            currentResolution = savedResolution;

            // 드롭다운에서 해당 해상도를 선택되게 만들기
            SetDropdownToResolution(savedResolution);

            // 풀스크린 모드 토글에 적용
            if (fullscreenToggle != null)
            {
                fullscreenToggle.isOn = savedFullscreenMode == FullScreenMode.FullScreenWindow;
            }
        }
    }

    void SetDropdownToResolution(Resolution resolution)
    {
        // 드롭다운에서 저장된 해상도를 찾아 인덱스를 설정
        for (int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            string optionText = resolutionDropdown.options[i].text;

            // 폭과 높이를 추출
            string[] parts = optionText.Split('x');
            if (parts.Length == 2 && int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height))
            {
                // 주사율을 고정한 경우 폭과 높이만 일치하는지 확인
                if (width == resolution.width && height == resolution.height)
                {
                    resolutionDropdown.value = i;
                    resolutionDropdown.RefreshShownValue();
                    break;
                }
            }
        }
    }


    void SaveResolution(Resolution resolution)
    {
        // 해상도를 문자열로 합치고 PlayerPrefs에 저장
        string resolutionString = resolution.width.ToString() + "x" + resolution.height.ToString();
        PlayerPrefs.SetString("ScreenResolution", resolutionString);

        // 풀스크린 모드를 int로 저장
        PlayerPrefs.SetInt("FullscreenMode", screenMode == FullScreenMode.FullScreenWindow ? 1 : 0);

        PlayerPrefs.Save();
    }

    Resolution LoadResolution()
    {
        // PlayerPrefs에서 문자열로 저장된 해상도를 불러와서 Resolution으로 변환
        string resolutionString = PlayerPrefs.GetString("ScreenResolution", "");
        if (!string.IsNullOrEmpty(resolutionString))
        {
            string[] resolutionParts = resolutionString.Split('x');
            if (resolutionParts.Length == 2 && int.TryParse(resolutionParts[0], out int width) && int.TryParse(resolutionParts[1], out int height))
            {
                return new Resolution { width = width, height = height };
            }
        }

        // 저장된 값이 없거나 변환에 실패할 경우 기본 해상도 반환
        return new Resolution { width = 1920, height = 1080 };
    }

    void SaveFullscreenMode(FullScreenMode fullscreenMode)
    {
        // 풀스크린 모드를 int로 저장
        PlayerPrefs.SetInt("FullscreenMode", fullscreenMode == FullScreenMode.FullScreenWindow ? 1 : 0);
        PlayerPrefs.Save();
    }

    FullScreenMode LoadFullscreenMode()
    {
        // PlayerPrefs에서 int로 저장된 풀스크린 모드를 불러와 FullScreenMode로 변환
        int fullscreenModeInt = PlayerPrefs.GetInt("FullscreenMode", 0);
        return fullscreenModeInt == 1 ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
}


