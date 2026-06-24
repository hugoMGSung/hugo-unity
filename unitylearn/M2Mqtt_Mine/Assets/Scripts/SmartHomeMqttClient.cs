using System.Collections.Generic;
using System.Text;
using M2MqttUnity;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;

public class SmartHomeMqttClient : M2MqttUnityClient
{
    [Header("Subscribe Topic")]
    public string topic = "home/sensor";

    private readonly List<string> receivedMessages = new List<string>();

    protected override void Start()
    {
        brokerAddress = "192.168.0.2";
        brokerPort = 1883;
        autoConnect = true;

        mqttUserName = "root";
        mqttPassword = "mqtt123456";

        base.Start();
    }

    protected override void SubscribeTopics()
    {
        client.Subscribe(
            new string[] { topic },
            new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE }
        );

        Debug.Log("Subscribed: " + topic);
    }

    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { topic });
    }

    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = Encoding.UTF8.GetString(message);

        Debug.Log("MQTT Topic: " + topic);
        Debug.Log(msg);

        receivedMessages.Add(msg);
    }

    private void Update()
    {
        base.Update();

        if (receivedMessages.Count > 0)
        {
            string msg = receivedMessages[0];
            receivedMessages.RemoveAt(0);

            // ���⼭ SmartHomeManager�� ����
            SmartHomeManager manager = FindAnyObjectByType<SmartHomeManager>();

            if (manager != null)
            {
                manager.UpdateSensorData(msg);
            }
        }
    }
}