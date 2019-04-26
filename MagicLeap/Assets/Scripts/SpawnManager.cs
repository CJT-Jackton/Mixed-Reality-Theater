using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SpawnManager : MonoBehaviour
{
    public GameObject[] prefabs;

    private SpawnMessage _spawnMsg;
    private bool _spawnPrefab;

    private bool _destroyAll;

    private List<MRTGameObject> _staticGameObjects;

    #region Unity Methods

    // Use this for initialization
    void Start()
    {
        _spawnPrefab = false;
        _destroyAll = false;
        _staticGameObjects = new List<MRTGameObject>();
    }

    void Update()
    {
        if (_spawnPrefab)
        {
            GameObject newInstantiate = Instantiate(prefabs[_spawnMsg.prefabId]);

            _staticGameObjects.Add(new MRTGameObject(newInstantiate, _spawnMsg.prefabId, _spawnMsg.prefabId));

            newInstantiate.transform.SetParent(GameObject.Find("World Origin").transform);
            newInstantiate.transform.localPosition = _spawnMsg.position;
            newInstantiate.transform.localRotation = Quaternion.Euler(_spawnMsg.rotation);

            _spawnPrefab = false;
        }

        if (_destroyAll)
        {
            Debug.Log("Destroy everything!");
            foreach (var obj in _staticGameObjects)
            {
                if (obj.gameObject)
                {
                    Destroy(obj.gameObject);
                }
            }

            _staticGameObjects.Clear();
            _destroyAll = false;
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

    public void DestroyAll()
    {
        _destroyAll = true;
    }

    #endregion
}
