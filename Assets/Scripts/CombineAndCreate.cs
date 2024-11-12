using UnityEngine;
using System.Collections;

public class CombineAndCreate : MonoBehaviour
{
    public TextureDrawer txtDrawer;
    public myNoiseMap myNoiseMap;
    public MyGridPos myGridPos;
    public ColliderCreator colliderCreator;
    public Mesh plane;
    public bool isDefaultPlane;
    public bool isAsyncGen;
    public MeshDrawer meshDrawer;
    public MeshDrawerAndColliderAsync meshDrawerAsync;

    [ContextMenu("Generate Me")]
    public void Gen()
    {
        StartCoroutine(GenAsync());
    }

    private IEnumerator GenAsync()
    {

        myGridPos.AttPos();


        myNoiseMap.reGenNoise(myGridPos.grid_pos);
        yield return null; // Wait for next frame

        if (isDefaultPlane)
        {
            // Step 3: Set up the default plane mesh if required
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.sharedMesh = plane;
            yield return null; // Wait for next frame

            // Step 4: Apply texture to map
            txtDrawer.TexturizeMap(myNoiseMap.noiseMap, gameObject);
            gameObject.transform.localScale = new Vector3(6.4f, 6.4f, 6.4f);
            yield return null; // Wait for next frame

            // Step 5: Create collider mesh
            colliderCreator.CreateColliderMesh();
            yield return null; // Wait for next frame
        }
        else
        {
            // Step 6: Apply texture and draw mesh
            txtDrawer.TexturizeMap(myNoiseMap.noiseMap, gameObject);
            yield return null; // Wait for next frame
            if (isAsyncGen)
            {
                meshDrawerAsync.DrawMeshAndAttachMeshFilterAsync(myNoiseMap.noiseMap);
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                yield return null;


            }
            else
            {
                meshDrawer.DrawMeshAndAttachMeshFilter(myNoiseMap.noiseMap);
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                colliderCreator.CreateColliderMesh();
                yield return null;


            }

        }
    }
}
