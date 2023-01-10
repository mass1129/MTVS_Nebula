using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BAnimRotation : BuildingAnimation
{
    public Transform toRotateObj = null;

    public Vector3 toRotateV3;
    float time;
    // Update is called once per frame
    void Update()
    {
        if (!createDone) return;
        time += Time.deltaTime;
            toRotateObj.localEulerAngles = toRotateV3 * time;
    }
}
