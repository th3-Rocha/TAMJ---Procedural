using UnityEngine;
using System.Collections;

public class genCave : MonoBehaviour
{
    public CelulaGen celulaGen;
    public MeshDrawerAndColliderAsync meshDrawerAndColliderAsync;
    public ColliderCreator colliderCreator;
    public Material urpLitMaterial;
    public Renderer renderers;
    public float[,] heeightmap;

    [ContextMenu("Generate Cave")]
    public void genCaveWithCollider()
    {
        StartCoroutine(GenerateCaveWithColliderCoroutine());
    }

    private IEnumerator GenerateCaveWithColliderCoroutine()
    {
        int seed = (int)(transform.position.x + transform.position.z);
        
        yield return celulaGen.StartCaveGeneration(seed);
        
        heeightmap = celulaGen.heightMap;
        meshDrawerAndColliderAsync.DrawMeshAndAttachMeshFilterAsync(heeightmap);
        
        urpLitMaterial = new Material(urpLitMaterial);
        renderers.sharedMaterial = urpLitMaterial;
    }
}
