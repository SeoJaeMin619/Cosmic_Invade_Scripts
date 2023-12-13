using UnityEngine;
using UnityEngine.UI;

public class ToolChanger : MonoBehaviour
{
    public GameObject toolPanel; // ���� �г�
    public Image[] toolImages; // 4���� ���� �̹���
    public GameObject[] toolObjects; // 4���� ���� ������Ʈ

    private bool isPanelOpen = false; // �г��� ���� �ִ��� ����

    void Update()
    {
        if (TopDownPlayerMove.isCameraFollowing)
        {
            GetMouseDown();
        }

    }

    void GetMouseDown()
    {
        // ���콺 ��ġ Ȯ��
        Vector3 mousePosition = Input.mousePosition;

        // ���콺 �߰� ��ư�� ������ �� �г� ����/�ݱ�
        if (Input.GetMouseButtonDown(2))
        {
            isPanelOpen = !isPanelOpen;
            toolPanel.SetActive(isPanelOpen);
        }
        else if (Input.GetMouseButtonUp(2))
        {
            isPanelOpen = false;
            toolPanel.SetActive(false);

            // �г��� ���� �� ���õ� ���� Ȱ��ȭ
            ActivateSelectedTool();
        }

        // ���� ������ �߾� ��ǥ
        Vector3 circleCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        float circleRadius = 200f; // ���� ������ ������

        // ���콺�� ���� ������ �߾� ���� �Ÿ� ���
        float distanceToCenter = Vector3.Distance(mousePosition, circleCenter);

        // ��� ��������Ʈ ���� ������� �ʱ�ȭ
        foreach (Image toolImage in toolImages)
        {
            ChangeColor(toolImage, Color.white);
        }

        // ���콺�� ���� ���� ���� �ִ� ���
        if (distanceToCenter <= circleRadius)
        {
            // ���� ������ �� ������ ���� (���, ����, ����, �»�)
            Rect[] quadrantRects = new Rect[]
            {
                new Rect(circleCenter.x, circleCenter.y, circleRadius, circleRadius), // ���
                new Rect(circleCenter.x, circleCenter.y - circleRadius, circleRadius, circleRadius), // ����
                new Rect(circleCenter.x - circleRadius, circleCenter.y - circleRadius, circleRadius, circleRadius), // ����
                new Rect(circleCenter.x - circleRadius, circleCenter.y, circleRadius, circleRadius) // �»�
            };

            // ���콺 ��ġ�� ��� ������ ���ϴ��� Ȯ��
            int currentQuadrant = -1;
            for (int i = 0; i < quadrantRects.Length; i++)
            {
                if (quadrantRects[i].Contains(mousePosition))
                {
                    currentQuadrant = i;
                    break;
                }
            }

            // ���� ���콺 ��ġ�� ���� ��������Ʈ�� �� ����
            if (currentQuadrant != -1)
            {
                ChangeColor(toolImages[currentQuadrant], Color.black);
            }
        }
    }

    void ChangeColor(Image toolImage, Color newColor)
    {
        // Image ������Ʈ�� ����Ͽ� �� ����
        if (toolImage != null)
        {
            toolImage.color = newColor;
        }
    }

    void ActivateSelectedTool()
    {
        // ���õ� ������ �ε��� ã��
        int selectedToolIndex = -1;
        for (int i = 0; i < toolImages.Length; i++)
        {
            if (toolImages[i].color == Color.black)
            {
                selectedToolIndex = i;
                break;
            }
        }

        // ���õ� ������ ���� ��� �ش� ������ ������Ʈ�� Ȱ��ȭ�ϰ� ������ ���� ������Ʈ�� ��Ȱ��ȭ
        if (selectedToolIndex != -1 && selectedToolIndex < toolObjects.Length)
        {
            for (int i = 0; i < toolObjects.Length; i++)
            {
                toolObjects[i].SetActive(i == selectedToolIndex);
            }
        }
    }
}
