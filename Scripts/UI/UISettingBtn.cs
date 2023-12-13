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

    public Slider masterVolumeSlider; // �� �����̴��� ������� ������ ���� ���� ���� �ݿ�
    public Slider sfxVolumeSlider;    // ȿ���� ���� �����̴�
    public Slider bgvolumeSlider;     // ������� ���� �����̴�

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

        // PlayerPrefs���� ���� �ҷ�����
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", defaultMasterVolume);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", defaultSFXVolume);
        bgvolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", defaultBGMVolume);
    }

    public void OnCloseSettingBtn()
    {
        // ������ ����
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("BGMVolume", bgvolumeSlider.value);
        PlayerPrefs.Save();

        SoundManager.instance.UpdateAllAudioVolumes();
        //SoundManager.instance.SaveSoundSettings();

        // ���� �޴��� �ݰ� ���� ��ư�� �ٽ� Ȱ��ȭ
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
