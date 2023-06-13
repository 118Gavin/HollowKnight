using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterResetPosition : MonoBehaviour
{
    [SerializeField]
    private SharedTransform[] playerRespawnsPos;

    public Dictionary<SharedTransform, bool> playerRespawnPosRealTime; // bool 值为false表示角色没去过这个复活点

    public static CharacterResetPosition Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (playerRespawnPosRealTime == null)
        {
            playerRespawnPosRealTime = new Dictionary<SharedTransform, bool>();
        }
    }

    public void RespawnPlayer()
    {
        for (int i = playerRespawnPosRealTime.Count - 1; i >= 0; i--)
        {
            if (playerRespawnPosRealTime[playerRespawnsPos[i]] == true)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = playerRespawnsPos[i].Value.position;
                return;
            }
        }
    }

    public void EnterPoint(int PosCount)
    {
        playerRespawnPosRealTime[playerRespawnsPos[PosCount]] = true;
    }

    public void ResetData()
    {
        foreach (var playerPos in playerRespawnsPos)
        {
            playerRespawnPosRealTime[playerPos] = false;
        }

        playerRespawnPosRealTime[playerRespawnsPos[0]] = true;
    }

    // 返回当前玩家重生点
    public int CurrentPos()
    {
        int result = 0;
        for (int i = playerRespawnPosRealTime.Count - 1; i >= 0; i--)
        {
            if (playerRespawnPosRealTime[playerRespawnsPos[i]] == true)
            {
                result = i;
                break;
            }
        }

        return result;
    }

    public void UpdatePlayerRespawnPos(int count)
    {
        if (count > playerRespawnPosRealTime.Count || count <= 0)
        {
            return;
        }

        for (int i = 1; i < count + 1; i++)
        {
            if (!playerRespawnPosRealTime[playerRespawnsPos[i]])
                playerRespawnPosRealTime[playerRespawnsPos[i]] = true;
        }
    }
}
