using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
public enum classes {
    A, B, C
}

public class PlayerData : MonoBehaviour
{
    public Data data;
    [ContextMenu("To Json Data")]
    void saveDataToJson() {
        string jsonData = JsonUtility.ToJson(data);
        string path = Path.Combine(Application.dataPath, "playerData.json");
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("From Json Data")]
    void LoadDataFromJson() {
        string path = Path.Combine(Application.dataPath, "playerData.json");
        string jsonData = File.ReadAllText(path);
        data = JsonUtility.FromJson<Data>(jsonData);

    }

}

[System.Serializable]
public class Data {
    public string name;
    public int age;
    public int level;
    public classes classes_;

}