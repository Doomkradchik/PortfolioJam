using System.Linq;
using UnityEngine;

public class Grid3D : MonoBehaviour
{
    /// <summary>
    /// Params: CellSize
    /// 
    /// methods : GetCenterOfCellByPosition(Vector3 pos);
    /// 
    /// </summary>
    /// 
    [SerializeField]
    private GameObject _effector;

    [SerializeField]
    private Vector3 _cellSize = Vector3.one;

    private Vector3[] offsets;

    private void Awake()
    {
        offsets = new Vector3[]
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(-1f, 0f, 0f),
            new Vector3(0f, -1f, 0f),
            new Vector3(0f, 0f, -1f),
        };
    }

    public void TrySpawnFantomCellsIn(Vector3 cellCenter)
    {
        foreach(var offset in offsets)
        {
            var newCenter = cellCenter + Vector3.Scale(offset, _cellSize);
            if(CanSpawnEffectorCell(newCenter))
            {
                Instantiate(_effector, newCenter, Quaternion.identity);
            }
        }
    }

    private bool CanSpawnEffectorCell(Vector3 pos)
    {
        var colliders = Physics.OverlapBox(pos, _cellSize * 0.4f, Quaternion.identity);
        return colliders.Length == 0;
            //.Where(collider => collider.CompareTag(_effector.tag))
            //.ToArray()
            //.Length == 0;
    }

    public Vector3 GetCenterOfCellByPosition(Vector3 point)
    {
        return Vector3.Scale(point.DivideRoundingUpBy(_cellSize), _cellSize) - _cellSize;
    }

    public void SetCellRotation(GameObject cell, Vector3 direction)
    {
        var xz_dir = new Vector3(direction.x, 0f, direction.z);
        var y_rotation = Quaternion.LookRotation(xz_dir, Vector3.up).eulerAngles.y;
        var nearestAngle = (int)((y_rotation + 45) / 90) * 90;
        cell.transform.rotation = Quaternion.Euler(0f, nearestAngle, 0f);
    }
}

public static class Vector3Extesion
{
    public static Vector3 DivideRoundingUpBy(this Vector3 vec1, Vector3 vec2)
    {
        var x = Mathf.Round(vec1.x / vec2.x + 1);
        var y = Mathf.Round(vec1.y / vec2.y + 1);
        var z = Mathf.Round(vec1.z / vec2.z + 1);

        return new Vector3(x, y, z);
    }
}