using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReinforce : MonoBehaviour
{

    int maxSkillLevel = 5;    
    [SerializeField]
    private TextMeshProUGUI curHealthLvTxt;    
    [SerializeField]
    private TextMeshProUGUI curHealthRecoverLvTxt;   
    [SerializeField]
    private TextMeshProUGUI curHealthReduceLvTxt;
    private int healthRecoverLevel = 0;
    private int healthReduceLevel = 0;
    private int healthQuantityLevel = 0;

    [SerializeField]
    private TextMeshProUGUI curOxygenLevelQuantity;
    [SerializeField]
    private TextMeshProUGUI curOxygenLevelRecovery; 
    [SerializeField]
    private TextMeshProUGUI curOxygenLevelReduce;
    private int oxygenQuantityLevel = 0;
    private int oxygenRecoveLevel = 0;
    private int oxygenReduceLevel = 0;


    [SerializeField]
    private TextMeshProUGUI bulletQuantityLvTxt;
    [SerializeField]
    private TextMeshProUGUI bulltReloadLvTxt;
    [SerializeField]
    private TextMeshProUGUI bulletSpeedLvTxt;
    private int bulletQuantityLevel = 0;
    private int bulletReloadLevel = 0;
    private int bulletSpeedLevel = 0;

    [Header("Button")]
    [SerializeField] private Button healthBtn;
    [SerializeField] private Button oxygenBtn;
    [SerializeField] private Button bulletBtn;

    [Header("UpgradeBullet")]
    [SerializeField] private List<Button> upgradeBullet;
    [SerializeField] private TextMeshProUGUI bulletUpgradeCostTxt1;
    [SerializeField] private TextMeshProUGUI bulletUpgradeCostTxt2;
    [SerializeField] private TextMeshProUGUI bulletUpgradeCostTxt3;

    [Header("UpgradeOxygen")]
    [SerializeField] private List<Button> upgradeOxygen;
    [SerializeField] private TextMeshProUGUI oxygenUpgradeCostTxt1;
    [SerializeField] private TextMeshProUGUI oxygenUpgradeCostTxt2;
    [SerializeField] private TextMeshProUGUI oxygenUpgradeCostTxt3;

    [Header("UpgradeHealth")]
    [SerializeField] private List<Button> upgradeHealth;
    [SerializeField] private TextMeshProUGUI healthUpgradeCostTxt1;
    [SerializeField] private TextMeshProUGUI healthUpgradeCostTxt2;
    [SerializeField] private TextMeshProUGUI healthUpgradeCostTxt3;


    [Header("Text")]
    [SerializeField] private TextMeshProUGUI titleText;

    [Header("Panel")]
    [SerializeField] private List<GameObject> panels;

    private PlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();

        healthBtn.onClick.AddListener(PanelHeath);
        oxygenBtn.onClick.AddListener(PanelOxygen);
        bulletBtn.onClick.AddListener(PanelBullet);

     
            upgradeBullet[0].onClick.AddListener(BulletQuantity);
            upgradeBullet[1].onClick.AddListener(BulletReload);
            upgradeBullet[2].onClick.AddListener(BulletSpeed);

            upgradeHealth[0].onClick.AddListener(MaxHealthUp);
            upgradeHealth[1].onClick.AddListener(HealthRecoverUp);
            upgradeHealth[2].onClick.AddListener(HealthReducedUp);

            upgradeOxygen[0].onClick.AddListener(MaxOxygenUp);
            upgradeOxygen[1].onClick.AddListener(OxygenRecoverUp);
            upgradeOxygen[2].onClick.AddListener(OxygenReducedUp);


        UpdateUpgradeCostText1();
        UpdateUpgradeCostText2();
        UpdateUpgradeCostText3();
        ActivatePanel(0);
    }

    private void OxygenReducedUp()
    {
        if (oxygenReduceLevel < maxSkillLevel)
        {
            int upgradeCost = 500 * (oxygenReduceLevel + 1); 
            if (GameManager.goldcount >= upgradeCost)
            {
                oxygenReduceLevel++;
                Player.oxygenDecreaseRate -= 0.7f;
                curOxygenLevelReduce.text = "현재 레벨 : " + oxygenReduceLevel.ToString();
                upgradeOxygen[2].interactable = oxygenReduceLevel < maxSkillLevel;
                GameManager.goldcount -= upgradeCost;
                UpdateUpgradeCostText3();
            }
            else
            {
                Debug.Log("골드가 부족합니다.");
            }
        }
    }

    private void OxygenRecoverUp()
    {
        if (oxygenRecoveLevel < maxSkillLevel)
        {
            int upgradeCost = 500 * (oxygenRecoveLevel + 1);
            if (GameManager.goldcount >= upgradeCost)
            {
                oxygenRecoveLevel++;
                Player.oxygenIncreaseRate += 0.1f;
                curOxygenLevelRecovery.text = "현재 레벨 : " + oxygenRecoveLevel.ToString();
                upgradeOxygen[1].interactable = oxygenRecoveLevel < maxSkillLevel;
                GameManager.goldcount -= upgradeCost;
                UpdateUpgradeCostText2();
            }
            else
            {
                Debug.Log("골드가 부족합니다.");
            }
        }
    }


    private void MaxOxygenUp()
    {
        if (oxygenQuantityLevel < maxSkillLevel)
        {
            int upgradeCost = 500 * (oxygenQuantityLevel + 1);
            if (GameManager.goldcount >= upgradeCost)
            {
                oxygenQuantityLevel++;
                Player.maxOxygen += 10f;
                curOxygenLevelQuantity.text = "현재 레벨 : " + oxygenQuantityLevel.ToString();
                upgradeOxygen[0].interactable = oxygenQuantityLevel < maxSkillLevel;
                GameManager.goldcount -= upgradeCost;
                UpdateUpgradeCostText1();
            }
            else
            {
                Debug.Log("골드가 부족합니다.");
            }
        }
    }

    private void HealthReducedUp()
    {
        if (healthReduceLevel < maxSkillLevel)
        {
            int upgradeCost = 500 * (healthReduceLevel + 1);
            if (GameManager.goldcount >= upgradeCost)
            {
                healthReduceLevel++;
                Player.healthDecreaseRate -= 0.1f;
                curHealthReduceLvTxt.text = "현재 레벨 : " + healthReduceLevel.ToString();
                upgradeHealth[2].interactable = healthReduceLevel < maxSkillLevel;
                GameManager.goldcount -= upgradeCost;
                UpdateUpgradeCostText3();
            }
            else
            {
                Debug.Log("골드가 부족합니다.");
            }
        }
    }


    private void HealthRecoverUp()
    {
        if (healthRecoverLevel < maxSkillLevel)
        {
            int upgradeCost = 500 * (healthRecoverLevel + 1);
            if (GameManager.goldcount >= upgradeCost)
            {
                healthRecoverLevel++;
                Player.healthIncreaseRate += 0.1f;
                curHealthRecoverLvTxt.text = "현재 레벨 : " + healthRecoverLevel.ToString();
                upgradeHealth[1].interactable = healthRecoverLevel < maxSkillLevel;
                GameManager.goldcount -= upgradeCost;
                UpdateUpgradeCostText2();
            }
            else
            {
                Debug.Log("골드가 부족합니다.");
            }
        }
    }

    private void MaxHealthUp()
    {
        if (healthQuantityLevel < maxSkillLevel)
        {
            int upgradeCost = 500 * (healthQuantityLevel + 1);
            if (GameManager.goldcount >= upgradeCost)
            {
                healthQuantityLevel++;
                Player.playerMaxHp += 10;
                curHealthLvTxt.text = "현재 레벨 : " + healthQuantityLevel.ToString();
                upgradeHealth[0].interactable = healthQuantityLevel < maxSkillLevel;
                GameManager.goldcount -= upgradeCost;
                UpdateUpgradeCostText1();
            }
            else
            {
                Debug.Log("골드가 부족합니다.");
            }
        }
    }

    private void BulletReload()
    {
        if (bulletReloadLevel < maxSkillLevel)
        {
            int upgradeCost = 500 * (bulletReloadLevel + 1);
            if (GameManager.goldcount >= upgradeCost)
            {
                bulletReloadLevel++;
                PlayerAttack.reloadSpeed -= 0.3f;
                bulltReloadLvTxt.text = "현재 레벨 : " + bulletReloadLevel.ToString();
                upgradeBullet[1].interactable = bulletReloadLevel < maxSkillLevel;
                GameManager.goldcount -= upgradeCost;
                UpdateUpgradeCostText2();
            }
            else
            {
                Debug.Log("골드가 부족합니다.");
            }
        }
    }

    private void BulletQuantity()
    {
        if (bulletQuantityLevel < maxSkillLevel)
        {
            int upgradeCost = 500 * (bulletQuantityLevel + 1);
            if (GameManager.goldcount >= upgradeCost)
            {
                bulletQuantityLevel++;
                PlayerAttack.maxBulletQuantity += 10;
                bulletQuantityLvTxt.text = "현재 레벨 : " + bulletQuantityLevel.ToString();
                upgradeBullet[0].interactable = bulletQuantityLevel < maxSkillLevel;
                GameManager.goldcount -= upgradeCost;
                UpdateUpgradeCostText1();
            }
            else
            {
                Debug.Log("골드가 부족합니다.");
            }
        }
    }

    private void BulletSpeed()
    {
        if (bulletSpeedLevel < maxSkillLevel)
        {   
            int upgradeCost = 500 * (bulletSpeedLevel + 1);
            if (GameManager.goldcount >= upgradeCost)
            {
                bulletSpeedLevel++;
                PlayerAttack.shootinterval -= 0.04f;
                bulletSpeedLvTxt.text = "현재레벨 : " + bulletSpeedLevel.ToString();
                upgradeBullet[2].interactable = bulletSpeedLevel < maxSkillLevel;
                GameManager.goldcount -= upgradeCost;
                UpdateUpgradeCostText3();
            }
            else
            {
                Debug.Log("골드가 부족합니다.");
            }
        }
    }

    private void PanelBullet()
    {
        titleText.text = "탄환 강화";
        ActivatePanel(2);
    }

    private void PanelOxygen()
    {
        titleText.text = "산소 강화";
        ActivatePanel(1);

    }

    private void PanelHeath()
    {
        titleText.text = "체력 강화";
        ActivatePanel(0);
    }


    private void ActivatePanel(int index)
    {
        for (int i = 0; i < panels.Count; i++)
        {
            if (i == index)
            {
                panels[i].SetActive(true);
            }
            else
            {
                panels[i].SetActive(false);
            }
        }
    }

    private void UpdateGoldCount()
    {
        GameManager.goldcount -= 500;
    }

    private void UpdateUpgradeCostText1()
    {
        bulletUpgradeCostTxt1.text = "G : " + (500 * (bulletQuantityLevel + 1)).ToString();
        oxygenUpgradeCostTxt1.text = "G : " + (500 * (oxygenQuantityLevel + 1)).ToString();
        healthUpgradeCostTxt1.text = "G : " + (500 * (healthQuantityLevel + 1)).ToString();
    
       
    }
    private void UpdateUpgradeCostText2()
    {
        bulletUpgradeCostTxt2.text = "G : " + (500 * (bulletReloadLevel + 1)).ToString();
        oxygenUpgradeCostTxt2.text = "G : " + (500 * (oxygenRecoveLevel + 1)).ToString();
        healthUpgradeCostTxt2.text = "G : " + (500 * (healthRecoverLevel + 1)).ToString();
    }

    private void UpdateUpgradeCostText3()
    {
        bulletUpgradeCostTxt3.text = "G : " + (500 * (bulletSpeedLevel + 1)).ToString();
        oxygenUpgradeCostTxt3.text = "G : " + (500 * (oxygenReduceLevel + 1)).ToString();
        healthUpgradeCostTxt3.text = "G : " + (500 * (healthReduceLevel + 1)).ToString();
    }
}
