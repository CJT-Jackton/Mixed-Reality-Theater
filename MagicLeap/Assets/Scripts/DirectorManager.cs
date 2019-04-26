using System.Collections.Generic;
using UnityEngine;

public class DirectorManager : MonoBehaviour
{
    #region Private Variables

    private ClientManager _clientManager;

    private List<MRTGameObject> _gameObjects = new List<MRTGameObject>();
    private List<bool> _uploaded = new List<bool>();

    #endregion


    // Use this for initialization
    void Start()
    {
        _clientManager = GameObject.Find("Network Manager").GetComponent<ClientManager>();

        InvokeRepeating("Upload", 5.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Public Methods

    public void AddObject(GameObject go, int prefabId)
    {
        MRTGameObject newObject = new MRTGameObject(go, _gameObjects.Count, prefabId);

        _gameObjects.Add(newObject);
        _uploaded.Add(false);
    }

    public void Upload()
    {
        for (int i = 0; i < _gameObjects.Count; ++i)
        {
            if (!_uploaded[i])
            {
                DirectorSpawnMessage msg = new DirectorSpawnMessage();
                msg.connectId = 0;
                msg.instanceId = _gameObjects[i].id;
                msg.prefabId = _gameObjects[i].prefabId;

                msg.position = _gameObjects[i].gameObject.transform.position;
                msg.rotation = _gameObjects[i].gameObject.transform.rotation.eulerAngles;

                msg.payload = "Upload anchor.";

                _clientManager.UploadAnchor(msg);
                _uploaded[i] = true;
            }
        }
    }

    #endregion
}
