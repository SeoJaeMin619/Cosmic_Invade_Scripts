using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [Header("PlayerHP")]
    public static float playerMaxHp = 100f;   
    public float playerCurHp;
    [SerializeField]
    private TextMeshProUGUI playerCurHpText;
    [SerializeField]
    private TextMeshProUGUI playerMaxHpText; 
    [SerializeField]
    Slider hpSlider;
    [SerializeField]
    private GameObject healthRecoverZone;
    public TextMeshProUGUI hpAlarmTxt;
    public static float healthDecreaseRate = 2;
    public static float healthIncreaseRate = 1;



    [Header("PlayerLife")]
    [SerializeField]
    private int maxLives = 5;
    [SerializeField]
    private int curLives;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI countdownReviveTxt;


    [Header("Oxygen")]
    public static float maxOxygen = 100f;
    [SerializeField]
    private float curOxygen;   
    [SerializeField] 
    Slider oxygenSlider;
    [SerializeField]
    private TextMeshProUGUI curOxygenText;
    [SerializeField]
    private TextMeshProUGUI maxOxygenText;
    [SerializeField]
    private GameObject OxygenRecoverZone;
    public static float oxygenDecreaseRate = 5;
    public static float oxygenIncreaseRate = 1;    

    public GameObject safePanel;
    public GameObject spacePanel;
    public GameObject hpAlarm;
    public GameObject reinforceCircle;
    public GameObject reinforcePanel;
    public GameObject spaceZone;
    public GameObject safeZone;
    public GameObject healingEffect;

    public Transform revivePosition;
        
    private bool isInSpace = false;
    private bool isInSafety = false;
    private bool isInRecoverHealth = false;
    private bool isInRecoverOxygen = false;
    
    


    private void Start()
    {
        
        playerCurHp = playerMaxHp;
        curLives = maxLives;
        curOxygen = maxOxygen;
        //maxOxygenText.text = "100"; 
        //playerMaxHpText.text = "100";

        hpAlarm.SetActive(false);
        safePanel.SetActive(false);
        spacePanel.SetActive(false);
        UpdateLife();
        
    }

    private void Update()
    {      
        
        if (isInSpace)
            {
                curOxygen -= oxygenDecreaseRate * Time.deltaTime;
                curOxygen = Mathf.Clamp(curOxygen, 0f, maxOxygen);
                UpdateOxygenSlider();
                

                if (curOxygen <=40 && curOxygen >= 30)
                {
                    hpAlarm.SetActive(true);                  
                    playerCurHp -= healthDecreaseRate * Time.deltaTime;
                    UpdateHealthSlider();
                    UpdateAlarmText();
                }
                if (curOxygen <30 && curOxygen >= 20)
                {
                    hpAlarm.SetActive(true);
                    playerCurHp -= healthDecreaseRate * 2 * Time.deltaTime;
                    UpdateHealthSlider();
                    UpdateAlarmText();
                }                
                if (curOxygen < 20 && curOxygen >= 10)
                {
                    hpAlarm.SetActive(true);
                    playerCurHp -= healthDecreaseRate * 3f * Time.deltaTime;
                    UpdateHealthSlider();
                    UpdateAlarmText();
                }   
                if (curOxygen < 10 && curOxygen >= 0)
                {
                    hpAlarm.SetActive(true);
                    playerCurHp -= healthDecreaseRate * 5f * Time.deltaTime;
                    UpdateHealthSlider();
                    UpdateAlarmText();
                }

            }
            else if (isInSafety)
            {
                // 자연회복 수치
                curOxygen += oxygenIncreaseRate * Time.deltaTime;
                curOxygen = Mathf.Clamp(curOxygen, 0f, maxOxygen);
                UpdateOxygenSlider();

                if (playerCurHp < playerMaxHp)
                {
                    playerCurHp += healthIncreaseRate * Time.deltaTime;
                    playerCurHp = Mathf.Clamp(playerCurHp, 0f, playerMaxHp);
                    UpdateHealthSlider();
                }
            }
            if (isInRecoverOxygen)
            {            
                curOxygen += oxygenIncreaseRate * 20 * Time.deltaTime;

                if(curOxygen > 0 && curOxygen <= 99)               
                {
                
                int goldCost = (int)(oxygenIncreaseRate * 250 * Time.deltaTime);
                    if(GameManager.goldcount >= goldCost)
                    {
                       GameManager.goldcount -= goldCost;
                      
                }
                    else
                    {
                        isInRecoverOxygen = false;
                    }
                healingEffect.SetActive(true);
                curOxygen = Mathf.Clamp(curOxygen, 0f, maxOxygen);
                UpdateOxygenSlider();
                }                
               
            }
            if (isInRecoverHealth)
            {          
                playerCurHp += healthIncreaseRate * 20 * Time.deltaTime;

                if(playerCurHp > 0 && playerCurHp <= 99)
                {
                
                int goldCost = (int)(healthIncreaseRate * 250 * Time.deltaTime);
                    if(GameManager.goldcount >= goldCost)
                    {
                       
                       GameManager.goldcount -= goldCost;
                    }
                
                }
            healingEffect.SetActive(true);
            playerCurHp = Mathf.Clamp(playerCurHp, 0, playerMaxHp);
                UpdateHealthSlider();
            }




            if (playerCurHp <= 0 && curLives > 0)
            {
                curLives--;
                UpdateLife();
                StartCoroutine(PlayerRevive());
            }
            if (curLives == 0)
            {                
                Destroy(this.gameObject, 2f);
            }
            UpdateMaxHealthPoint();
            UpdateMaxOxygenPoint();
       

    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject == spaceZone)
        {            
            if(spaceZone != null)
            {
                isInSpace = true;
                StartCoroutine(SetActiveForOneSecond(spacePanel, true));               
            }
            else
            {
                Debug.LogWarning("Space가 없습니다.");
            }
           
        }
        if (collision.gameObject == safeZone)
        {            
            if(safeZone != null)
            {
                isInSafety = true;
                hpAlarm.SetActive(false);
                StartCoroutine(SetActiveForOneSecond(safePanel, true));
            }
            else
            {
                Debug.LogWarning("SafeZone이 없습니다.");
            }
        }
        if (collision.gameObject == healthRecoverZone)
        {
            if (healthRecoverZone != null)
            {
                
                isInRecoverHealth = true;                
            }
            else
            {
                Debug.LogWarning("healthRecoverZone이 없습니다.");
            }
            
        }
        if (collision.gameObject == OxygenRecoverZone)
        {        
            if(OxygenRecoverZone != null)
            {
               
                isInRecoverOxygen = true;
            }
            else
            {
                Debug.LogWarning("OxygenRecoverZone이 없습니다.");
            }
           
        }

        if(collision.gameObject == reinforceCircle)
        {
            reinforcePanel.SetActive(true);
        }
                
    }  
    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == spaceZone)
        {
            if (spaceZone != null)
            {
                isInSpace = false;
                StartCoroutine(SetActiveForOneSecond(spacePanel, false));                
            }
            else
            {
                Debug.LogWarning("Space가 없습니다.");
            }

        }
        if (collision.gameObject == safeZone)
        {           
            if(safeZone != null)
            {
                isInSafety = false;
                StartCoroutine(SetActiveForOneSecond(safePanel, false));
            }
            else
            {
                Debug.LogWarning("SafeZone이 없습니다.");
            }
        }
        if (collision.gameObject == healthRecoverZone)
        {           
            if(healthRecoverZone != null)
            {
                healingEffect.SetActive(false);
                isInRecoverHealth = false;                
            }
            Debug.LogWarning("healthRecoverZone이 없습니다.");
        }
        if (collision.gameObject == OxygenRecoverZone)
        {            
            if(OxygenRecoverZone != null)
            {
                healingEffect.SetActive(false);
                isInRecoverOxygen = false;
            }
            else
            {
                Debug.LogWarning("OxygenRecoverZone이 없습니다.");                
            }
            
        }
        if(collision.gameObject == reinforceCircle)
        {
            if(reinforcePanel != null)
            {
                reinforcePanel.SetActive(false);
            }
            else
            {
                Debug.LogWarning("ReinforcePanel이 없습니다.");
                return;
            }
        }

    }   

   
    private IEnumerator PlayerRevive()
    {
        
        hpAlarm.SetActive(false);
        transform.position = new Vector3(200, 200, 0);
        playerCurHp = playerMaxHp;
        UpdateHealthSlider();

        
        StartCoroutine(ShowCountdownText(10f));
        yield return new WaitForSeconds(10f);
        
    }

    private IEnumerator ShowCountdownText(float duration)
    {
        float timer = duration;

        while (timer > 0f)
        {
            countdownReviveTxt.text = "부활 대기중 ";
            yield return null;
            timer -= Time.deltaTime;
        }

        countdownReviveTxt.text = "";
    }   

    private IEnumerator SetActiveForOneSecond(GameObject panel, bool active)
    {
        if (!hpAlarm.activeSelf)
        {
            panel.SetActive(active);
            yield return new WaitForSeconds(0.8f);
            panel.SetActive(false);
        }
    }
    

    private void UpdateLife()
    {
        livesText.text = "남은 목숨 : " + curLives.ToString();
    }
    private void UpdateOxygenSlider()
    {
        oxygenSlider.value = curOxygen / maxOxygen;
        curOxygenText.text = Mathf.RoundToInt(curOxygen).ToString();

    }
    private void UpdateHealthSlider()
    {   
        hpSlider.value = playerCurHp / playerMaxHp;
        playerCurHpText.text = Mathf.RoundToInt(playerCurHp).ToString();
    }
    public void UpdateMaxHealthPoint()
    {
        playerMaxHpText.text = " / " + Mathf.RoundToInt(playerMaxHp).ToString();
    }
    public void UpdateMaxOxygenPoint()
    {
        maxOxygenText.text = " / " + Mathf.RoundToInt(maxOxygen).ToString();
    }

    private void UpdateAlarmText()
    {
        hpAlarmTxt.text = "Hp가 감소합니다\n 산소농도: " + Mathf.RoundToInt(curOxygen).ToString() + "%";
    }
}
