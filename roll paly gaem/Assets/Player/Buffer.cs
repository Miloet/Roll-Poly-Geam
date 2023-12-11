using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : MonoBehaviour
{
    private float time;
    private float resetTime = 0.15f;
    private bool once;
    public static Buffer SetBuffer(GameObject self, float Time, bool ResetOponPress = false)
    {
        Buffer b = self.AddComponent<Buffer>();
        b.resetTime = Time;
        b.once = ResetOponPress;
        return b;
    }

    private void Update()
    {
        if (time >= 0) time -= Time.deltaTime;
    }
    public void Pressed()
    {
        time = resetTime;
    }
    public void Unpress()
    {
        time = 0;
    }
    public bool GetPress()
    {
        bool b = time > 0;
        if (once && b) time = 0;
        return b;
    }


}
