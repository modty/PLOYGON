using System.Collections;
using System.Collections.Generic;
using SurvivalEngine;
using UnityEngine;

public class CraftPanelController : UISlotPanel
{
        [Header("Craft Panel")]
        public Animator animator;


        private int selected_slot = -1;
        private UISlot prev_slot;


        protected override void Awake()
        {
            base.Awake();

        
            if (animator != null)
                animator.SetBool("Visible", IsVisible());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void Start()
        {
            base.Start();


            onClickSlot += OnClick;
            onPressAccept += OnAccept;
            onPressCancel += OnCancel;

            RefreshCategories();
        }

        private void RefreshCategories()
        {


          
        }

        public override void Show(bool instant = false)
        {
            base.Show(instant);

            CancelSelection();
            if (animator != null)
                animator.SetBool("Visible", IsVisible());

            RefreshCategories();
        }

        public override void Hide(bool instant = false)
        {
            base.Hide(instant);

            CancelSelection();
            if (animator != null)
                animator.SetBool("Visible", IsVisible());
        }

        private void OnClick(UISlot uislot)
        {
            if (uislot != null)
            {
                CategorySlot cslot = (CategorySlot)uislot;

                for (int i = 0; i < slots.Length; i++)
                    slots[i].UnselectSlot();
            }
        }


        private void OnAccept(UISlot slot)
        {
        }

        private void OnCancel(UISlot slot)
        {
            Toggle();
            UISlotPanel.UnfocusAll();
        }

        public void CancelSubSelection()
        {
        }

        public void CancelSelection()
        {
            selected_slot = -1;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null)
                    slots[i].UnselectSlot();
            }
            CancelSubSelection();
        }

        public int GetSelected()
        {
            return selected_slot;
        }

}
