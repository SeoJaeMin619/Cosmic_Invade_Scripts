using System;
using UnityEngine;
using UnityEngine.UI;

public class Nexus : Unit
{
    public Action OnDestroyNexus;
    public Action OnDamagedNexus;
    public Action OnHealedNexus;
    public NexusSO Data;
    public Slider healthSlider; // Unity Inspector에서 설정하기 위한 Slider 변수
    private float timeSinceLastHeal;
    private float healInterval = 1f;

    private void Update()
    {
        timeSinceLastHeal += Time.deltaTime;

        if (timeSinceLastHeal >= healInterval)
        {
            Healed(5); // 여기서 수치 조절 가능
            OnHealedNexus?.Invoke();
            timeSinceLastHeal = 0f;
        }

        // CurrnetHP를 Slider에 반영
        if (healthSlider != null)
        {
            healthSlider.value = CalculateHealthPercentage();
        }
    }

    private void Awake()
    {
        if (Data != null)
        {
            MaxHP = Data.MaxHP();
            CurrnetHP = MaxHP;
        }
    }

    public override void Damaged(int dmg)
    {
        base.Damaged(dmg);

        if (CurrnetHP <= 0)
        {
            OnDestroyNexus?.Invoke();
            GameManager.instance.GameOver();
            Destroy(this.gameObject);

        }
 
        OnDamagedNexus?.Invoke();
    }

    public override void Healed(int heal)
    {
        base.Healed(heal);

        OnHealedNexus?.Invoke();
    }

    // 현재 체력의 백분율을 계산
    private float CalculateHealthPercentage()
    {
        return(float) CurrnetHP / MaxHP;
    }
}