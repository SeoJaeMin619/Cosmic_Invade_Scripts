using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectMenu : MonoBehaviour
{
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button towerReinforceBtn;
    [SerializeField] private Button towerSellBtn;
    [SerializeField] private Button towerRepairBtn;

    [SerializeField] private GameObject towerUpgradePopup;
    [SerializeField] private GameObject towerMenuPopup;

    private TextMeshProUGUI towerRange;
    private TextMeshProUGUI sellPrice;

    void Start()
    {
        cancelBtn.onClick.AddListener(Cancel);
        //towerReinforceBtn.onClick.AddListener(TowerReinforce);
        towerSellBtn.onClick.AddListener(TowerSell);
        towerRepairBtn.onClick.AddListener(TowerRepair);
    }

    private void Update()
    {
        UpdateReinforceButton();
    }

    private void UpdateReinforceButton()
    {
        if (GameManager.goldcount <= 50)
        {
            towerReinforceBtn.interactable = false;
        }
        else if (GameManager.goldcount > 50)
        {
            towerReinforceBtn.interactable = true;
        }
    }

    //private void TowerReinforce()
    //{
    //    towerUpgradePopup.SetActive(true);
    //}
    private void Cancel()
    {
        towerMenuPopup.SetActive(false);
    }
    private void TowerRepair()
    {
        // ÅÍ·¿ Ã¼·Â È¸º¹(1ÃÊ¿¡ 2 »ó½Â)
    }

    private void TowerSell()
    {
        GameManager.goldcount += 50;
        towerMenuPopup.SetActive(false);
    }

}
