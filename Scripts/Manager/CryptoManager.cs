using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CryptoCurrency
{
    public string name;
    public int currentPrice;
    public int previousPrice;
    public Text priceText;

    public int minPrice;
    public int maxPrice;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
}

public class CryptoManager : MonoBehaviour
{
    public static CryptoManager I;
    public CryptoCurrency[] cryptocurrencies = new CryptoCurrency[6];

    public void Awake()
    {
        I = this;
    }

    void Start()
    {
        for (int i = 0; i < cryptocurrencies.Length; i++)
        {
            cryptocurrencies[i] = new CryptoCurrency
            {
                previousPrice = 0,
                priceText = GameObject.Find("Crypto" + (i + 1) + "Text").GetComponent<Text>(),
                minPrice = GameManager.GetCryptoPrice(i) / 2,
                maxPrice = GameManager.GetCryptoPrice(i) * 3,
            };

            cryptocurrencies[i].Name = "";
            cryptocurrencies[i].currentPrice = GameManager.GetCryptoPrice(i);

            UpdateCryptoPrices(cryptocurrencies[i]);
        }

        // InvokeRepeating("UpdateCryptoPrices", 60f, 60f);
    }

    public void UpdateCryptoPrices(CryptoCurrency crypto)
    {
        crypto.previousPrice = crypto.currentPrice;
        crypto.currentPrice = GenerateNewPrice(crypto);
        UpdateUI(crypto);
    }

    int GenerateNewPrice(CryptoCurrency crypto)
    {
        float randomFactor = Random.Range(-0.5f, 0.5f);
        int newPrice = Mathf.RoundToInt(crypto.previousPrice * (1f + randomFactor));

        newPrice = Mathf.Clamp(newPrice, crypto.minPrice, crypto.maxPrice);

        return newPrice;
    }

    void UpdateUI(CryptoCurrency crypto)
    {
        if (crypto.priceText != null)
        {
            int priceChange = crypto.currentPrice - crypto.previousPrice;
            Color textColor = crypto.currentPrice > crypto.previousPrice ? Color.green : (crypto.currentPrice < crypto.previousPrice ? Color.red : Color.white);

            string priceChangeString = (priceChange >= 0) ? " + " + priceChange.ToString() : " - " + Mathf.Abs(priceChange).ToString();
            string fullText = crypto.Name + "$ " + crypto.currentPrice.ToString() + priceChangeString;

            string coloredPart = (priceChange >= 0) ? "    + " + priceChange.ToString() : "    - " + Mathf.Abs(priceChange).ToString();
            string coloredText = "<color=" + (priceChange >= 0 ? "green" : "red") + ">" + coloredPart + "</color>";

            fullText = crypto.Name + "$ " + crypto.currentPrice.ToString() + coloredText;
            crypto.priceText.text = fullText;
        }
    }
}