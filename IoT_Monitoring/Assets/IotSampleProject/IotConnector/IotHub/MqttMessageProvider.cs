using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using M2Mqtt;
using M2Mqtt.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace IndustryCSE.IoT
{
    /// <summary>
    /// MqttMessageProvider 클래스는 IoT 메시지를 MQTT 브로커에서 받아오거나
    /// 시뮬레이션 모드로 전환하여 가상의 메시지를 처리할 수 있도록 하는 기능을 제공한다.
    /// </summary>
    public class MqttMessageProvider : BaseMessageProvider
    {
        // MQTT 브로커 접속 정보 (Inspector에서 설정)
        [SerializeField] private string _mqttBrokerEndpoint = "<YOUR_MQTT_BROKER_ADDRESS>";
        [SerializeField] private int _mqttBrokerPort = 1883;
        [SerializeField] private string _mqttSubTopic = "<YOUR_MQTT_Subscription_Topic>";
        [SerializeField] private string _mqttUserName = "<YOUR_MQTT_User_Name>";
        [SerializeField] private string _mqttPassword = "<YOUR_MQTT_Password>";

        [SerializeField] private bool _secured = true; // TLS 보안 연결 여부

        private MqttClient _mqttClient; // MQTT 클라이언트 객체
        private string _clientId;       // 클라이언트 ID
        private bool _isConnected;      // 연결 상태

        /// <summary>
        /// 시뮬레이션 모드와 MQTT 모드를 전환하는 메서드
        /// </summary>
        public override async Task SetModeAsync(bool useSimulatedEvents)
        {
            _simulateEvents = useSimulatedEvents;

            if (_simulateEvents)
            {
                Debug.Log("시뮬레이션 모드로 전환...");

                // MQTT 연결 해제
                DisconnectClient();

                // 시뮬레이션 메시지 콜백 등록
                _deviceSimulator.OnDeviceMessage += ReadSimulatedMessage;
            }
            else
            {
                Debug.Log("MQTT 모드로 전환...");

                // 시뮬레이션 이벤트 비활성화
                _deviceSimulator.OnDeviceMessage -= ReadSimulatedMessage;

                // MQTT 연결 초기화
                await InitializeAsync();
            }
        }

        /// <summary>
        /// 초기화: 시뮬레이션 모드 또는 MQTT 모드에 따라 설정
        /// </summary>
        public override async Task InitializeAsync()
        {
            if (_simulateEvents)
            {
                Debug.Log("시뮬레이션 모드 초기화...");
                _deviceSimulator.OnDeviceMessage += ReadSimulatedMessage;
                return;
            }

            Debug.Log("MQTT 모드 초기화...");
            await InitializeMqttClientAsync();
        }

        /// <summary>
        /// MQTT 클라이언트를 비동기적으로 초기화
        /// </summary>
        private async Task InitializeMqttClientAsync()
        {
            try
            {
                _clientId = Guid.NewGuid().ToString();

                // 보안 여부에 따라 TLS 연결 여부 결정
                if (!_secured)
                    _mqttClient = new MqttClient(_mqttBrokerEndpoint, _mqttBrokerPort, false, null, null, MqttSslProtocols.None);
                else
                    _mqttClient = new MqttClient(_mqttBrokerEndpoint, _mqttBrokerPort, true, null, null, MqttSslProtocols.TLSv1_2);

                // 메시지 수신 이벤트 등록
                _mqttClient.MqttMsgPublishReceived += OnMqttMsgPublishReceived;

                Debug.Log("MQTT 브로커에 연결 시도...");

                // 연결 시도 (비동기)
                Task connectTask = Task.Run(() => _mqttClient.Connect(_clientId, _mqttUserName, _mqttPassword));
                await connectTask;

                if (_mqttClient.IsConnected)
                {
                    _isConnected = true;
                    SubscribeToTopic(_mqttSubTopic); // 토픽 구독
                    Debug.Log("MQTT 클라이언트 연결 성공");
                }
                else
                {
                    Debug.LogError("MQTT 브로커 연결 실패");
                }
            }
            catch (SocketException se)
            {
                Debug.LogError($"[SocketException] MQTT 브로커 연결 실패: {se.Message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Exception] MQTT 클라이언트 초기화 실패: {e.Message}");
            }
        }

        /// <summary>
        /// 특정 MQTT 토픽 구독
        /// </summary>
        private void SubscribeToTopic(string topic)
        {
            if (!_isConnected || _mqttClient == null) return;

            try
            {
                _mqttClient.Subscribe(new[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                Debug.Log($"MQTT 토픽 구독 완료: {topic}");
            }
            catch (Exception e)
            {
                Debug.LogError($"MQTT 토픽 '{topic}' 구독 실패: {e.Message}");
            }
        }

        /// <summary>
        /// 시뮬레이션 모드에서 메시지 읽기
        /// </summary>
        private void ReadSimulatedMessage(string message)
        {
            if (IsPaused) return;

            DeviceMessage deviceMessage = CreateDeviceMessage(message);

            if (deviceMessage != null)
            {
                MessageReceived(deviceMessage);
            }
        }

        /// <summary>
        /// MQTT 메시지 수신 처리
        /// </summary>
        private void OnMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if (IsPaused) return;

            try
            {
                string message = Encoding.UTF8.GetString(e.Message);
                DeviceMessage deviceMessage = CreateDeviceMessage(message);

                if (deviceMessage != null)
                {
                    MessageReceived(deviceMessage);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"MQTT 메시지 처리 오류: {ex.Message}");
            }
        }

        /// <summary>
        /// MQTT 메시지를 DeviceMessage 객체로 변환
        /// </summary>
        private DeviceMessage CreateDeviceMessage(string message)
        {
            string deviceId = "";

            try
            {
                JObject body = JsonConvert.DeserializeObject<JObject>(message);
                if (body.TryGetValue("deviceId", out JToken device_id) && !string.IsNullOrEmpty(device_id?.ToString()))
                {
                    deviceId = device_id.ToString();
                }
            }
            catch (Exception)
            {
                Debug.LogWarning("잘못된 MQTT 메시지: 'deviceId' 없음");
            }

            return new DeviceMessage(message) { DeviceId = deviceId };
        }

        /// <summary>
        /// 애플리케이션 종료 시 리소스 정리
        /// </summary>
        private void OnApplicationQuit()
        {
            DisconnectClient();
        }

        /// <summary>
        /// MQTT 클라이언트 연결 해제
        /// </summary>
        private void DisconnectClient()
        {
            if (_mqttClient != null)
            {
                try
                {
                    if (_mqttClient.IsConnected)
                    {
                        _mqttClient.Disconnect();
                        Debug.Log("MQTT 클라이언트 연결 해제 완료");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"MQTT 클라이언트 연결 해제 오류: {e.Message}");
                }
                finally
                {
                    _mqttClient = null;
                }
            }
        }
    }
}
