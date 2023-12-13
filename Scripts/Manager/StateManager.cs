using UnityEngine;
public class StateManager : MonoBehaviour
{
    public static StateManager I;
    public GameObject hpSlider;
    void Awake()
    {
        I = this;
    }
}
