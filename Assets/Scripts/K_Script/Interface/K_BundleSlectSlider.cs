using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UltimateClean
{

    public class K_BundleSlectSlider : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private FadeButton prevButton;
        [SerializeField]
        private FadeButton nextButton;

        [SerializeField]
        protected TextMeshProUGUI optionLabel;
        [SerializeField]
        protected TextMeshProUGUI optionNumberLabel;
#pragma warning restore 649

        public List<string> Options = new List<string>();

        public Image icon;
        public InventoryObject inventory;
        public K_UserInterface quickSlot;
        public int Index
        {
            get { return currentIndex; }
        }
        protected int currentIndex=0;

        public void OnPreviousButtonPressed()
        {
            --currentIndex;
            if (currentIndex < 0)
            {
                currentIndex = 0;
            }

            SetCurrentOptionLabel();
        }
        public void OnNextButtonPressed()
        {
            ++currentIndex;
            if (inventory.GetSlots[currentIndex].amount == 0)
            {
                currentIndex --;
            }

            SetCurrentOptionLabel();
        }
   

        private void Start()
        {
            
            SetCurrentOptionLabel();
        }
        protected void SetCurrentOptionLabel()
        {


            var slot = inventory.GetSlots[currentIndex];
            icon.sprite = inventory.database.ItemObjects[slot.item.id].uiDisplay;
            
            quickSlot.inventory.AddBundleListToWindow(inventory.database.ItemObjects[slot.item.id].subItem);
            
        }
        public void OnDestroy()
        {
            quickSlot.inventory.Clear();
        }

        public string GetCurrentOptionText()
        {
            return Options[currentIndex];
        }

        public void SetCurrentOption(int index)
        {
            if (index >= 0 && index < Options.Count)
            {
                currentIndex = index;
                icon.sprite = inventory.GetSlots[index].GetItemObject().uiDisplay;
               
                SetCurrentOptionLabel();
            }
        }
    }
}