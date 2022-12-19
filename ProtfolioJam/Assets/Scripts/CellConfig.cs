using UnityEngine;

[CreateAssetMenu]
public class CellConfig : ScriptableObject
{
    public GameObject _cell;

    [SerializeField]
    private GameObject _fantomCell;

    public GameObject FantomCell { get; private set; }
    public bool _inited = false;

    public void CreateFantomCell(Transform root)
    {
        FantomCell = Instantiate(_fantomCell, root);
        FantomCell.SetActive(false);
        _inited = true;
    }
}
