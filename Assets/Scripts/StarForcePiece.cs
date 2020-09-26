using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarForcePiece : MonoBehaviour
{
    public AudioClip collectAudio;
    public ActivateAfterDeaths activatething;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
      if(other.tag=="Player") {
        Destroy(gameObject);
        AudioManager.Instance.PlayClip(collectAudio);
      }
      activatething.Progress();
    }
}
