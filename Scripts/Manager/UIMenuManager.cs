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
    public Slider masterVolumeSlider; // �� �����̴��� ������� ������ ���� ���� ���� �ݿ�
    public Slider sfxVolumeSlider;    // ȿ���� ���� �����̴�
    public Slider bgVolumeSlider;     // ������� ���� �����̴�

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
            Debug.Log("UIMenuManager�� MainBtnGameOver ����");

        if (ExitBtnGameOver != null)
            ExitBtnGameOver.onClick.AddListener(Exit);
        else
            Debug.Log("UIMenuManager�� ExitBtnGameOver ����");

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

            // Playerprefs���� ����� ���� �ҷ��ͼ� FullScreenMode ����
            FullScreenMode loadedFullscreenMode = LoadFullscreenMode();

            // FullScreenMode�� ���� toggle ����
            fullscreenToggle.isOn = loadedFullscreenMode == FullScreenMode.FullScreenWindow;

            // FullScreenMode ����
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

        // PlayerPrefs���� ���� �ҷ�����
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", defaultMasterVolume);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", defaultSFXVolume);
        bgVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", defaultBGMVolume);
    }

    public void OnCloseSettingBtn()
    {
        // ������ ����
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("BGMVolume", bgVolumeSlider.value);
        PlayerPrefs.Save();

        SoundManager.instance.UpdateAllAudioVolumes();
        //SoundManager.instance.SaveSoundSettings();
        isPaused = false;
        Time.timeScale = 1; // ���� ������ �ٽ� �簳
        // ���� �޴��� �ݰ� ���� ��ư�� �ٽ� Ȱ��ȭ
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
        Debug.Log("������ �����մϴ�.");
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
        float fadeDuration = 1f; // ���̵� �� ȿ���� �ӵ��� �����Ϸ��� �� ���� ����.

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
            // ���ӿ��� �г��� CanvasGroup�� ����Ͽ� ���̵� �� ȿ�� ����
            StartCoroutine(FadeInGameOverPanel(() =>
            {
                // ���ӿ��� �г��� ��Ÿ�� �Ŀ� �� �۾����� �߰�.
                //�ٸ� UI ������Ʈ, �Ҹ� ��� ��

            }));
        }
    }

    void InitUI()
    {
        // PlayerPrefs�� ����� �ػ󵵿� Ǯ��ũ�� ��带 �ҷ�����, ����� ���� ���� ��쿡�� �⺻ �ػ󵵿� Ǯ��ũ�� ���� ����
        Resolution savedResolution = LoadResolution();
        FullScreenMode savedScreenMode = LoadFullscreenMode();
        bool savedFullscreenToggleState = LoadFullscreenToggleState();

        if (savedResolution.width != 0 && savedResolution.height != 0)
        {
            currentResolution = savedResolution;
        }
        else
        {
            // �⺻ �ػ� ���� (1920x1080)
            currentResolution = new Resolution { width = 1920, height = 1080 };
        }

        if (resolutionDropdown != null)
        {
            InitializeResolutionDropdown();
        }

        // Ǯ��ũ�� ��� ���� ����
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
        // PlayerPrefs���� Ǯ��ũ�� ��� ���¸� �ҷ���
        return PlayerPrefs.GetInt("FullscreenToggleState", 1) == 1 ? true : false;
    }

    private Dictionary<string, int> resolutionIndexMap = new Dictionary<string, int>();  // �ػ� ���ڿ��� �ε��� ����
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

            // �ߺ��� �ػ󵵸� ���͸�
            if (!resolutionIndexMap.ContainsKey(resolutionString))
            {
                resolutionIndexMap.Add(resolutionString, i);

                // Ŀ���� �ػ󵵿� �ش��ϸ� ��Ӵٿ �߰�
                if (IsCustomResolution(resolutions[i]))
                {
                    resolutionDropdown.options.Add(new Dropdown.OptionData(resolutionString));
                }

                if (resolutions[i].width == currentResolution.width && resolutions[i].height == currentResolution.height)
                {
                    resolutionDropdown.value = resolutionDropdown.options.Count - 1; // ���� �ػ��� �ε��� ����
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

                // ������ �ػ󵵷� ��Ӵٿ� ����
                SetDropdownToResolution(selectedResolution);
            }
            else
            {
                Debug.LogError("Failed to find original index for selected resolution.");
            }
        }
    }

    // ���ϴ� �ػ����� Ȯ���ϴ� �Լ�
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

        // ������ �ػ󵵿� Ǯ��ũ�� ���� ȭ�� ����
        Screen.SetResolution(currentResolution.width, currentResolution.height, screenMode);
    }

    public void CancelResolution()
    {
        // PlayerPrefs���� ����� �ػ󵵿� Ǯ��ũ�� ��带 �ҷ����� ����
        Resolution savedResolution = LoadResolution();
        FullScreenMode savedFullscreenMode = LoadFullscreenMode();

        if (savedResolution.width != 0 && savedResolution.height != 0)
        {
            // PlayerPrefs���� �ҷ��� ������ �ػ󵵸� ����
            Screen.SetResolution(savedResolution.width, savedResolution.height, savedFullscreenMode);
            currentResolution = savedResolution;

            // ��Ӵٿ�� �ش� �ػ󵵸� ���õǰ� �����
            SetDropdownToResolution(savedResolution);

            // Ǯ��ũ�� ��� ��ۿ� ����
            if (fullscreenToggle != null)
            {
                fullscreenToggle.isOn = savedFullscreenMode == FullScreenMode.FullScreenWindow;
            }
        }
    }

    void SetDropdownToResolution(Resolution resolution)
    {
        // ��Ӵٿ�� ����� �ػ󵵸� ã�� �ε����� ����
        for (int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            string optionText = resolutionDropdown.options[i].text;

            // ���� ���̸� ����
            string[] parts = optionText.Split('x');
            if (parts.Length == 2 && int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height))
            {
                // �ֻ����� ������ ��� ���� ���̸� ��ġ�ϴ��� Ȯ��
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
        // �ػ󵵸� ���ڿ��� ��ġ�� PlayerPrefs�� ����
        string resolutionString = resolution.width.ToString() + "x" + resolution.height.ToString();
        PlayerPrefs.SetString("ScreenResolution", resolutionString);

        // Ǯ��ũ�� ��带 int�� ����
        PlayerPrefs.SetInt("FullscreenMode", screenMode == FullScreenMode.FullScreenWindow ? 1 : 0);

        PlayerPrefs.Save();
    }

    Resolution LoadResolution()
    {
        // PlayerPrefs���� ���ڿ��� ����� �ػ󵵸� �ҷ��ͼ� Resolution���� ��ȯ
        string resolutionString = PlayerPrefs.GetString("ScreenResolution", "");
        if (!string.IsNullOrEmpty(resolutionString))
        {
            string[] resolutionParts = resolutionString.Split('x');
            if (resolutionParts.Length == 2 && int.TryParse(resolutionParts[0], out int width) && int.TryParse(resolutionParts[1], out int height))
            {
                return new Resolution { width = width, height = height };
            }
        }

        // ����� ���� ���ų� ��ȯ�� ������ ��� �⺻ �ػ� ��ȯ
        return new Resolution { width = 1920, height = 1080 };
    }

    void SaveFullscreenMode(FullScreenMode fullscreenMode)
    {
        // Ǯ��ũ�� ��带 int�� ����
        PlayerPrefs.SetInt("FullscreenMode", fullscreenMode == FullScreenMode.FullScreenWindow ? 1 : 0);
        PlayerPrefs.Save();
    }

    FullScreenMode LoadFullscreenMode()
    {
        // PlayerPrefs���� int�� ����� Ǯ��ũ�� ��带 �ҷ��� FullScreenMode�� ��ȯ
        int fullscreenModeInt = PlayerPrefs.GetInt("FullscreenMode", 0);
        return fullscreenModeInt == 1 ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
}


