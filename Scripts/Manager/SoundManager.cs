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
    //public Slider bgVolumeSlider;  // ������� ���� ���� �����̴�
    //public Slider sfxVolumeSlider; // ȿ���� ���� ���� �����̴�
    //public Slider masterVolumeSlider; // ������ ���� ���� �����̴�

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
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume") * PlayerPrefs.GetFloat("MasterVolume"); // ȿ���� ������ ������ ������ �Բ� ����
        audioSource.Play();
        Destroy(go, SFXlist[index].length);
    }

    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = PlayerPrefs.GetFloat("BGMVolume") * PlayerPrefs.GetFloat("MasterVolume"); // ��� ���� ������ ������ ������ �Բ� ����
        bgSound.Play();
    }

    public void LoadSoundSettings()
    {
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        BGMVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);

        // ������ ���� ���� �� ��� ����� ����� ���� ������Ʈ
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
        // ��� ����� ��ҿ� ������ ���� ����
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume");

        // ȿ���� ������ ������Ʈ
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume") * masterVolume;
        // ��� ���� ������ ������Ʈ
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume") * masterVolume;

        // ������ ����� ��ҿ� ������Ʈ�� ���� ����
        bgSound.volume = bgmVolume;
        // �ٸ� ����� ��ҵ� �ʿ��Ѵ�� �߰�
    }


    private IEnumerator CheckWalkingStatus(GameObject go, AudioSource audioSource)
    {
        while (TopDownPlayerMove.isWalking)
        {
            yield return null;
        }

        // �Ҹ��� ���� false�� �Ǹ� �Ҹ��� �����ϰ� GameObject�� �ı�
        audioSource.Stop();
        Destroy(go);
    }
    public void SFXPlay(SFXType sfxType)
    {
        // enum ����� ������ �״�� ���
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
        if (!ExistWalk)  // Walk ���尡 ���� ���� ����
        {
            int index = (int)sfxType;

            GameObject go = new GameObject(sfxType.ToString() + "Sound");
            AudioSource audioSource = go.AddComponent<AudioSource>();
            audioSource.clip = SFXlist[index];
            audioSource.volume = PlayerPrefs.GetFloat("SFXVolume") * PlayerPrefs.GetFloat("MasterVolume");
            audioSource.loop = true;
            audioSource.Play();
            ExistWalk = true;  // Walk ���尡 �����Ǿ����� ǥ��
            Debug.Log("������");
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
            Debug.Log("�帱������");
            StartCoroutine(CheckDrillStatus(go, audioSource));
        }
    }
    private IEnumerator CheckDrillStatus(GameObject go, AudioSource audioSource)
    {
        while (PlayerDrill.IsDrilling && TopDownPlayerMove.isCameraFollowing)
        {
            yield return null;
        }

        // �Ҹ��� ���� false�� �Ǹ� �Ҹ��� �����ϰ� GameObject�� �ı�
        audioSource.Stop();
        Destroy(go);
        Debug.Log("��");
    }
}