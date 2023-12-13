using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingBtn : MonoBehaviour
{
    public GameObject ResumeBtn;
    public GameObject SettingBtn;
    public GameObject MainMenuBtn;
    public GameObject ExitBtn;

    public Slider masterVolumeSlider; // 이 슬라이더에 사용자의 마스터 볼륨 설정 값을 반영
    public Slider sfxVolumeSlider;    // 효과음 볼륨 슬라이더
    public Slider bgvolumeSlider;     // 배경음악 볼륨 슬라이더

    public GameObject SettingPanel;

    private float defaultMasterVolume = 0.5f;
    private float defaultSFXVolume = 0.5f;
    private float defaultBGMVolume = 0.5f;

    void Awake()
    {
    
    }

    public void OnSettingBtn()
    {
        ResumeBtn.SetActive(false);
        SettingBtn.SetActive(false);
        MainMenuBtn.SetActive(false);
        ExitBtn.SetActive(false);

        SettingPanel.SetActive(true);

        // PlayerPrefs에서 설정 불러오기
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", defaultMasterVolume);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", defaultSFXVolume);
        bgvolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", defaultBGMVolume);
    }

    public void OnCloseSettingBtn()
    {
        // 설정을 저장
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("BGMVolume", bgvolumeSlider.value);
        PlayerPrefs.Save();

        SoundManager.instance.UpdateAllAudioVolumes();
        //SoundManager.instance.SaveSoundSettings();

        // 세팅 메뉴를 닫고 원래 버튼을 다시 활성화
        ResumeBtn.SetActive(true);
        SettingBtn.SetActive(true);
        MainMenuBtn.SetActive(true);
        ExitBtn.SetActive(true);

        SettingPanel.SetActive(false);
    }

    public void UICancel()
    {
        ResumeBtn.SetActive(true);
        SettingBtn.SetActive(true);
        MainMenuBtn.SetActive(true);
        ExitBtn.SetActive(true);

        SettingPanel.SetActive(false);
    }
}
