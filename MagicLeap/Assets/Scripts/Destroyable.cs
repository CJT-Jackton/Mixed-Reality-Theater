using UnityEngine;

public class Destroyable : MonoBehaviour
{
    #region Public Variables

    public int MaxHitPoint = 1;
    #endregion

    #region Private Variables

    private int _hitCount;

    #endregion

    #region Public Methods

    public void Hit()
    {
        _hitCount += 1;

        if (_hitCount >= MaxHitPoint)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Unity Methods

    // Use this for initialization
    void Start()
    {
        _hitCount = 0;
    }

    #endregion

}
