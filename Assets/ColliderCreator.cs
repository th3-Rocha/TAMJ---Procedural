using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ColliderCreator : MonoBehaviour
{
    public void CreateColliderMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = meshFilter.sharedMesh;
        gameObject.isStatic = true;
        meshCollider.convex = false;

    }
}
