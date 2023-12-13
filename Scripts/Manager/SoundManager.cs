using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public enum SFXType
    {
        blop,
        coin,
        walk,
        shot,
        crash,
        laser,
        hitSound,
        bonk,
        drill,
    }
    public AudioSource bgSound;
    public AudioClip[] bglist;
    public AudioClip[] SFXlist; 
    //public Slider bgVolumeSlider;  // 배경음악 볼륨 조절 슬라이더
    //public Slider sfxVolumeSlider; // 효과음 볼륨 조절 슬라이더
    //public Slider masterVolumeSlider; // 마스터 볼륨 조절 슬라이더

    private float MasterVolume = 0.5f;
    private float SFXVolume = 0.5f;
    private float BGMVolume = 0.5f;

    public static bool ExistWalk = false;
    public static bool ExistDrill= false;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            if (transform.parent == null)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                //Debug.LogWarning("Warning: GameObject with DontDestroyOnLoad should be a root GameObject.");
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        LoadSoundSettings();
        Onplay(0);
    }

    public void SFXPlay(string sfxName, int index)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = SFXlist[index];
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume") * PlayerPrefs.GetFloat("MasterVolume"); // 효과음 볼륨과 마스터 볼륨을 함께 적용
        audioSource.Play();
        Destroy(go, SFXlist[index].length);
    }

    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = PlayerPrefs.GetFloat("BGMVolume") * PlayerPrefs.GetFloat("MasterVolume"); // 배경 음악 볼륨과 마스터 볼륨을 함께 적용
        bgSound.Play();
    }

    public void LoadSoundSettings()
    {
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        BGMVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);

        // 마스터 볼륨 변경 시 모든 오디오 요소의 볼륨 업데이트
        UpdateAllAudioVolumes();
    }

    public void SaveSoundSettings()
    {
        //PlayerPrefs.SetFloat("MasterVolume");
        //PlayerPrefs.SetFloat("SFXVolume");
        //PlayerPrefs.SetFloat("BGMVolume");
        PlayerPrefs.Save();
        UpdateAllAudioVolumes();
    }

    public void Onplay(int index)
    {
        BgSoundPlay(bglist[index]);
    }


    public void UpdateAllAudioVolumes()
    {
        // 모든 오디오 요소에 마스터 볼륨 적용
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume");

        // 효과음 볼륨을 업데이트
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume") * masterVolume;
        // 배경 음악 볼륨을 업데이트
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume") * masterVolume;

        // 각각의 오디오 요소에 업데이트된 볼륨 적용
        bgSound.volume = bgmVolume;
        // 다른 오디오 요소도 필요한대로 추가
    }


    private IEnumerator CheckWalkingStatus(GameObject go, AudioSource audioSource)
    {
        while (TopDownPlayerMove.isWalking)
        {
            yield return null;
        }

        // 불리언 값이 false가 되면 소리를 중지하고 GameObject를 파괴
        audioSource.Stop();
        Destroy(go);
    }
    public void SFXPlay(SFXType sfxType)
    {
        // enum 상수의 순서를 그대로 사용
        int index = (int)sfxType;

        GameObject go = new GameObject(sfxType.ToString() + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = SFXlist[index];
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume") * PlayerPrefs.GetFloat("MasterVolume");
        audioSource.Play();
        Destroy(go, SFXlist[index].length);
    }
    public void LoopSFXPlay(SFXType sfxType)
    {
        if (!ExistWalk)  // Walk 사운드가 없을 때만 실행
        {
            int index = (int)sfxType;

            GameObject go = new GameObject(sfxType.ToString() + "Sound");
            AudioSource audioSource = go.AddComponent<AudioSource>();
            audioSource.clip = SFXlist[index];
            audioSource.volume = PlayerPrefs.GetFloat("SFXVolume") * PlayerPrefs.GetFloat("MasterVolume");
            audioSource.loop = true;
            audioSource.Play();
            ExistWalk = true;  // Walk 사운드가 생성되었음을 표시
            Debug.Log("생성함");
            StartCoroutine(CheckWalkingStatus(go, audioSource));
        }
    }

    public void DrillSFXPlay(SFXType sfxType)
    {
        if (!ExistDrill)
        {
            int index = (int)sfxType;

            GameObject go = new GameObject(sfxType.ToString() + "Sound");
            AudioSource audioSource = go.AddComponent<AudioSource>();
            audioSource.clip = SFXlist[index];
            audioSource.volume = PlayerPrefs.GetFloat("SFXVolume") * PlayerPrefs.GetFloat("MasterVolume");
            audioSource.loop = true;
            audioSource.Play();
            ExistDrill= true;
            Debug.Log("드릴생성함");
            StartCoroutine(CheckDrillStatus(go, audioSource));
        }
    }
    private IEnumerator CheckDrillStatus(GameObject go, AudioSource audioSource)
    {
        while (PlayerDrill.IsDrilling && TopDownPlayerMove.isCameraFollowing)
        {
            yield return null;
        }

        // 불리언 값이 false가 되면 소리를 중지하고 GameObject를 파괴
        audioSource.Stop();
        Destroy(go);
        Debug.Log("끝");
    }
}