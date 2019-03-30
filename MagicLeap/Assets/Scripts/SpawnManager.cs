using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] prefabs;

    private SpawnMessage _spawnMsg;
    private bool _spawnPrefab;

    #region Unity Methods

    // Use this for initialization
    void Start()
    {
        _spawnPrefab = false;
    }

    void Update()
    {
        if (_spawnPrefab)
        {
            GameObject newInstantiate = Instantiate(prefabs[_spawnMsg.prefabId]);
            newInstantiate.transform.SetParent(GameObject.Find("World Origin").transform);
            newInstantiate.transform.localPosition = _spawnMsg.position;
            newInstantiate.transform.localRotation = Quaternion.Euler(_spawnMsg.rotation);

            _spawnPrefab = false;
        }
    }

    #endregion

    #region Public Methods

    public void Spawn(SpawnMessage msg)
    {
        Debug.Log(msg.payload);
        _spawnMsg = msg;
        _spawnPrefab = true;
    }

    #endregion
}
