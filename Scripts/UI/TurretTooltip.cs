using UnityEngine;

public class TurretTooltip : MonoBehaviour
{
    private bool isMouseOver = false; // ���콺�� ���� �ִ��� ����

    // ���콺�� ������ �� ȣ��Ǵ� �޼���
    void OnPointerEnter()
    {
        isMouseOver = true;
        TurretClick.TurretState();
    }

    // ���콺�� ������ �� ȣ��Ǵ� �޼���
    void OnPointerExit()
    {
        isMouseOver = false;
        // ������ ���⼭ ����ϴ�.
        // �ؽ�Ʈ�� ����ų� ���� ������Ʈ�� ��Ȱ��ȭ�� �� �ֽ��ϴ�.
    }

    // �� �����Ӹ��� ȣ��Ǵ� �޼���
    void Update()
    {
        // ���� �������� ���� ��ġ�� ���⼭ ������Ʈ�� �� �ֽ��ϴ�.
        if (isMouseOver)
        {
            // ���콺 ��ġ �Ǵ� ��ư ��ġ�� ������� ��ġ�� ������Ʈ�մϴ�.
            // ScreenPointToLocalPointInRectangle�� ����ϴ� ��� ���콺 ��ġ�� UI �������� ��ȯ�ؾ� �� �� �ֽ��ϴ�.
        }
    }
}