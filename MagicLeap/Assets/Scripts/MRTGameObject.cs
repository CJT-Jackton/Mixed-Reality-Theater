using UnityEngine;

public class MRTGameObject
{
    public readonly GameObject gameObject;
    public readonly int id;
    public readonly int prefabId;

    public MRTGameObject(GameObject gameObject, int id, int prefabId)
    {
        this.gameObject = gameObject;
        this.id = id;
        this.prefabId = prefabId;
    }

}
