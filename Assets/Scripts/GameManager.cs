using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private JsonWriteReadSystem data;

    // 游戏一开始更新数据
    void Start()
    {
        data = GetComponent<JsonWriteReadSystem>();
        data.LoadFromJson();
        data.LoadVolumeToData();
        Application.targetFrameRate = 80;
        CharacterResetPosition.Instance.RespawnPlayer();
    }


    private void OnApplicationQuit()
    {
        data.SaveToJson();
    }

    private void OnDestroy()
    {
       data.SaveToJson();
    }
}
