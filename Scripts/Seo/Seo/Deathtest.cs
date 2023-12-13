using UnityEngine;
using UnityEngine.UI;

public class Deathtest : MonoBehaviour
{
    public RoundManager roundManager;

    public void Start()
    {
   
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(DamegeButton);
        }

    }

    private void DamegeButton()
    {

        foreach(GameObject unit in roundManager.SpawnedUnitsList)

        {
            HpSlider hpSlider = unit.GetComponent<HpSlider>();
            if (hpSlider != null)

            {
                
                unit.GetComponent<Unit>().Damaged(1000);

            }
            
        }

    }
}
