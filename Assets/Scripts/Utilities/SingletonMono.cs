using UnityEngine;

namespace UnityUtilities {
    /// <summary>
    /// This singleton is a template that can be inherited by any class that needs global access to only 1 instance
    /// e.g. GameManagers or AudioPlayers
    /// Named "SingletonMono" to differentiate from the future DOTS/ECS version which won't use MonoBehaviour
    /// Declaration syntax:     
    ///     public class UIManager : SingletonMono<UIManager>
    ///
    /// </summary>
    
    public class SingletonMono<T> : MonoBehaviour  where T : MonoBehaviour{        
        public static T Instance { get; private set; }
        [SerializeField] private bool _parentDontDestroyOnLoad = true;
        
        protected void Awake() {
            if (Instance != null) {
                Debug.LogWarning("Warning: " + typeof(T) + " Singleton already exists.  Destroying newly created one.");
                Destroy(this.gameObject);
            }
            else { 
                Instance = (T)FindObjectOfType(typeof(T) );
            }

            if (_parentDontDestroyOnLoad) {
                DontDestroyOnLoad(this.transform.root.gameObject);  // Makes the root parent don't destroy on load. Required by Unity
            }
        }
    }
}
