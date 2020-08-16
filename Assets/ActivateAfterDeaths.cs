using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateAfterDeaths : MonoBehaviour
{
    public int num;
    private int progress;
    public AudioClip collectAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Progress() {
      progress++;
      if(progress>=num) {
        gameObject.SetActive(true);
      }
    }
    private void OnTriggerEnter(Collider other) {
      if(other.tag=="Player") {
        Destroy(gameObject);
        AudioManager.Instance.PlayClip(collectAudio);
        // Invoke("NextScene", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
      }
    }

    void NextScene() {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
