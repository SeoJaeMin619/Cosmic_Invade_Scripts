using UnityEngine;

public class TurretTooltip : MonoBehaviour
{
    private bool isMouseOver = false; // 마우스가 위에 있는지 여부

    // 마우스가 들어왔을 때 호출되는 메서드
    void OnPointerEnter()
    {
        isMouseOver = true;
        TurretClick.TurretState();
    }

    // 마우스가 나갔을 때 호출되는 메서드
    void OnPointerExit()
    {
        isMouseOver = false;
        // 툴팁을 여기서 숨깁니다.
        // 텍스트를 지우거나 툴팁 오브젝트를 비활성화할 수 있습니다.
    }

    // 매 프레임마다 호출되는 메서드
    void Update()
    {
        // 선택 사항으로 툴팁 위치를 여기서 업데이트할 수 있습니다.
        if (isMouseOver)
        {
            // 마우스 위치 또는 버튼 위치를 기반으로 위치를 업데이트합니다.
            // ScreenPointToLocalPointInRectangle을 사용하는 경우 마우스 위치를 UI 공간으로 변환해야 할 수 있습니다.
        }
    }
}