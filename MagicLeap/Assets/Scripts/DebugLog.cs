using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DebugLog : MonoBehaviour
{
    public bool debug;
    public Text debugText;

    private string debugMsg;

    void Start()
    {
        debugMsg = "";
    }

    void Update()
    {
        if (debug)
        {
            debugText.text = debugMsg;
        }
    }

    public void Log(string msg)
    {
        if (debug)
        {
            debugMsg = msg;
        }
    }
}
