using Unity.VisualScripting;
using UnityEngine;

public class TextureDrawer : MonoBehaviour
{
    public Renderer textureRenderer;

    public bool isDrawHeight = false;

    public MapGenerator mapGenerator;

    public Material urpLitMaterial;


    public void DrawTexture(Texture2D texture)
    {
        if (urpLitMaterial == null)
        {
            urpLitMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        }
        else
        {

            urpLitMaterial = new Material(urpLitMaterial);
        }
        urpLitMaterial.mainTexture = texture;

        textureRenderer.sharedMaterial = urpLitMaterial;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }
    public void TexturizeMap(float[,] noiseMapColor, GameObject TerrainInQuestion)
    {
        mapGenerator = FindAnyObjectByType<MapGenerator>();
        Color[] colourMap = new Color[VaribleGlobal.worldSize * VaribleGlobal.worldSize];
        float[,] noiseBlackWhite = new float[VaribleGlobal.worldSize, VaribleGlobal.worldSize];
        for (int y = 0; y < VaribleGlobal.worldSize; y++)
        {
            for (int x = 0; x < VaribleGlobal.worldSize; x++)
            {
                float currentHeight = noiseMapColor[x, y];
                noiseBlackWhite[x, y] = noiseMapColor[x, y];
                for (int i = 0; i < mapGenerator.regions.Length; i++)
                {
                    if (currentHeight <= mapGenerator.regions[i].height)
                    {
                        colourMap[y * VaribleGlobal.worldSize + x] = mapGenerator.regions[i].colour; //add some rand noise to mix the colors of the regions
                        break;
                    }
                }
            }
        }
        TextureDrawer display = TerrainInQuestion.GetComponent<TextureDrawer>();

        if (isDrawHeight)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseBlackWhite));
        }
        else
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, VaribleGlobal.worldSize, VaribleGlobal.worldSize));
        }
    }

}
