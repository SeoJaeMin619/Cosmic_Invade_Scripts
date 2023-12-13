using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpSlider : MonoBehaviour
{
    public GameObject hpslider;
    private Slider slider;
    

    private void Awake()
    {

        Debug.Assert(GameManager.instance.hpSlider != null,"HP Null");
        this.hpslider = Instantiate(GameManager.instance.hpSlider, GameObject.Find("HpSlider").transform);
        this.slider = this.hpslider.GetComponent<Slider>();
        this.slider.value = 1;
    }

    private void Update()
    {

        Vector3 screenPostion = Camera.main.WorldToScreenPoint( gameObject.transform.position );
        this.hpslider.GetComponent<RectTransform>().position = screenPostion + Vector3.down * 20.0f; 
 
        Unit unit = gameObject.GetComponent<Unit>();

        if (TurretInfo.Load.ContainsKey(gameObject))
        {
            TurretInfo Turret = TurretInfo.Load[gameObject];
            this.slider.value = (float)Turret.CurrentHP / Turret.HP;
        }
        if (unit != null)
        {
            this.slider.value = (float)unit.CurrnetHP / unit.MaxHP;
        }


    }
}