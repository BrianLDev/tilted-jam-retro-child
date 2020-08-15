using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;


namespace UnityUtilities
{
    /// <summary>
    /// 
    /// </summary>
    public class TextTypingEffect : MonoBehaviour
    {

        public enum TypingSpeedTypes { Blazing, VeryFast, Fast, Normal, Slow, VerySlow, OcarinaOfTime }
        public Dictionary<TypingSpeedTypes, float> typingSpeedDict = new Dictionary <TypingSpeedTypes, float> {
            {TypingSpeedTypes.Blazing, 0.001f}, {TypingSpeedTypes.VeryFast, 0.005f}, {TypingSpeedTypes.Fast, 0.01f}, 
            {TypingSpeedTypes.Normal, 0.02f}, 
            {TypingSpeedTypes.Slow, 0.05f}, {TypingSpeedTypes.VerySlow, 0.1f}, {TypingSpeedTypes.OcarinaOfTime, 0.2f} 
        };
        public TypingSpeedTypes typingSpeed = TypingSpeedTypes.Normal;

        private TMP_Text m_TextComponent;
        private bool m_SpeedButtonPressed = false;  // TODO: Add input check to see when button pressed
        private bool m_HasTextChanged;
        private bool m_IsTyping = true; 


        void Awake() {
            m_TextComponent = gameObject.GetComponent<TMP_Text>();
        }

        void Start() {
            StartCoroutine(RevealCharacters(m_TextComponent));
            //StartCoroutine(RevealWords(m_TextComponent));
        }

        void OnEnable() {
            // Subscribe to event fired when text object has been regenerated.
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
        }

        void OnDisable() {
            // Unsubscribe to event fired when text object has been regenerated.
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
        }

        // Event received when the text object has changed.
        void ON_TEXT_CHANGED(Object obj) {
            m_HasTextChanged = true;
        }

        /// <summary>
        /// Method revealing the text one character at a time.
        /// </summary>
        /// <returns></returns>
        IEnumerator RevealCharacters(TMP_Text textComponent) {
            textComponent.ForceMeshUpdate();

            TMP_TextInfo textInfo = textComponent.textInfo;

            int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
            int visibleCount = 0;

            while(m_IsTyping) {   // Coroutine types each character in this loop
                if (m_HasTextChanged) {
                    totalVisibleCharacters = textInfo.characterCount; // Update visible character count.
                    m_HasTextChanged = false; 
                    float waitTime = m_SpeedButtonPressed ? typingSpeedDict[TypingSpeedTypes.Blazing] : typingSpeedDict[typingSpeed];    // set to blazing speed if speed button pressed
                    yield return new WaitForSeconds( waitTime );   // add a small delay between each character typed
                }

                if (visibleCount > totalVisibleCharacters) {    // reached end of text.  Wait 1 second and start over
                    yield return new WaitForSeconds(1.0f);
                    visibleCount = 0;
                    m_IsTyping = false;
                }

                textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

                visibleCount += 1;

                yield return null;
            }

        }


        /// <summary>
        /// Method revealing the text one word at a time.
        /// </summary>
        /// <returns></returns>
        IEnumerator RevealWords(TMP_Text textComponent) {
            textComponent.ForceMeshUpdate();

            int totalWordCount = textComponent.textInfo.wordCount;
            int totalVisibleCharacters = textComponent.textInfo.characterCount; // Get # of Visible Character in text object
            int counter = 0;
            int currentWord = 0;
            int visibleCount = 0;

            while (m_IsTyping) {
                currentWord = counter % (totalWordCount + 1);

                // Get last character index for the current word.
                if (currentWord == 0) // Display no words.
                    visibleCount = 0;
                else if (currentWord < totalWordCount) // Display all other words with the exception of the last one.
                    visibleCount = textComponent.textInfo.wordInfo[currentWord - 1].lastCharacterIndex + 1;
                else if (currentWord == totalWordCount) // Display last word and all remaining characters.
                    visibleCount = totalVisibleCharacters;

                textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

                // Once the last character has been revealed, wait x seconds and start over.
                if (visibleCount >= totalVisibleCharacters) {   // last word typed
                    yield return new WaitForSeconds(1.0f);
                    m_IsTyping = false;
                }

                counter += 1;

                float waitTime = m_SpeedButtonPressed ? typingSpeedDict[TypingSpeedTypes.Blazing] : typingSpeedDict[typingSpeed];    // set to blazing speed if speed button pressed
                yield return new WaitForSeconds( waitTime );   // add a small delay between each character typed
            }
        }

    }
}