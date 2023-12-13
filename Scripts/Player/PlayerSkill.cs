using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField] 
    private Button usingSkill;
    private float curCooltime;
    private float maxCooltime = 30f;
    [SerializeField]
    private TextMeshProUGUI timer;
    [SerializeField]
    private Image disableImg;
    [SerializeField]
    private Transform home;
    [SerializeField]
    private GameObject hell;

    private bool isCooldown;

    private void Start()
    {
        usingSkill.onClick.AddListener(ComeBackHome);
        timer = usingSkill.GetComponentInChildren<TextMeshProUGUI>();
        disableImg = usingSkill.GetComponent<Image>();
        disableImg.type = Image.Type.Filled;
        disableImg.fillMethod = Image.FillMethod.Radial360;

    }

    private void Update()
    {
        if(isCooldown)
        {
            curCooltime -= Time.deltaTime;
            UpdateCooldownUI();

            if(curCooltime <= 0)
            {
                curCooltime = 0;
                isCooldown = false;
                usingSkill.interactable = true;
                disableImg.fillAmount = 1;
                timer.text = "";
            }
        }
    }

    private void UpdateCooldownUI()
    {
        float normalizedCooldown = curCooltime / maxCooltime;
        disableImg.fillAmount = normalizedCooldown;
        timer.text = Mathf.CeilToInt(curCooltime).ToString();
    }

    private void ComeBackHome()
    {
        if(!isCooldown)
        {
            transform.position = home.position;
            StartCooldown();
        }
    }

    private void StartCooldown()
    {
        curCooltime = maxCooltime;
        isCooldown = true;
        usingSkill.interactable = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == hell)
        {
            usingSkill.interactable = false;
        }
        if(collision.transform == home)
        {
            usingSkill.interactable = true;
        }
    }
}
