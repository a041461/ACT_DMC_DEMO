using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public GameObject[] Capsules;
    public GameObject Demon;
    public float time;
    private Transform[] mytransforms;
    private void Start()
    {
        Instance = this;
    }
    private void Update()
    {
        DetecedCapsules();
    }

    public void DetecedCapsules()
    {
        if (transform.childCount == 1)
        {
            Demon.SetActive(true);
        }
    }


    public void ActiveCapsules()
    {
        StartCoroutine(IActiveCapsules());
    }
    public IEnumerator IActiveCapsules()
    {
        for(int i = 0; i < Capsules.Length; i++)
        {
            Capsules[i].SetActive(true);
            yield return new WaitForSecondsRealtime(time);
        }
    }

}
