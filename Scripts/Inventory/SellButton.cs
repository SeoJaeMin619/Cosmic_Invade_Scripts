using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButton : MonoBehaviour
{
    public GameObject sellPopupPrefab;

    public void OpenSellPopup()
    {
        GameObject sellPopup = Instantiate(sellPopupPrefab);
        SellPopup sellPopupScript = sellPopup.GetComponent<SellPopup>();
        sellPopupScript.SetSellButton(this);
    }

    public void SellItem(int quantity)
    {
        GameManager.rubyCount -= quantity;       
    }


}
