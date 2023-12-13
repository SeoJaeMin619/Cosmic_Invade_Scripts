using UnityEngine;
using UnityEngine.UI;

public class ToolChanger : MonoBehaviour
{
    public GameObject toolPanel; // 도구 패널
    public Image[] toolImages; // 4개의 도구 이미지
    public GameObject[] toolObjects; // 4개의 도구 오브젝트

    private bool isPanelOpen = false; // 패널이 열려 있는지 여부

    void Update()
    {
        if (TopDownPlayerMove.isCameraFollowing)
        {
            GetMouseDown();
        }

    }

    void GetMouseDown()
    {
        // 마우스 위치 확인
        Vector3 mousePosition = Input.mousePosition;

        // 마우스 중간 버튼이 눌렸을 때 패널 열기/닫기
        if (Input.GetMouseButtonDown(2))
        {
            isPanelOpen = !isPanelOpen;
            toolPanel.SetActive(isPanelOpen);
        }
        else if (Input.GetMouseButtonUp(2))
        {
            isPanelOpen = false;
            toolPanel.SetActive(false);

            // 패널이 닫힐 때 선택된 도구 활성화
            ActivateSelectedTool();
        }

        // 원형 범위의 중앙 좌표
        Vector3 circleCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        float circleRadius = 200f; // 원형 범위의 반지름

        // 마우스와 원형 범위의 중앙 간의 거리 계산
        float distanceToCenter = Vector3.Distance(mousePosition, circleCenter);

        // 모든 스프라이트 색을 원래대로 초기화
        foreach (Image toolImage in toolImages)
        {
            ChangeColor(toolImage, Color.white);
        }

        // 마우스가 원형 범위 내에 있는 경우
        if (distanceToCenter <= circleRadius)
        {
            // 원형 범위의 각 영역을 정의 (우상, 우하, 좌하, 좌상)
            Rect[] quadrantRects = new Rect[]
            {
                new Rect(circleCenter.x, circleCenter.y, circleRadius, circleRadius), // 우상
                new Rect(circleCenter.x, circleCenter.y - circleRadius, circleRadius, circleRadius), // 우하
                new Rect(circleCenter.x - circleRadius, circleCenter.y - circleRadius, circleRadius, circleRadius), // 좌하
                new Rect(circleCenter.x - circleRadius, circleCenter.y, circleRadius, circleRadius) // 좌상
            };

            // 마우스 위치가 어느 영역에 속하는지 확인
            int currentQuadrant = -1;
            for (int i = 0; i < quadrantRects.Length; i++)
            {
                if (quadrantRects[i].Contains(mousePosition))
                {
                    currentQuadrant = i;
                    break;
                }
            }

            // 현재 마우스 위치에 따라 스프라이트의 색 변경
            if (currentQuadrant != -1)
            {
                ChangeColor(toolImages[currentQuadrant], Color.black);
            }
        }
    }

    void ChangeColor(Image toolImage, Color newColor)
    {
        // Image 컴포넌트를 사용하여 색 변경
        if (toolImage != null)
        {
            toolImage.color = newColor;
        }
    }

    void ActivateSelectedTool()
    {
        // 선택된 도구의 인덱스 찾기
        int selectedToolIndex = -1;
        for (int i = 0; i < toolImages.Length; i++)
        {
            if (toolImages[i].color == Color.black)
            {
                selectedToolIndex = i;
                break;
            }
        }

        // 선택된 도구가 있을 경우 해당 도구의 오브젝트를 활성화하고 나머지 도구 오브젝트는 비활성화
        if (selectedToolIndex != -1 && selectedToolIndex < toolObjects.Length)
        {
            for (int i = 0; i < toolObjects.Length; i++)
            {
                toolObjects[i].SetActive(i == selectedToolIndex);
            }
        }
    }
}
