using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellPopup : MonoBehaviour
{
    [SerializeField] InputField quantityInput;
    private SellButton sellButton;

    public void SetSellButton(SellButton button)
    {
        sellButton = button;
    }

    public void onSellButtonClicked()
    {
        int quantity = int.Parse(quantityInput.text);
        sellButton.SellItem(quantity);
        Destroy(gameObject);
    }
}
