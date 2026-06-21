using System.Reflection;
using TMPro;
using UnityEngine;

namespace IndustryCSE.IoT
{
    /// <summary>
    /// 컨텍스트 윈도우 스크립트: 리스트 뷰에 동적 항목을 표시하고 포맷을 개선하기 위한 클래스
    /// </summary>
    public class ContextWindow : MonoBehaviour
    {
        /// <summary>
        /// 컨텍스트 윈도우의 제목 텍스트 (디바이스 ID 표시용)
        /// </summary>
        [Header("Header UI Elements")]
        [SerializeField] private TMP_Text deviceID;

        /// <summary>
        /// 항목들이 표시될 스크롤 뷰 컨테이너
        /// </summary>
        [Header("List Container")]
        [SerializeField] protected GameObject list;

        /// <summary>
        /// 헤더 텍스트에 디바이스 ID를 설정
        /// </summary>
        /// <param name="deviceId">표시할 디바이스 식별자</param>
        public void SetDeviceID(string deviceId)
        {
            if (deviceID != null)
            {
                deviceID.text = $"Device ID: {deviceId}";
            }
        }

        /// <summary>
        /// 리스트 뷰의 모든 항목을 제거
        /// </summary>
        public virtual void ClearContext()
        {
            if (list != null)
            {
                // 리스트의 자식 오브젝트들을 역순으로 제거
                for (int i = list.transform.childCount - 1; i >= 0; i--)
                {
                    Transform child = list.transform.GetChild(i);
                    Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// 새로운 항목을 컨텍스트 뷰에 추가
        /// </summary>
        /// <param name="label">항목의 라벨</param>
        /// <param name="content">라벨과 관련된 내용</param>
        public virtual void AddToContext(string label, string content)
        {
            if (!string.IsNullOrEmpty(label) && !string.IsNullOrEmpty(content))
            {
                CreateTMPTextEntry(list, label, content, 28f);
            }
        }

        /// <summary>
        /// TMP_Text 항목을 생성하여 리스트 뷰에 추가
        /// </summary>
        /// <param name="parent">항목을 추가할 부모 오브젝트</param>
        /// <param name="label">항목 라벨</param>
        /// <param name="content">항목 내용</param>
        /// <param name="fontSize">텍스트 폰트 크기</param>
        private void CreateTMPTextEntry(GameObject parent, string label, string content, float fontSize)
        {
            if (parent == null) return;

            // 새로운 항목용 GameObject 생성
            GameObject textEntry = new GameObject($"{label}_Entry");
            textEntry.transform.SetParent(parent.transform, false);

            // TMP_Text 컴포넌트 추가 및 설정
            TextMeshProUGUI tmp = textEntry.AddComponent<TextMeshProUGUI>();
            tmp.text = $"<b>{label}:</b> {content}"; // 라벨은 굵게 표시
            tmp.fontSize = fontSize;
            tmp.color = new Color(0.9f, 0.9f, 0.9f); // 약간의 오프 화이트 색상으로 UI 대비 향상
            tmp.alignment = TextAlignmentOptions.Left; // 왼쪽 정렬
            tmp.margin = new Vector4(10, 5, 10, 5); // 여백 설정으로 가독성 향상

            // 레이아웃 컴포넌트 설정 (세로 레이아웃에서 정렬 및 간격 개선)
            RectTransform rectTransform = textEntry.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(0, fontSize * 2); // 폰트 크기에 따라 높이 동적 조정
        }
    }
}
