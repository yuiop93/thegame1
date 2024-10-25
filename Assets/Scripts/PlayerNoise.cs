using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoise : MonoBehaviour
{
    public float runNoiseLevel = 10f; // 跑步时的噪音值
    public float walkNoiseLevel = 5f; // 走路时的噪音值
    public Transform playerTransform; // 玩家位置，用于计算噪音源位置

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            GenerateNoise(0f); // 按下R键时，噪音值为0
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)) // 跑步
            {
                GenerateNoise(runNoiseLevel);
            }
            else if (Input.GetKey(KeyCode.W)) // 走路
            {
                GenerateNoise(walkNoiseLevel);
            }
        }
    }

    void GenerateNoise(float noiseLevel)
    {
        NoiseEvent(noiseLevel, playerTransform.position);
    }

    public void NoiseEvent(float noiseLevel, Vector3 noiseSource)
    {
        Collider[] hitColliders = Physics.OverlapSphere(noiseSource, 20f); // 假设20米听力范围
        foreach (var hitCollider in hitColliders)
        {
            var hearing = hitCollider.GetComponent<Hearing>();
            if (hearing != null)
            {
                hearing.DetectNoise(noiseLevel, noiseSource); // 将噪音级别传递给 Hearing
            }
        }
    }
}
