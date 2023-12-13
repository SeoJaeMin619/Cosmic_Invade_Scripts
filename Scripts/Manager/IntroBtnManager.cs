using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SoundManager;

public class IntroBtnManager : MonoBehaviour
{
    [Header("[Slider]")]
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider bgVolumeSlider;

    public GameObject UIMenu;

    [Header("[IntroButton]")]

    public Button settingButton;
    public Button exitButton;

    [Header("[SettingButton]")]

    public Button applyButton;


    private Button[] allButtons;


    [Header("[Resolutions]")]
    public Dropdown resolutionDropdown;
    FullScreenMode screenMode;
    public Toggle fullscreenToggle;

    private Resolution currentResolution;


    // Start is called before the first frame update
    void Start()
    {
        // �� ��ư�� ���� Ŭ�� �̺�Ʈ �ڵ鷯 ����
        settingButton.onClick.AddListener(OnSetting);
        //applyButton.onClick.AddListener(OnApply);
        applyButton.onClick.AddListener(OnCloseSettingBtn);

        exitButton.onClick.AddListener(OnExit);
        //cancelButton.onClick.AddListener(OnCancel);


        // �߰�: �迭�� �ִ� ��� ��ư�� ������ SFXPlay �߰�
      

        InitUI();


        if (applyButton != null)
        {
            applyButton.onClick.AddListener(ApplyResolution);
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
    }

    public void OnSetting()
    {
        SoundManager.instance.SFXPlay(SFXType.blop);

        UIMenu.SetActive(true);

        // �����̴� ������ PlayerPrefs���� �ҷ��ͼ� ����
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        bgVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
    }



    public void OnCancel()
    {
        UIMenu.SetActive(false);
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        bgVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
    }
    public void OnExit()
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

    public void OnCloseSettingBtn()
    {
        // ������ ����
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("BGMVolume", bgVolumeSlider.value);
        PlayerPrefs.Save();


        SoundManager.instance.LoadSoundSettings();

        SoundManager.instance.SaveSoundSettings();
        UIMenu.SetActive(false);
        // ���� �޴��� �ݰ� ���� ��ư�� �ٽ� Ȱ��ȭ

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
            string[] parts = optionText.Split('x');
            if (parts.Length == 2 && int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height))
            {
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

    public void GetSelectedResolution()
    {
        // ���� ���õ� �ε����� ������
        int selectedResolutionIndex = resolutionDropdown.value;

        // �ε����� ���� ���õ� ��(�ɼ�)�� ������
        string selectedResolution = resolutionDropdown.options[selectedResolutionIndex].text;

        // ���� selectedResolution���� ���õ� �ػ󵵰� ���ڿ��� ��� ����
        Debug.Log("Selected Resolution: " + selectedResolution);
    }
}