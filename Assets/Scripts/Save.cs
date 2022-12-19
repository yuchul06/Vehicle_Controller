using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveData
{
    public Vector2 pos;
}
public class SaveManager : MonoBehaviour
{
    SaveData saveData;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        saveData = new SaveData();
    }

    private void save()
    {
        saveData.pos = player.transform.position;

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.dataPath + "/data.txt", json);
    }
    
    private void load()
    {
        string loadedjson = File.ReadAllText(Application.dataPath + "/data.txt");
        saveData = JsonUtility.FromJson<SaveData>(loadedjson);

        player.transform.position = saveData.pos;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
