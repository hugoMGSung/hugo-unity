using IndustryCSE.IoT;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AppStates : MonoBehaviour
{
    // UI 요소 및 오브젝트를 Inspector에서 연결하기 위한 직렬화된 필드들
    [SerializeField] private TMP_Text labelOverlay;          // 현재 상태를 표시하는 오버레이 텍스트
    [SerializeField] private TMP_Text labelApplicationMode;  // 애플리케이션 모드(시뮬레이션/클라우드)를 표시하는 텍스트

    [SerializeField] private GameObject messageContainer;    // 메시지를 표시할 컨테이너 오브젝트
    [SerializeField] private TMP_Text messageDisplay;        // 메시지 내용을 표시하는 텍스트

    // 애플리케이션 상태 토글을 위한 불리언 변수들
    private bool _showOccupancy;        // 좌석 점유 상태 표시 여부
    private bool _showHVAC;             // HVAC(냉난방 제어) 표시 여부
    private bool _showRoomOccupancy;    // 방 점유 상태 표시 여부
    private bool _showRoomTemperature;  // 방 온도 표시 여부
    private bool _showLocator;          // 위치 찾기 기능 표시 여부

    private string _msgInfoString;      // 메시지 버스에서 수신한 문자열 캐싱

    // 성능 최적화를 위해 캐싱된 컴포넌트 배열
    private BaseDevice[] _baseDevices;                // 씬 내의 모든 IoT 디바이스 컴포넌트
    private BaseMessageProvider[] _messageProviders;  // 메시지 공급자 컴포넌트

    private void Start()
    {
        Debug.Log("AppStates checking for devices");
        // 씬 시작 시 모든 디바이스와 메시지 공급자를 찾아 캐싱
        //_baseDevices = FindObjectsByType<BaseDevice>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        _baseDevices = Object.FindObjectsByType<BaseDevice>(FindObjectsInactive.Include);

        //_messageProviders = FindObjectsByType<BaseMessageProvider>(FindObjectsSortMode.None);
        _messageProviders = FindObjectsByType<BaseMessageProvider>();

        // 메시지 버스 구독
        SubscribeMessageBus();
    }

    private void Update()
    {
        // 메시지 컨테이너가 활성화되어 있을 때 메시지 표시 업데이트
        if (messageContainer.activeSelf)
        {
            messageDisplay.text = _msgInfoString;
        }
    }

    #region Toggle Methods
    // 좌석 점유 상태 토글
    public void ToggleOccupancy()
    {
        _showOccupancy = !_showOccupancy;
        SetState("Seating Occupancy", _showOccupancy, IndustryCSE.IoT.DeviceType.Type.Occupancy);
    }

    // HVAC 상태 토글
    public void ToggleHVAC()
    {
        _showHVAC = !_showHVAC;
        SetState("HVAC", _showHVAC, IndustryCSE.IoT.DeviceType.Type.Thermostat);
    }

    // 방 점유 상태 토글
    public void ToggleRoomOccupancy()
    {
        _showRoomOccupancy = !_showRoomOccupancy;
        SetState("Room Occupancy", _showRoomOccupancy, IndustryCSE.IoT.DeviceType.Type.OccupancyDeviceGroup);
    }

    // 방 온도 상태 토글
    public void ToggleRoomTemperature()
    {
        _showRoomTemperature = !_showRoomTemperature;
        SetState("Room Temperature", _showRoomTemperature, IndustryCSE.IoT.DeviceType.Type.ThermostatDeviceGroup);
    }

    // 위치 찾기 기능 토글
    public void ToggleLocate()
    {
        _showLocator = !_showLocator;
        messageContainer.SetActive(_showLocator); // 메시지 컨테이너 활성화/비활성화
    }
    #endregion

    #region State Management
    // 특정 상태를 설정하는 메서드
    private void SetState(string label, bool isActive, IndustryCSE.IoT.DeviceType.Type deviceType)
    {
        HideVisuals(); // 다른 상태 비활성화
        SetDeviceVisibility(isActive, deviceType); // 해당 디바이스 타입만 표시
        if (isActive)
        {
            labelOverlay.text = label; // 상태 라벨 표시
            Debug.Log($"State changed: {label} is now {(isActive ? "ON" : "OFF")}");
        }
        else
        {
            labelOverlay.text = string.Empty; // 상태 라벨 제거
        }
    }

    // 특정 디바이스 타입의 표시 여부를 설정
    private void SetDeviceVisibility(bool isActive, IndustryCSE.IoT.DeviceType.Type deviceType)
    {
        foreach (BaseDevice component in _baseDevices)
        {
            if (component.DeviceType == deviceType)
            {
                component.SetVisibility(isActive);
            }
        }
    }

    // 모든 디바이스 시각화를 숨김
    private void HideVisuals()
    {
        SetDeviceVisibility(false, IndustryCSE.IoT.DeviceType.Type.Occupancy);
        SetDeviceVisibility(false, IndustryCSE.IoT.DeviceType.Type.Thermostat);
        SetDeviceVisibility(false, IndustryCSE.IoT.DeviceType.Type.OccupancyDeviceGroup);
        SetDeviceVisibility(false, IndustryCSE.IoT.DeviceType.Type.ThermostatDeviceGroup);
    }
    #endregion

    #region Application Mode
    /// <summary>
    /// 시뮬레이션 데이터 모드로 전환
    /// </summary>
    public void SimulateData()
    {
        labelApplicationMode.text = "Simulated"; // 모드 라벨 변경
        SetMessageProviderMode(true);           // 메시지 공급자를 시뮬레이션 모드로 설정
    }

    /// <summary>
    /// IoT 데이터 스트림 모드로 전환
    /// </summary>
    public void ActivateIotBackend()
    {
        labelApplicationMode.text = "Cloud";    // 모드 라벨 변경
        SetMessageProviderMode(false);          // 메시지 공급자를 실제 IoT 모드로 설정
    }

    // 메시지 공급자의 모드를 설정 (시뮬레이션/IoT)
    private void SetMessageProviderMode(bool simulate)
    {
        foreach (BaseMessageProvider provider in _messageProviders)
        {
            provider.SetModeAsync(simulate);
        }
    }
    #endregion

    // 메시지 버스 구독
    private void SubscribeMessageBus()
    {
        if (IotDeviceMessageReader.Instance != null)
            IotDeviceMessageReader.Instance.MessageBus.Subscribe<DeviceMessage>(OnDeviceMessageReceived);
    }

    // 메시지 수신 시 호출되는 콜백
    private void OnDeviceMessageReceived(IndustryCSE.IoT.Messenger.IMessagePublisher publisher, DeviceMessage msg)
    {
        _msgInfoString = msg.ValueAsString(); // 메시지를 문자열로 변환하여 캐싱
    }
}
