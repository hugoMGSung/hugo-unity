using UnityEngine;

namespace IndustryCSE.IoT
{
    // IoT 디바이스 메시지를 읽는 클래스
    // DeviceMessageReader를 상속받아 IoT 환경에 맞게 구현
    public class IotDeviceMessageReader : DeviceMessageReader
    {
        // 실제 메시지를 제공하는 프로바이더 (Inspector에서 연결)
        [SerializeField] private BaseMessageProvider _actualProvider;

        // DeviceMessageReader의 초기화 과정에서 호출되는 내부 메서드
        // 메시지 공급자를 실제 프로바이더로 설정
        protected override void InternalInit()
        {
            _messageProvider = _actualProvider;
        }

        // Unity의 Start() 메서드: 첫 프레임 업데이트 전에 호출됨
        void Start()
        {
            // 디바이스 메시지 읽기 시작
            ReadDeviceMessages();
        }
    }
}
