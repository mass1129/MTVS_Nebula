using UnityEngine;

namespace UltimateClean
{
    /// <summary>
    /// This class is responsible for creating and opening a popup of the
    /// given prefab and adding it to the UI canvas of the current scene.
    /// </summary>
    public class K_PopupOpener : MonoBehaviour
    {
        public GameObject popupPrefab;

        public bool isPopup;
      

        protected void Start()
        {
           
        }

        public virtual void OpenPopup()
        {

            popupPrefab.SetActive(true);
            if(isPopup)
            {
                popupPrefab.transform.localScale = Vector3.zero;
                popupPrefab.GetComponent<K_Popup>().Open();
            }
          
        }
    }
}

