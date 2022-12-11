using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class end : MonoBehaviour
{
    public void End()
    {
        while (GetComponent<Image>().color.a < 255)
        {
            var c = GetComponent<Image>().color.a;
            c  += 0.1f;
        }
    }

    void Update()
    {
        End();
    }
}
