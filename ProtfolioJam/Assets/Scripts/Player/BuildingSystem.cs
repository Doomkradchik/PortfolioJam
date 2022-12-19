using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraChild;

    [SerializeField]
    private Vector3Int _cellSize = Vector3Int.one;

    private const string WALL_TAG = "Wall";
    private const string STAIRS_TAG = "Stairs";
    private const string FLOOR_TAG = "Floor";

    private Dictionary<string, Func<Vector3, Transform, Vector3>> _cellPositionProvider;
    private void Awake()
    {
        _cellPositionProvider = new Dictionary<string, Func<Vector3, Transform, Vector3>>
        {
            {WALL_TAG, GetWallPosition },
            {STAIRS_TAG, GetStairsPosition },
            {FLOOR_TAG, GetFloorPosition },
        };
    }

    private void Update()
    {
        var ray = new Ray(_cameraChild.position, _cameraChild.forward);

        var config = CellFactory.Instance.Peek(Input.GetKeyDown(KeyCode.Q));

        if (Physics.Raycast(ray, out RaycastHit hit, 7f))
        {
            Vector3 cellPosition;
            var hasProvider = _cellPositionProvider.TryGetValue(hit.collider.tag, out var positionProvider);

            var y_rotation = Mathf.RoundToInt(transform.eulerAngles.y) != 0f ?
                Mathf.RoundToInt(transform.eulerAngles.y / 90f) * 90f : 0;

            config.FantomCell.transform.eulerAngles = Vector3.up * y_rotation;

            if (hasProvider && config.FantomCell.CompareTag(hit.collider.tag))
                cellPosition = positionProvider.Invoke(hit.point, config.FantomCell.transform);
            else
                cellPosition = CalculateCellPosition(hit.point);

            config.FantomCell.transform.position = cellPosition;

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(config._cell, config.FantomCell.transform.position, config.FantomCell.transform.rotation);
            }
        }
    }

    private Vector3 GetFloorPosition(Vector3 point, Transform fantom)
    {
        return CalculateCellPosition(point) + fantom.forward * _cellSize.z - fantom.up * _cellSize.y;
    }

    private Vector3 GetStairsPosition(Vector3 point, Transform fantom)
    {
        return CalculateCellPosition(point) + fantom.up * _cellSize.y + fantom.forward * _cellSize.z;
    }

    private Vector3 GetWallPosition(Vector3 point, Transform fantom)
    {
        return CalculateCellPosition(point) + fantom.up * _cellSize.y;
    }

    private Vector3 CalculateCellPosition(Vector3 position)
    {
        return new Vector3(Mathf.RoundToInt(position.x) != 0f ? Mathf.RoundToInt(position.x / _cellSize.x) * _cellSize.x : _cellSize.x,
            Mathf.RoundToInt(position.y) != 0f ? Mathf.RoundToInt(position.y / _cellSize.y) * _cellSize.y : 0,
            Mathf.RoundToInt(position.z) != 0f ? Mathf.RoundToInt(position.z / _cellSize.z) * _cellSize.z : _cellSize.z);
    }
}
