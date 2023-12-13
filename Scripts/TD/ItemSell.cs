using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSell : MonoBehaviour
{
    [Header(" Item Sell ")]
    [Space]
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private Text quantityText;
    [SerializeField] private Slider quantitySlider;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button[] gemButtons;

    private int maxQuantity = 100;
    private int currentQuantity = 0;
    private int price = 0;
    private int[] prices;
    private int Number;
    private int currentGemIndex = -1;

    void Start()
    {
        quantitySlider.maxValue = 0;
        UpdateQuantityText();

        quantitySlider.onValueChanged.AddListener(UpdateQuantityFromSlider);
        sellButton.onClick.AddListener(SellItem);

        Debug.Assert(gemButtons != null, "gemButtons Null");

        for (int i = 0; i < gemButtons.Length; i++)
        {
            int index = i;
            gemButtons[i].onClick.AddListener(() => Price(index));
        }
    }
    private void Update()
    {
        prices = new int[] { GameManager.diamondPrice, GameManager.rubyPrice, GameManager.emeraldPrice,
                         GameManager.sapphirePrice, GameManager.amethystPrice, GameManager.aquamarinePrice };

        price = prices[Number];
        UpdateQuantityText();
    }
    void Price(int index)
    {
        currentGemIndex = index;
        
        int[] counts = { GameManager.diamondCount, GameManager.rubyCount, GameManager.emeraldCount,
                         GameManager.sapphireCount, GameManager.amethystCount, GameManager.aquamarineCount };

        Number = index;
        price = prices[index];
        maxQuantity = counts[index];

        quantitySlider.maxValue = maxQuantity;

        switch (index)
        {
            case 0:
                name.text = "���̾�";
                break;
            case 1:
                name.text = "���";
                break;
            case 2:
                name.text = "���޶���";
                break;
            case 3:
                name.text = "�����̾�";
                break;
            case 4:
                name.text = "�ڼ���";
                break;
            case 5:
                name.text = "����Ƹ���";
                break;
            default:
                break;
        }
    }

    void UpdateQuantityFromSlider(float value)
    {
        currentQuantity = Mathf.RoundToInt(value);
        UpdateQuantityText();
    }

    void UpdateQuantityText()
    {
        quantityText.text = "�Ǹ� ����: " + currentQuantity + "\n" + currentQuantity * price + "���� �Ǹ��Ͻðڽ��ϱ�?";
    }

    void SellItem()
    {
        if (maxQuantity >= currentQuantity)
        {
            if (currentQuantity > 0)
            {
                switch (currentGemIndex)
                {
                    case 0:
                        GameManager.diamondCount -= currentQuantity;
                        TurretManager.I.textfadeout.DisplayErrorMessage("���̾Ƹ�" + currentQuantity + "�� �Ǹ��ϼ̽��ϴ�") ;
                        break;
                    case 1:
                        GameManager.rubyCount -= currentQuantity;
                        TurretManager.I.textfadeout.DisplayErrorMessage("���" + currentQuantity + "�� �Ǹ��ϼ̽��ϴ�");
                        break;
                    case 2:
                        GameManager.emeraldCount -= currentQuantity;
                        TurretManager.I.textfadeout.DisplayErrorMessage("���޶��带" + currentQuantity + "�� �Ǹ��ϼ̽��ϴ�");
                        break;
                    case 3:
                        GameManager.sapphireCount -= currentQuantity;
                        TurretManager.I.textfadeout.DisplayErrorMessage("�����̾" + currentQuantity + "�� �Ǹ��ϼ̽��ϴ�");
                        break;
                    case 4:
                        GameManager.amethystCount -= currentQuantity;
                        TurretManager.I.textfadeout.DisplayErrorMessage("�ڼ�����" + currentQuantity + "�� �Ǹ��ϼ̽��ϴ�");
                        break;
                    case 5:
                        GameManager.aquamarineCount -= currentQuantity;
                        TurretManager.I.textfadeout.DisplayErrorMessage("����Ƹ�����" + currentQuantity + "�� �Ǹ��ϼ̽��ϴ�");
                        break;
                    default:
                        break;
                }

                GameManager.goldcount += currentQuantity * price;

                maxQuantity -= currentQuantity;
                currentQuantity = 0;
                quantitySlider.value = 0;
                quantitySlider.maxValue = maxQuantity;
                UpdateQuantityText();
            }
            else
            {
                TurretManager.I.textfadeout.DisplayErrorMessage("0���� �ǸŰ� �Ұ����ؿ�");
            }
        }

    }
}