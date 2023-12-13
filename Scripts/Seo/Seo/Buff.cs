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

    public GameObject targetImageGameObject;  // �̹����� ���� GameObject�� Inspector���� �Ҵ��ϰų� Awake���� ã�� �� �ֽ��ϴ�.
    private Image fillImage;  // �̹����� Image ������Ʈ

    private void Start()
    {
        cooltime = true;
        currentMP = data.MaxMP();

        if (DamageButton != null)
        {
            DamageButton.onClick.AddListener(DamageUp);
        }

        // targetImageGameObject���� Image ������Ʈ ��������
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
                Invoke("RemoveBuff", damagebufftime); // 40�� �Ŀ� RemoveBuff ȣ��

                // �̹����� Fill Amount�� 1���� 0���� �����ϴ� �ڷ�ƾ ����
                StartCoroutine(FillImageOverTime(1f, 0f, damagebufftime));
            }
            else
            {
                Debug.Log("���Ǻ���");
            }
        }
        else
        {
            Debug.Log("��Ÿ����");
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

    // �̹����� Fill Amount�� �����ϴ� �ڷ�ƾ
    IEnumerator FillImageOverTime(float startFill, float endFill, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // �ð��� ���� Fill Amount�� ����
            fillImage.fillAmount = Mathf.Lerp(startFill, endFill, elapsedTime / duration);

            // ��� �ð� ������Ʈ
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // ���������� Fill Amount�� ����
        fillImage.fillAmount = endFill;
    }
}