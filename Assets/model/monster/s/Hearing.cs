using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour
{
    public float hearingRange = 10f;  // 听觉范围
    private bool isHearingPlayer = false; // 是否听到玩家的标志
    public bool IsHearingPlayer { get { return isHearingPlayer; } }

    // 检测噪音并记录噪音来源位置
    public void DetectNoise(float noiseLevel, Vector3 noiseSource)
    {
        // 每次偵測噪音前重置狀態
        isHearingPlayer = false;

        float distanceToNoise = Vector3.Distance(transform.position, noiseSource);
        // 计算可听范围，根据噪音级别调整
        float effectiveHearingRange = hearingRange;
        // 根据噪音级别调整听觉范围
        if (noiseLevel == 10f) // 跑步噪音
        {
            effectiveHearingRange += 10f; // 增加听觉范围
        }
        else if (noiseLevel == 5f) // 走路噪音
        {
            effectiveHearingRange -= 2f; // 减少听觉范围
        }
        else if (noiseLevel == 0f) // 噪音值为0时
        {
            effectiveHearingRange = 0f; // 确保听觉范围为0
        }

        // 如果噪音距离在调整后的听觉范围内
        if (distanceToNoise <= effectiveHearingRange)
        {
            isHearingPlayer = true; // 标记为听到玩家
            Debug.Log("怪物听到了噪音！");
        }
    }

    // 返回是否听到了噪音
    public bool HasHeardNoise()
    {
        return isHearingPlayer;
    }
}
