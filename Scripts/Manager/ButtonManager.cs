using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SoundManager;

public class ButtonManager : MonoBehaviour
{
    #region Singleton
    public static ButtonManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    [Header("[인벤토리]")]
    [SerializeField] private List<Button> sellItems;
    [SerializeField] private Button extendInven;
    [SerializeField] private Button reduceInven;
    [SerializeField] private Button inventoryOpen;
    [SerializeField] private Button inventoryClose;
    [SerializeField] private List<Button> itemSellBtn;
    [SerializeField] private GameObject itemSellPopup;

    [Header("[터렛 관리]")]
    [SerializeField] private Button turretMangeOpen;       
    [SerializeField] private Button closeShop;    

    [Header("[UI 전체관리]")]
    [SerializeField] private Button allUIClose;
    [SerializeField] private GameObject generalUIOpenPopup;
    [SerializeField] private Button generalUIBtn; 

    [Header("[기타 오브젝트]")]
    [SerializeField] private GameObject generalPopup;
    [SerializeField] private GameObject inven;    
    [SerializeField] private GameObject turretBarrak;    
    [SerializeField] private GameObject minimap;

    [Header("[카메라 스위쳐]")]
    [SerializeField] private Button camera1Btn;
    public static Button camera1Btns;
    [SerializeField] private Button camera2Btn;
    [SerializeField] private CameraController cameraController;

    [Header("[옵션 세팅]")]
    [SerializeField] private Toggle cryptoUI;
    [SerializeField] private GameObject crypto;

    [Header("치트키")]
    [SerializeField] private Button goldCheat;

    public static bool roundOn;

    //private ButtonGroup allButtonGroup;

    private RectTransform rect;

    bool activeInven = false;

    
    void Start()
    {
        ButtonManager.camera1Btns = camera1Btn;
        //List<Button> allButtons = new List<Button>();
        //foreach (var field in GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
        //{
        //    if (field.FieldType == typeof(Button))
        //    {
        //        allButtons.Add((Button)field.GetValue(this));
        //    }
        //    else if (field.FieldType == typeof(List<Button>))
        //    {
        //        allButtons.AddRange((List<Button>)field.GetValue(this));
        //    }
        //}


        //allButtonGroup = new ButtonGroup(allButtons.ToArray());

        camera1Btn.onClick.AddListener(cameraController.PlayerCamera);
        camera2Btn.onClick.AddListener(cameraController.DefenceCamera);

        
        //generalPopup.SetActive(false);
        minimap.SetActive(false);
        inven.SetActive(activeInven);
        
        rect = inven.GetComponent<RectTransform>(); 
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 300);

        //confirmPopup.SetActive(false);

        //foreach (Button button in sellItems)
        //{
        //    button.onClick.AddListener(OpenConfirmPopup);
        //}
     

        extendInven.interactable = true;
        reduceInven.interactable = false;
        extendInven.onClick.AddListener(() =>
        {
            reduceInven.interactable = true;
            extendInven.interactable = false;
            ExtendInventory();
        });        
        reduceInven.onClick.AddListener(() =>
        {
            extendInven.interactable = true;
            reduceInven.interactable = false;
            ReduceInventory();
        });

      
       foreach(Button btn in itemSellBtn)
        {
            btn.onClick.AddListener(() =>  OpenItemSellPopup());
        }

        generalUIBtn.onClick.AddListener(GeneralUIOpen);
        closeShop.onClick.AddListener(CloseShop);
        inventoryOpen.onClick.AddListener(InventoryOpen);
        inventoryClose.onClick.AddListener(InventoryClose);
        turretMangeOpen.onClick.AddListener(TurretManage);
        
        allUIClose.onClick.AddListener(AllClose);        


        ToggleCrypto(cryptoUI.isOn);
        cryptoUI.onValueChanged.AddListener(ToggleCrypto);
        goldCheat.onClick.AddListener(GoldCheat);

    }

    private void GoldCheat()
    {
        GameManager.goldcount += 1000000;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1)) 
        {
            inven.SetActive(!inven.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            turretBarrak.SetActive(!turretBarrak.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            minimap.SetActive(!minimap.activeSelf);
        }
     
      
        if(Input.GetKeyDown(KeyCode.Escape))
        {
           
            inven.SetActive(false);
            minimap.SetActive(false);
            turretBarrak.SetActive(false);
            generalPopup.SetActive(false);
            generalUIOpenPopup.SetActive(true);
        }
        //if (cameraController != null)
        //{
        //    if (Input.GetKeyDown(KeyCode.Alpha1))
        //    {
        //        cameraController.PlayerCamera();
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha2))
        //    {
        //        cameraController.DefenceCamera();
        //    }
        //}
    }
    private void ToggleCrypto(bool isOn)
    {
        crypto.SetActive(isOn);
    }

    private void HandleUIInput(KeyCode keyCode, GameObject uiElement)
    {
        if (Input.GetKeyDown(keyCode))
        {
            uiElement.SetActive(true);
        }

        if (Input.GetKeyUp(keyCode))
        {
            uiElement.SetActive(false);
        }
    }

    private void OpenItemSellPopup()
    {
        if(itemSellBtn != null)
        {
            itemSellPopup.SetActive(true);
        }
        else
        {
            Debug.Log("아이템 팝업이 없습니다.");
        }

    }


   

    private void GeneralUIOpen()
    {
        generalPopup.SetActive(true);
        generalUIOpenPopup.SetActive(false);

    }

    private void AllClose()
    {      

        minimap.SetActive(false);
        inven.SetActive(false);
        turretBarrak.SetActive(false);        
        generalPopup.SetActive(false);
        generalUIOpenPopup.SetActive(true);
      
    }

    private void TurretManage()
    {
        turretBarrak.SetActive(!turretBarrak.activeSelf);
    }

    private void MapOpen()
    {
        minimap.SetActive(!minimap.activeSelf);
    }

    private void InventoryOpen()
    {

        inven.SetActive(!inven.activeSelf);
    }
    private void InventoryClose()
    {
        inven.SetActive(false);
    }


    private void CloseShop()
    {
        turretBarrak.SetActive(false);
        
    }
      
      

    private void ReduceInventory()
    {
        activeInven = true;
        inven.SetActive(activeInven);
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 300);
    }

    private void ExtendInventory()
    {
        activeInven = true; 
        inven.SetActive(activeInven); 
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 800);
    }
  

    //private void OpenConfirmPopup()
    //{
    //    confirmPopup.SetActive(true);
    //}
    //private class ButtonGroup
    //{
    //    private List<Button> buttons;

    //    public ButtonGroup(params Button[] buttons)
    //    {
    //        this.buttons = new List<Button>(buttons);

    //        foreach (Button button in buttons)
    //        {
    //            button.onClick.AddListener(() => OnButtonClick());
    //        }
    //    }

    //    private void OnButtonClick()
    //    {
    //        SoundManager.instance.SFXPlay(SFXType.blop);
    //    }
    //}
}
