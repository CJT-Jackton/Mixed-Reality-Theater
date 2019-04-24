using System.Collections.Generic;
using UnityEngine;

public class DirectorManager : MonoBehaviour
{
    #region Private Variables

    private struct MRTObject
    {
        public GameObject gameObject;
        public int prefabId;
        public bool _hasMove;
    }

    private List<MRTObject> _prefabs;

    #endregion


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Public Methods

    public void AddObject(GameObject go, int id)
    {
        MRTObject newObject = new MRTObject();
        newObject.gameObject = go;
        newObject.prefabId = id;

        _prefabs.Add(newObject);
    }

    #endregion
}
