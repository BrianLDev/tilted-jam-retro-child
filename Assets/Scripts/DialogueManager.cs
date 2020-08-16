using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
  public GameObject display;
  public TextMeshProUGUI dialogueTextDisplay;
  public TextMeshProUGUI nameDisplay;
    public string displayText;
    private float textPosition = 0;
    public static DialogueManager instance;
    public int stuffIndex=0;
    public bool talkingHere = false;
    public bool nextSceneOnComplete;
    public delegate void OnComplete();
    public OnComplete onComplete;
    public GameObject enableOnComplete;
    public string speakerName;

    // Start is called before the first frame update
    void Start()
    {
        DialogueManager.instance = this;
        if(talkingHere)StartDialogue(stuffToSay, speakerName);
    }
 void Update() {
      if(talkingHere) {
        if(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump")) {
          if(textPosition>displayText.Length)
            NextStuff();
        }
      }
      if(displayText!="") {
        textPosition += 1;//Time.deltaTime;
        int textIndex = (int)textPosition;
        if(textIndex>displayText.Length)textIndex=displayText.Length;
        dialogueTextDisplay.text = displayText.Substring(0,textIndex);
      }
      
    }

    public void DisplayText(string text) {
      textPosition = 0;
      displayText = text;
      dialogueTextDisplay.text = text;
    }

    public string[] stuffToSay;
    public void StartDialogue(string[] stuff, string name) {
      speakerName = name;
      nameDisplay.text = name;
      stuffToSay = stuff;
      stuffIndex=0;
      DisplayText(stuffToSay[stuffIndex]);
      stuffIndex = 0;
      talkingHere = true;
      display.SetActive(true);
    }
    public void EndDialogue() {
      displayText = "";
      talkingHere = false;
      display.SetActive(false);
      if(onComplete!=null) onComplete();
      if(nextSceneOnComplete) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
      }
      if(enableOnComplete) {
        enableOnComplete.SetActive(true);
      }
    }

    public void NextStuff() {
      stuffIndex +=1;
      if(stuffIndex >= stuffToSay.Length) {
        EndDialogue();
      } else {
        DisplayText(stuffToSay[stuffIndex]);
      }
    }


}
