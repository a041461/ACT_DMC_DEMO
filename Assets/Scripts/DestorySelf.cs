using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestorySelf : MonoBehaviour
{
    public float time = 2f;
    public bool destory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            if (destory)
                Destroy(this.gameObject);
            else
                this.gameObject.SetActive(false);
        }
           
    }
}
