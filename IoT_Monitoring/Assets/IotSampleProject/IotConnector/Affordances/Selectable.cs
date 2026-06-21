using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IndustryCSE.IoT
{
    public class Selectable : MonoBehaviour
    {
        // 선택되었을 때 호출되는 이벤트(Action 델리게이트)
        public Action OnSelected;

        // UI에 표시할 리소스 라벨(TextMeshPro 텍스트)
        [SerializeField] TMP_Text _resourceLabel;

        // 마우스로 오브젝트를 클릭했을 때 호출되는 Unity 이벤트 함수
        private void OnMouseDown()
        {
            // OnSelected 이벤트가 등록되어 있으면 실행
            OnSelected?.Invoke();
        }

        // 버튼이 눌렸을 때 호출되는 함수 (UI 버튼과 연결 가능)
        public void ButtonPressed()
        {
            // OnSelected 이벤트 실행
            Debug.Log("나오남?");
            OnSelected?.Invoke();
        }

        // 라벨 텍스트를 설정하는 함수
        public void SetLabel(string label)
        {
            _resourceLabel.text = label;
        }

        // 오브젝트를 보이거나 숨기는 함수
        public void ShowOrHideSelectables(bool show)
        {
            if (show)
            {
                // 오브젝트 활성화
                this.gameObject.SetActive(true);
            }
            else
            {
                // 오브젝트 비활성화
                this.gameObject.SetActive(false);
            }
        }
    }
}
