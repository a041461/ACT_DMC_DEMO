using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaRain_area : MonoBehaviour
{
    //public Transform rainArea;
    public Collider rainArea;
    public GameObject katana;
    private void Start()
    {
        
        for (int i = 0; i < 22; i++)
        {
           GameObject go= Instantiate(katana, rainArea.transform);
            go.transform.position = new Vector3(Random.Range(rainArea.bounds.min.x, rainArea.bounds.max.x)
                , Random.Range(rainArea.bounds.min.y, rainArea.bounds.max.y)
            , Random.Range(rainArea.bounds.min.z, rainArea.bounds.max.z));
            
            
        }
    }
}
