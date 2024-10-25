using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    public GameObject laserPrefab;  // 激光预制体
    public Transform firePoint;     // 射击的起点
    public float laserSpeed = 10f;  // 激光的速度
    private Transform player;  // 玩家 Transform

    private void Start()
    {
        // 找到玩家（假设玩家有 "Player" 标签）
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // 在动画事件中调用这个方法发射激光
    public void FireLaser()
    {
        if (laserPrefab != null && firePoint != null && player != null)
        {
            // 实例化激光预制体
            GameObject laser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
            
            // 计算朝向玩家的方向
            Vector3 directionToPlayer = (player.position - firePoint.position).normalized;

            // 让激光朝向玩家
            laser.transform.forward = directionToPlayer;
            laser.transform.Rotate(73, 0, 0);
            Rigidbody rb = laser.GetComponent<Rigidbody>();

            // 如果激光有刚体组件，给它一个速度
            if (rb != null)
            {
                rb.velocity = directionToPlayer * laserSpeed;
            }
        }
        else
        {
            Debug.LogWarning("LaserPrefab, FirePoint, or Player not assigned.");
        }
    }
}
