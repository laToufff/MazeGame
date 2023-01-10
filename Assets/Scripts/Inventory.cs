using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Item[] items = new Item[3];
    [SerializeField] Sprite[] slotBgs = new Sprite[3];
    [SerializeField] Image slotBgImage;
    [SerializeField] Image[] slots = new Image[3];
    [SerializeField] Sprite emptySlot;
    public int activeSlot = 0;
    public Item activeItem { get { return items[activeSlot]; } }

    private void Start() {
        UpdateUI();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ChangeActiveSlot(0);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            ChangeActiveSlot(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            ChangeActiveSlot(2);
        }

        if (Input.mouseScrollDelta.y > 0) {
            ChangeActiveSlot((activeSlot + 1) % 3);
        } else if (Input.mouseScrollDelta.y < 0) {
            ChangeActiveSlot((activeSlot + 2) % 3);
        }

        if (Input.GetMouseButtonDown(0)) {
            if (activeItem != null) {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                activeItem.Use(mousePosition);
            }
        }
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                // item is added to the inventory
                return true;
            }
        }
        // inventory is full
        return false;
    }

    private void ChangeActiveSlot(int slot) {
        activeSlot = slot;
        slotBgImage.sprite = slotBgs[slot];
    }

    private void UpdateUI() {
        for (int i = 0; i < items.Length; i++) {
            if (items[i] != null) {
                slots[i].sprite = items[i].icon;
            } else {
                slots[i].sprite = emptySlot;
            }
        }
    }

}
