using UnityEngine;
using CLL;

public class CellFactory : MonoBehaviour
{
    [SerializeField]
    private CellConfig _wall;

    [SerializeField]
    private CellConfig _floor;

    [SerializeField]
    private CellConfig _stairs;

    [SerializeField]
    private Transform _root;

    public static CellFactory Instance;
    private CircularLinkedList<CellConfig> _configs;
    private Node<CellConfig> _current;  


    private void Awake()
    {
        if (Instance != null)
            throw new System.InvalidOperationException("CellFactory has to be singleton");

        Instance = this;
        _configs = new CircularLinkedList<CellConfig>(new[] { _wall, _floor, _stairs });
        _current = _configs.head;
    }


    private void Start()
    {
        _wall.CreateFantomCell(_root);
        _floor.CreateFantomCell(_root);
        _stairs.CreateFantomCell(_root);
    }


    public CellConfig Peek(bool next = false)
    {
        if (next == false)
            return _current.Data;

        _current.Data.FantomCell.SetActive(false);
        _current = _current.Next;
        _current.Data.FantomCell.SetActive(true);
        return _current.Data;
    }
}
