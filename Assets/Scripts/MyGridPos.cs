using UnityEngine;

public class MyGridPos : MonoBehaviour
{

    public Vector2Int grid_pos;
    int scaling = 10;
    public void AttPos()
    {
        scaling = (VaribleGlobal.worldSize * 1) / 64;
        Vector3 worldPos = transform.position;
        grid_pos.x = Mathf.FloorToInt(worldPos.x / (VaribleGlobal.worldSize * scaling));
        grid_pos.y = Mathf.FloorToInt(worldPos.z / (VaribleGlobal.worldSize * scaling));

    }
}
