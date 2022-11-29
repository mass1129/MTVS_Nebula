using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UltimateClean
{

    public class K_NonLoopSlectSlider : MonoBehaviour
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
        protected int currentIndex;

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
   
        private void OnEnable()
        {
            SetCurrentOptionLabel();
        }

        protected void SetCurrentOptionLabel()
        {


            var slot = inventory.GetSlots[currentIndex];
            icon.sprite = inventory.database.ItemObjects[slot.item.id].uiDisplay;
            quickSlot.inventory.Clear();
            quickSlot.inventory.AddBundleListToWindow(inventory.database.ItemObjects[slot.item.id].subItem);
            quickSlot.inventory.UpdateInventory();
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