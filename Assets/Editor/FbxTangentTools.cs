using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FbxTangentTools
{
    [MenuItem("Tools/计算切线并导入模型")]
    public static void CalculateTangentAndImport()
    {
        var selectedGameObject = Selection.activeGameObject;
        if (selectedGameObject == null)
        {
            Debug.LogError("请先选择一个游戏对象");
            return;
        }

        var meshes = new List<Mesh>();

        // 处理MeshFilter
        MeshFilter[] meshFilters = selectedGameObject.GetComponentsInChildren<MeshFilter>();
        foreach (var meshFilter in meshFilters)
        {
            Mesh mesh = meshFilter.sharedMesh;
            if (mesh != null)
            {
                meshes.Add(mesh);
            }
        }

        // 处理SkinnedMeshRenderer
        SkinnedMeshRenderer[] skinnedMeshRenderers = selectedGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
        {
            Mesh mesh = skinnedMeshRenderer.sharedMesh;
            if (mesh != null)
            {
                meshes.Add(mesh);
            }
        }

        // 计算并应用切线
        foreach (var mesh in meshes)
        {
            CalculateAndApplyTangent(mesh);
        }
    }

    private static void CalculateAndApplyTangent(Mesh mesh)
    {
        var averageNormalHash = new Dictionary<Vector3, Vector3>();

        for (var i = 0; i < mesh.vertexCount; i++)
        {
            Vector3 vertex = mesh.vertices[i];
            Vector3 normal = mesh.normals[i];

            if (!averageNormalHash.ContainsKey(vertex))
            {
                averageNormalHash.Add(vertex, normal);
            }
            else
            {
                averageNormalHash[vertex] = (averageNormalHash[vertex] + normal).normalized;
            }
        }

        var averageNormals = new Vector3[mesh.vertexCount];
        for (var i = 0; i < mesh.vertexCount; i++)
        {
            averageNormals[i] = averageNormalHash[mesh.vertices[i]];
        }

        var tangents = new Vector4[mesh.vertexCount];
        for (var i = 0; i < mesh.vertexCount; i++)
        {
            tangents[i] = new Vector4(averageNormals[i].x, averageNormals[i].y, averageNormals[i].z, 0);
        }

        mesh.tangents = tangents;
    }
}
