using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PlayerCamera;
    public float focusTime;
    private bool started;
     void OnTriggerExit(Collider other)
    {
        if (started)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().isTrigger = false;
            PlayerCamera.SetActive(false);
            BattleManager.Instance.ActiveCapsules();
            StartCoroutine(ICameraFocus());
            started = true;
        }
        
    }
    IEnumerator ICameraFocus()
    {       
        yield return new WaitForSecondsRealtime(focusTime);
        PlayerCamera.SetActive(true);
    }

}
