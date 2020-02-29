using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public ParticleSystem ps;

    private void Update() {
        if (Input.GetKeyDown("space"))
        {
            ps.Play();
        }
    }
}
