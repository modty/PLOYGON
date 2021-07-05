using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
    /// 在你的物品栏或装备栏中显示单个物品的物品槽
    /// </summary>

    public class ItemSlot : UISlot
    {
        [Header("Item Slot")]
        public Image icon;
        public Text value;
        public Text title;
        public Text dura;

        [Header("Extra")]
        public Image default_icon;
        public Image highlight;
        public Image filter;

        private Animator animator;

        //private CraftData item;
        private int quantity;
        private float durability;

        private float highlight_opacity = 1f;

        protected override void Start()
        {
            base.Start();

            animator = GetComponent<Animator>();

            if (highlight)
            {
                highlight.enabled = false;
                highlight_opacity = highlight.color.a;
            }

            if (dura)
                dura.enabled = false;
        }

        protected override void Update()
        {
            base.Update();

            if (highlight != null)
            {
                highlight.enabled = selected || key_hover;
                float alpha = selected ? highlight_opacity : (highlight_opacity * 0.8f);
                highlight.color = new Color(highlight.color.r, highlight.color.g, highlight.color.b, alpha);
            }
        }


        public void SetSlotCustom(Sprite sicon, string title, int quantity, bool selected=false)
        {
            this.quantity = quantity;
            this.durability = 0f;
            icon.enabled = sicon != null;
            icon.sprite = sicon;
            value.text = quantity.ToString();
            value.enabled = quantity > 1;
            this.selected = selected;

            if (this.title != null)
            {
                this.title.enabled = selected;
                this.title.text = title;
            }

            if (dura != null)
                dura.enabled = false;

            if (filter != null)
                filter.enabled = false;

            if (default_icon != null)
                default_icon.enabled = false;

            gameObject.SetActive(true);
        }

        public void ShowTitle()
        {
            if (this.title != null)
                this.title.enabled = true;
        }

        public void SetDurability(int durability, bool show_value)
        {
            this.durability = durability;

            if (dura != null)
            {
                dura.enabled = show_value;
                dura.text = durability.ToString() + "%";
            }
        }

        public void SetFilter(int filter_level)
        {
            if (filter != null)
            {
                filter.enabled = filter_level > 0;
            }
        }

        public void Select()
        {
            this.selected = true;
            if (this.title != null)
                this.title.enabled = true;
        }

        public void Unselect()
        {
            this.selected = false;
            if (this.title != null)
                this.title.enabled = false;
        }

        public void AnimateGain()
        {
            if (animator != null)
                animator.SetTrigger("Gain");
        }

       

        public int GetQuantity()
        {
            return quantity;
        }

        public float GetDurability()
        {
            return durability; //This returns the DISPLAY value in %, not the actual durability value
        }


    }
