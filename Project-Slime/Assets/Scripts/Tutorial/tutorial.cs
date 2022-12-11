using SA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial : MonoBehaviour
{
    public int sl = 0;

    void Start()
    {
        FindObjectOfType<UI_controll>().Pause();
        FindObjectOfType<UI_controll>().enabled = false;
        FindObjectOfType<StateManager>().inAction = true;
    }
    public void Next_sl()
    {
        transform.GetChild(sl).gameObject.SetActive(false);
        sl += 1;
        if (sl > 4)
        {
            Destroy(gameObject);
            FindObjectOfType<UI_controll>().UnPause();
            FindObjectOfType<UI_controll>().enabled = true;
        }
        else
        {
            transform.GetChild(sl).gameObject.SetActive(true);
        }
    }
}
