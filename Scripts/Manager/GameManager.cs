using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // GameManager의 싱글톤 인스턴스   

    [Header("아이템 수량")]
    [SerializeField] private TextMeshProUGUI goldTxtInven;
    [SerializeField] private TextMeshProUGUI goldTxtGUI;
    [SerializeField] private TextMeshProUGUI diamondTxtGUI;
    [SerializeField] private TextMeshProUGUI rubyTxtGUI;
    [SerializeField] private TextMeshProUGUI emeraldTxtGUI;
    [SerializeField] private TextMeshProUGUI sapphireTxtGUI;
    [SerializeField] private TextMeshProUGUI amethystTxtGUI;
    [SerializeField] private TextMeshProUGUI aqumarineTxtGUI;


    [Header("아이템 판매가격")]
    [SerializeField] private TextMeshProUGUI diamondPriceTxt;
    [SerializeField] private TextMeshProUGUI rubyPriceTxt;
    [SerializeField] private TextMeshProUGUI emeraldPriceTxt;
    [SerializeField] private TextMeshProUGUI sapphirePriceTxt;
    [SerializeField] private TextMeshProUGUI amethystPriceTxt;
    [SerializeField] private TextMeshProUGUI aquamarinePriceTxt;

    //[SerializeField] private Text Testtxt;

    public GameObject hpSlider;
    public static int goldcount;

    public static int diamondCount;
    public static int rubyCount;
    public static int emeraldCount;
    public static int sapphireCount;
    public static int amethystCount;
    public static int aquamarineCount;

    // ↑보석의 유동가격 ↓보석의 초기 가격
    public static int diamondPrice = 300;
    public static int rubyPrice = 300;
    public static int emeraldPrice = 300;
    public static int sapphirePrice = 150;
    public static int amethystPrice = 150;
    public static int aquamarinePrice = 150;


    private float timer = 0.0f;
    private float interval = 2.0f;
    private bool isBuildSetting = false;

    public UIMenuManager uIMenuManager;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (transform.parent == null)
                DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        goldcount = 1000;
        //diamondCount = 1;
        //rubyCount = 2;
        //emeraldCount = 3;
        //sapphireCount = 4;
        //amethystCount = 5;
        //aquamarineCount = 6;

    }

    public static int GetCryptoPrice(int cryptoIndex)
    {
        switch (cryptoIndex)
        {
            case 0: return diamondPrice;
            case 1: return rubyPrice;
            case 2: return emeraldPrice;
            case 3: return sapphirePrice;
            case 4: return amethystPrice;
            case 5: return aquamarinePrice;
            default: return 0;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            goldcount++;
            timer = 0.0f;
        }

        diamondPrice = CryptoManager.I.cryptocurrencies[0].currentPrice;
        rubyPrice = CryptoManager.I.cryptocurrencies[1].currentPrice;
        emeraldPrice = CryptoManager.I.cryptocurrencies[2].currentPrice;
        sapphirePrice = CryptoManager.I.cryptocurrencies[3].currentPrice;
        amethystPrice = CryptoManager.I.cryptocurrencies[4].currentPrice;
        aquamarinePrice = CryptoManager.I.cryptocurrencies[5].currentPrice;

        diamondTxtGUI.text = diamondCount.ToString();
        diamondPriceTxt.text = "$" + diamondPrice.ToString();

        rubyTxtGUI.text = rubyCount.ToString();
        rubyPriceTxt.text = "$" + rubyPrice.ToString();

        emeraldTxtGUI.text = emeraldCount.ToString();
        emeraldPriceTxt.text = "$" + emeraldPrice.ToString();

        sapphireTxtGUI.text = sapphireCount.ToString();
        sapphirePriceTxt.text = "$" + sapphirePrice.ToString();

        amethystTxtGUI.text = amethystCount.ToString();
        amethystPriceTxt.text = "$" + amethystPrice.ToString();

        aqumarineTxtGUI.text = aquamarineCount.ToString();
        aquamarinePriceTxt.text = "$" + aquamarinePrice.ToString();

      
            goldTxtInven.text = goldcount.ToString();
            goldTxtGUI.text = goldcount.ToString();
        
       


    }

    public void AddGold(int amount)
    {
        goldcount += amount;
    }

    public void AddDiamond(int amount)
    {
        diamondCount += amount;

    }
    public void AddRuby(int amount)
    {
        rubyCount += amount;

    }
    public void AddAmethyst(int amount)
    {
        amethystCount += amount;

    }

    public void AddEmerald(int amount)
    {
        emeraldCount += amount;

    }
    public void AddSapphire(int amount)
    {
        sapphireCount += amount;

    }
    public void AddJem(int amount)
    {
        aquamarineCount += amount;

    }

    public void GameOver()
    {
        if (uIMenuManager != null)
            uIMenuManager.ShowGameOverPanel();
        else
        {
            uIMenuManager = null;
            Debug.Log("GameManager의 inspector창의 uiMenuManager 없음.");
        }
    }
    


}


