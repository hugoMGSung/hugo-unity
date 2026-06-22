using System;
using UnityEngine;
using TMPro;

public class SmartHomeManager : MonoBehaviour
{
    public GameObject bed;
    public GameObject bath;
    public GameObject living;
    public GameObject dining;

    public TMP_Text bedText;

    public void UpdateSensorData(string json)
    {
        try
        {
            string wrappedJson = "{\"rooms\":" + json + "}";

            SensorList sensorList =
                JsonUtility.FromJson<SensorList>(wrappedJson);

            foreach (SensorData room in sensorList.rooms)
            {
                UpdateRoom(room);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private string bedInfo;
    private string bathInfo;
    private string livingInfo;
    private string diningInfo;

    private void UpdateRoom(SensorData room) { 

        string info =
            $"{room.RoomName}\n" +
            $"온도 : {room.Temp:F1}℃\n" +
            $"습도 : {room.Humid:F1}%";

        GameObject target = null;


        switch (room.RoomName)
        {
            case "BED":
                target = bed;
                bedInfo = info;
                bedText.text = info;
                break;

            case "BATH":
                target = bath;
                bathInfo = info;
                break;

            case "LIVING":
                target = living;
                livingInfo = info;
                break;

            case "DINING":
                target = dining;
                diningInfo = info;
                break;
        }

        if (target == null) return;

        Renderer renderer = target.GetComponent<Renderer>();

        if (room.Temp < 24)
        {
            renderer.material.color = Color.blue;
        }
        else if (room.Temp < 27)
        {
            renderer.material.color = Color.green;
        }
        else
        {
            renderer.material.color = Color.red;
        }

        Debug.Log(
            $"{room.RoomName} " +
            $"{room.Temp}℃ " +
            $"{room.Humid}%");
    }
}

[Serializable]
public class SensorData
{
    public string HomeId;
    public string RoomName;
    public string SensingDateTime;
    public float Temp;
    public float Humid;
}

[Serializable]
public class SensorList
{
    public SensorData[] rooms;
}