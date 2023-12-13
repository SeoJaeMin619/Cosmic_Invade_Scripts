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
                name.text = "다이아";
                break;
            case 1:
                name.text = "루비";
                break;
            case 2:
                name.text = "에메랄드";
                break;
            case 3:
                name.text = "사파이어";
                break;
            case 4:
                name.text = "자수정";
                break;
            case 5:
                name.text = "아쿠아마린";
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
        quantityText.text = "판매 갯수: " + currentQuantity + "\n" + currentQuantity * price + "원에 판매하시겠습니까?";
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
                        TurretManager.I.textfadeout.DisplayErrorMessage("다이아를" + currentQuantity + "개 판매하셨습니다") ;
                        break;
                    case 1:
                        GameManager.rubyCount -= currentQuantity;
                        TurretManager.I.textfadeout.DisplayErrorMessage("루비를" + currentQuantity + "개 판매하셨습니다");
                        break;
                    case 2:
                        GameManager.emeraldCount -= currentQuantity;
                        TurretManager.I.textfadeout.DisplayErrorMessage("에메랄드를" + currentQuantity + "개 판매하셨습니다");
                        break;
                    case 3:
                        GameManager.sapphireCount -= currentQuantity;
                        TurretManager.I.textfadeout.DisplayErrorMessage("사파이어를" + currentQuantity + "개 판매하셨습니다");
                        break;
                    case 4:
                        GameManager.amethystCount -= currentQuantity;
                        TurretManager.I.textfadeout.DisplayErrorMessage("자수정을" + currentQuantity + "개 판매하셨습니다");
                        break;
                    case 5:
                        GameManager.aquamarineCount -= currentQuantity;
                        TurretManager.I.textfadeout.DisplayErrorMessage("아쿠아마린을" + currentQuantity + "개 판매하셨습니다");
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
                TurretManager.I.textfadeout.DisplayErrorMessage("0개는 판매가 불가능해요");
            }
        }

    }
}