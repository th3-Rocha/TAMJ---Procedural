using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshDrawerAndColliderAsync : MonoBehaviour
{
    public ColliderCreator colliderCreator;
    public float amplitude = 50;
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector2[] uvs;
    private Vector3[] normals;
    private int[] triangles;

    private int xSize = 64;
    private int zSize = 64;
    private int scaling = 10;

    public void DrawMeshAndAttachMeshFilterAsync(float[,] heightMap)
    {
        StartCoroutine(GenerateTerrainAsync(heightMap));
    }

    private IEnumerator GenerateTerrainAsync(float[,] heightMap)
    {
        xSize = VaribleGlobal.worldSize;
        zSize = VaribleGlobal.worldSize;
        scaling = (VaribleGlobal.worldSize * 1) / 64;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        yield return StartCoroutine(CreateShapeAsync(heightMap));
        yield return StartCoroutine(CreateTrianglesAsync());
        UpdateMesh();
    }

    private IEnumerator CreateShapeAsync(float[,] heightMap)
    {
        vertices = new Vector3[xSize * zSize * 6];
        uvs = new Vector2[xSize * zSize * 6];
        normals = new Vector3[xSize * zSize * 6];

        float centerX = xSize * 0.5f;
        float centerZ = zSize * 0.5f;
        int vertIndex = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                float yA = heightMap[x, z] * amplitude;
                float yB = heightMap[x + 1, z] * amplitude;
                float yC = heightMap[x, z + 1] * amplitude;
                float yD = heightMap[x + 1, z + 1] * amplitude;

                Vector3 a = new Vector3(-(x - centerX) * scaling, yA * scaling, -(z - centerZ) * scaling);
                Vector3 b = new Vector3(-(x + 1 - centerX) * scaling, yB * scaling, -(z - centerZ) * scaling);
                Vector3 c = new Vector3(-(x - centerX) * scaling, yC * scaling, -(z + 1 - centerZ) * scaling);
                Vector3 d = new Vector3(-(x + 1 - centerX) * scaling, yD * scaling, -(z + 1 - centerZ) * scaling);

                Vector3 normal = CalculateNormal(a, c, b);

                // First triangle
                vertices[vertIndex] = a;
                vertices[vertIndex + 1] = c;
                vertices[vertIndex + 2] = b;

                normals[vertIndex] = normal;
                normals[vertIndex + 1] = normal;
                normals[vertIndex + 2] = normal;

                uvs[vertIndex] = new Vector2((float)x / xSize, (float)z / zSize);
                uvs[vertIndex + 1] = new Vector2((float)x / xSize, (float)(z + 1) / zSize);
                uvs[vertIndex + 2] = new Vector2((float)(x + 1) / xSize, (float)z / zSize);

                vertIndex += 3;

                // Second triangle
                vertices[vertIndex] = b;
                vertices[vertIndex + 1] = c;
                vertices[vertIndex + 2] = d;

                normals[vertIndex] = normal;
                normals[vertIndex + 1] = normal;
                normals[vertIndex + 2] = normal;

                uvs[vertIndex] = new Vector2((float)(x + 1) / xSize, (float)z / zSize);
                uvs[vertIndex + 1] = new Vector2((float)x / xSize, (float)(z + 1) / zSize);
                uvs[vertIndex + 2] = new Vector2((float)(x + 1) / xSize, (float)(z + 1) / zSize);

                vertIndex += 3;

                // Yield every few rows to spread load across frames
                if (x % 32 == 0)
                {
                    yield return null;
                }
            }
        }
    }

    private IEnumerator CreateTrianglesAsync()
    {
        triangles = new int[vertices.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = i;
            if (i % 10000 == 0)
            {
                yield return null;
            }
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.RecalculateBounds();

        mesh.UploadMeshData(false);
        colliderCreator.CreateColliderMesh();
    }

    private Vector3 CalculateNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 side1 = b - a;
        Vector3 side2 = c - a;
        return Vector3.Cross(side1, side2).normalized;
    }
}
