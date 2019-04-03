using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    public ParticleSystem ps;

    private float m_timer = 0.0f;
    private float m_interval = 0.2f;

    public int Branch_Number;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("space key was pressed");
            ps.TriggerSubEmitter(0);
            
        }
    }
}
