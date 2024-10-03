using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PlugTangentTools
{
    [MenuItem("Tools/模型平均法线写入切线数据")]
    public static void WriteAverageNormalToTangentTools()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        foreach (GameObject obj in selectedObjects)
        {
            ProcessMeshFilters(obj);
            ProcessSkinnedMeshRenderers(obj);
        }
    }

    private static void ProcessMeshFilters(GameObject obj)
    {
        MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();
        foreach (var meshFilter in meshFilters)
        {
            Mesh mesh = meshFilter.sharedMesh;
            if (mesh != null)
            {
                WriteAverageNormalToTangent(mesh);
                EditorUtility.SetDirty(mesh);
            }
        }
    }

    private static void ProcessSkinnedMeshRenderers(GameObject obj)
    {
        SkinnedMeshRenderer[] skinnedMeshRenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
        {
            Mesh mesh = skinnedMeshRenderer.sharedMesh;
            if (mesh != null)
            {
                WriteAverageNormalToTangent(mesh);
                EditorUtility.SetDirty(mesh);
            }
        }
    }

    private static void WriteAverageNormalToTangent(Mesh mesh)
    {
        var averageNormalHash = new Dictionary<Vector3, Vector3>();
        for (var j = 0; j < mesh.vertexCount; j++)
        {
            if (!averageNormalHash.ContainsKey(mesh.vertices[j]))
            {
                averageNormalHash.Add(mesh.vertices[j], mesh.normals[j]);
            }
            else
            {
                averageNormalHash[mesh.vertices[j]] =
                    (averageNormalHash[mesh.vertices[j]] + mesh.normals[j]).normalized;
            }
        }

        var averageNormals = new Vector3[mesh.vertexCount];
        for (var j = 0; j < mesh.vertexCount; j++)
        {
            averageNormals[j] = averageNormalHash[mesh.vertices[j]];
        }

        var tangents = new Vector4[mesh.vertexCount];
        for (var j = 0; j < mesh.vertexCount; j++)
        {
            tangents[j] = new Vector4(averageNormals[j].x, averageNormals[j].y, averageNormals[j].z, 0);
        }
        mesh.tangents = tangents;
    }
}
