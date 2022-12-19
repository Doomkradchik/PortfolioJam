using UnityEngine;

public class GameEntity : MonoBehaviour
{
    [SerializeField] private GameObject _snowPrefab;
    [SerializeField] private Transform _snowItemRoot;

    ///<summary> 
    /// Game Entity is game system which opened for extesion by Player or Enemy AI;
    /// 1) Throw ball by ballistic trajectory;
    /// 2) Grow throwable object by holding mouse and then throw
    /// 
    /// </summary>
    ///

    protected GameObject _holdingBall = null;

    
    protected void CreateSnowBall(Vector3 position)
    {
        _snowItemRoot.position = position;
        _holdingBall = Instantiate(_snowPrefab, _snowItemRoot);
        

        //_holdingBall = Instantiate(_snowPrefab, position, Quaternion.identity);
        
    }
}
