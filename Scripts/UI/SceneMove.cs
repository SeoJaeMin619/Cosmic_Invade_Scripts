using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene); // 전달된 씬 이름으로 이동
        SoundManager.instance.Onplay(1);
        TopDownPlayerMove.isCameraFollowing = true;
        CameraView.isCameraFollowing = false;
    }
}
