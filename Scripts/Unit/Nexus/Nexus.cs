using System;
using UnityEngine;
using UnityEngine.UI;

public class Nexus : Unit
{
    public Action OnDestroyNexus;
    public Action OnDamagedNexus;
    public Action OnHealedNexus;
    public NexusSO Data;
    public Slider healthSlider; // Unity Inspector���� �����ϱ� ���� Slider ����
    private float timeSinceLastHeal;
    private float healInterval = 1f;

    private void Update()
    {
        timeSinceLastHeal += Time.deltaTime;

        if (timeSinceLastHeal >= healInterval)
        {
            Healed(5); // ���⼭ ��ġ ���� ����
            OnHealedNexus?.Invoke();
            timeSinceLastHeal = 0f;
        }

        // CurrnetHP�� Slider�� �ݿ�
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

    // ���� ü���� ������� ���
    private float CalculateHealthPercentage()
    {
        return(float) CurrnetHP / MaxHP;
    }
}