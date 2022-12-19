using UnityEngine;

public class GridTest : MonoBehaviour
{
    [SerializeField]
    private GameObject _testCellPrefab;

    [SerializeField]
    private GameObject _testIntersectionPrefab;

    [SerializeField]
    private GameObject _testOriginCellPrefab;

    private Grid3D _grid3d;

    private GameObject _intersection;
    private GameObject _standingCell = null;


    private GameObject Intersection
    {
        get
        {
            if(_intersection == null)
                _intersection = Instantiate(_testIntersectionPrefab);

            return _intersection;
        }
    }

    private GameObject _originCell;
    private GameObject OriginCell
    {
        get
        {
            if (_originCell == null)
                _originCell = Instantiate(_testOriginCellPrefab);

            return _originCell;
        }
    }

    private void Start()
    {
        _grid3d = FindObjectOfType<Grid3D>();

        if (_grid3d == null)
            throw new System.Exception("Cannot find Grid3D");
    }


    private void Update()
    {
        var ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            var center = _grid3d.GetCenterOfCellByPosition(hit.point);

            Intersection.transform.position = hit.point;
            OriginCell.transform.position = center;


            if (Input.GetMouseButtonDown(0))
            {
                //if (hit.collider.CompareTag("Fantom"))
                //{
                //    ReplaceEffectorCell(hit.collider.gameObject, ray.direction);
                //    return;
                //}

                if(_standingCell != null)
                {
                    CreateCell(_standingCell.transform.position, ray.direction);
                    return;
                }    

                CreateCell(center, ray.direction);
            }
                
        }

        Debug.DrawRay(ray.origin, ray.direction * 10f);
    }

    private void CreateCell(Vector3 position, Vector3 direction)
    {
        var cell = Instantiate(_testCellPrefab, position, Quaternion.identity);
        _grid3d.SetCellRotation(cell, direction);
        _grid3d.TrySpawnFantomCellsIn(position);
    }

    private void ReplaceEffectorCell(GameObject effector, Vector3 direction)
    {
        var position = effector.transform.position;
        Destroy(effector);
        CreateCell(position, direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fantom"))
            _standingCell = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Fantom"))
            _standingCell = null;
    }
}
