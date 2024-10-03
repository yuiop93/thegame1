using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlendShapeManager : MonoBehaviour
{
    // 设置要控制的标签
    public string tagToControl = "wink"; // 这里输入你为要控制的物件设置的标签
    public float blinkDuration = 0.2f; // 从 0 到 100 的时间（闭眼）
    public float blinkInterval = 2.0f; // 眨眼之间的间隔时间（包括闭眼和睁眼）

    private void Start()
    {
        // 查找场景中所有带有指定标签的 SkinnedMeshRenderer 物件
        SkinnedMeshRenderer[] skinnedMeshRenderersToControl = FindObjectsOfType<SkinnedMeshRenderer>();
        List<SkinnedMeshRenderer> filteredRenderers = new List<SkinnedMeshRenderer>();

        foreach (var renderer in skinnedMeshRenderersToControl)
        {
            // 检查物件的标签是否匹配
            if (renderer.CompareTag(tagToControl))
            {
                filteredRenderers.Add(renderer);
            }
        }

        StartCoroutine(ControlBlink(filteredRenderers.ToArray()));
    }

    private IEnumerator ControlBlink(SkinnedMeshRenderer[] skinnedMeshRenderers)
    {
        while (true) // 无限循环
        {
            // 同时闭眼
            yield return StartCoroutine(Blink(skinnedMeshRenderers, 100f)); // 闭眼
            // 立即睁眼
            yield return StartCoroutine(Blink(skinnedMeshRenderers, 0f)); // 睁眼
            yield return new WaitForSeconds(blinkInterval); // 等待一段时间再眨眼
        }
    }

    private IEnumerator Blink(SkinnedMeshRenderer[] skinnedMeshRenderers, float targetWeight)
    {
        float elapsedTime = 0f;

        // 获取当前的权重
        float startWeight = targetWeight == 100f ? 0f : 100f;

        // 逐渐到达目标权重
        while (elapsedTime < blinkDuration)
        {
            float weight = Mathf.Lerp(startWeight, targetWeight, elapsedTime / blinkDuration);
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                for (int i = 0; i < skinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(i, weight);
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一帧
        }

        // 确保最终权重被设置
        foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
        {
            for (int i = 0; i < skinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(i, targetWeight);
            }
        }
    }
}
