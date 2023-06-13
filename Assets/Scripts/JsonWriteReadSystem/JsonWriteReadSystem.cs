using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonWriteReadSystem : MonoBehaviour
{

    // 保存数据转化为json保存到本地
    public void SaveToJson()
    {
        Data data = new Data();
        data.health = PlayerData.Instance.Health;
        data.coinCount = PlayerData.Instance.cointCount;
        data.playerCurrentPos = CharacterResetPosition.Instance.CurrentPos();

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/Data/DataFile.json", json);
    }

    // 读取本地文件从json转化为数据
    public void LoadFromJson()
    {
        string filePath = Application.dataPath + "/Data/DataFile.json";

        // 判断本地是否有该文件，否则就对游戏数据初始化，并创建新文件保存在本地
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Data data = JsonUtility.FromJson<Data>(json);
            PlayerData.Instance.Health = data.health;
            PlayerData.Instance.cointCount = data.coinCount;

            CharacterResetPosition.Instance.ResetData();
            CharacterResetPosition.Instance.UpdatePlayerRespawnPos(data.playerCurrentPos);
            
        }
        else
        {
            PlayerData.Instance.Init(5, 0);
            CharacterResetPosition.Instance.ResetData();
            SaveToJson();
        }
    }

    public void SaveVolumeToJson()
    {
        VolumeData data = new VolumeData();
        data.globalVolume = AudioManager.instance.GlobalVolume;
        data.bgVolume = AudioManager.instance.BgVolume;
        data.simpleVolume = AudioManager.instance.SimpleVolume;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/Data/VolumeData.json", json);
    }


    public void LoadVolumeToData()
    {
        string filePath = Application.dataPath + "/Data/VolumeData.json";

        // 判断本地是否有该文件，否则就对游戏数据初始化，并创建新文件保存在本地
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            VolumeData data = JsonUtility.FromJson<VolumeData>(json);

            AudioManager.instance.GlobalVolume = data.globalVolume;
            AudioManager.instance.BgVolume = data.bgVolume;
            AudioManager.instance.SimpleVolume = data.simpleVolume;
        }
        else
        {
            SaveVolumeToJson();
        }
    }
}
