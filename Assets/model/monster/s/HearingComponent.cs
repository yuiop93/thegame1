using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingComponent : MonoBehaviour
{
    public float hearingRange = 10f;
    private bool isHearingPlayer = false;
    private Vector3 lastHeardPosition;

    public bool IsHearingPlayer { get { return isHearingPlayer; } }

    public void DetectNoise(float noiseLevel, Vector3 noiseSource)
    {
        float distanceToNoise = Vector3.Distance(transform.position, noiseSource);
        
        // 如果噪音距离在听觉范围内
        if (distanceToNoise <= hearingRange)
        {
            isHearingPlayer = true;
            lastHeardPosition = noiseSource;
            Debug.Log("怪物听到了噪音！");
        }
    }

    public bool HasHeardNoise()
    {
        return isHearingPlayer;
    }

    public void ResetHearing()
    {
        isHearingPlayer = false;
    }
}
