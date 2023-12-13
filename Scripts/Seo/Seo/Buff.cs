using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    [SerializeField] private NexusSO data;
    public static int currentMP;

    public Button DamageButton;
    public static int damagebuff = 1000;
    private float damagebufftime = 40f;
    private bool cooltime;

    public GameObject targetImageGameObject;  // 이미지가 속한 GameObject를 Inspector에서 할당하거나 Awake에서 찾을 수 있습니다.
    private Image fillImage;  // 이미지의 Image 컴포넌트

    private void Start()
    {
        cooltime = true;
        currentMP = data.MaxMP();

        if (DamageButton != null)
        {
            DamageButton.onClick.AddListener(DamageUp);
        }

        // targetImageGameObject에서 Image 컴포넌트 가져오기
        if (targetImageGameObject != null)
        {
            fillImage = targetImageGameObject.GetComponent<Image>();
        }
    }

    private void DamageUp()
    {
        TurretWeapon[] turrets = GameObject.FindObjectsOfType<TurretWeapon>();

        if (cooltime && !IsInvoking("ResetCooltime"))
        {
            if (currentMP >= 10)
            {
                currentMP -= 10;
                Debug.Log(currentMP);

                foreach (TurretWeapon turret in turrets)
                {
                    if (TurretInfo.Load.ContainsKey(turret.gameObject))
                    {
                        TurretInfo.Temp = TurretInfo.Load[turret.gameObject];
                        turret.AddBuff(damagebuff);
                    }
                }

                cooltime = false;
                Invoke("ResetCooltime", damagebufftime);
                Invoke("RemoveBuff", damagebufftime); // 40초 후에 RemoveBuff 호출

                // 이미지의 Fill Amount를 1에서 0으로 변경하는 코루틴 시작
                StartCoroutine(FillImageOverTime(1f, 0f, damagebufftime));
            }
            else
            {
                Debug.Log("엠피부족");
            }
        }
        else
        {
            Debug.Log("쿨타임중");
        }
    }

    private void ResetCooltime()
    {
        cooltime = true;
    }

    private void RemoveBuff()
    {
        TurretWeapon[] turrets = GameObject.FindObjectsOfType<TurretWeapon>();

        foreach (TurretWeapon turret in turrets)
        {
            if (TurretInfo.Load.ContainsKey(turret.gameObject))
            {
                TurretInfo.Temp = TurretInfo.Load[turret.gameObject];
                turret.RemoveBuff(damagebuff);
            }
        }
    }

    // 이미지의 Fill Amount를 변경하는 코루틴
    IEnumerator FillImageOverTime(float startFill, float endFill, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 시간에 따라 Fill Amount를 변경
            fillImage.fillAmount = Mathf.Lerp(startFill, endFill, elapsedTime / duration);

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 최종적으로 Fill Amount를 설정
        fillImage.fillAmount = endFill;
    }
}