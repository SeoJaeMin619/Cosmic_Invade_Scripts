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
        // 각 버튼에 대한 클릭 이벤트 핸들러 설정
        settingButton.onClick.AddListener(OnSetting);
        //applyButton.onClick.AddListener(OnApply);
        applyButton.onClick.AddListener(OnCloseSettingBtn);

        exitButton.onClick.AddListener(OnExit);
        //cancelButton.onClick.AddListener(OnCancel);


        // 추가: 배열에 있는 모든 버튼에 동일한 SFXPlay 추가
      

        InitUI();


        if (applyButton != null)
        {
            applyButton.onClick.AddListener(ApplyResolution);
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
    }

    public void OnSetting()
    {
        SoundManager.instance.SFXPlay(SFXType.blop);

        UIMenu.SetActive(true);

        // 슬라이더 값들을 PlayerPrefs에서 불러와서 설정
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
        Debug.Log("게임을 종료합니다.");
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
        // 설정을 저장
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("BGMVolume", bgVolumeSlider.value);
        PlayerPrefs.Save();


        SoundManager.instance.LoadSoundSettings();

        SoundManager.instance.SaveSoundSettings();
        UIMenu.SetActive(false);
        // 세팅 메뉴를 닫고 원래 버튼을 다시 활성화

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

    public void GetSelectedResolution()
    {
        // 현재 선택된 인덱스를 가져옴
        int selectedResolutionIndex = resolutionDropdown.value;

        // 인덱스를 통해 선택된 값(옵션)을 가져옴
        string selectedResolution = resolutionDropdown.options[selectedResolutionIndex].text;

        // 이제 selectedResolution에는 선택된 해상도가 문자열로 들어 있음
        Debug.Log("Selected Resolution: " + selectedResolution);
    }
}