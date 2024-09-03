using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemIcon;
    public Text itemCountText;
    private Transform originalParent;
    private InventoryPresenter presenter;
    private Item currentItem;
    public Tooltip tooltip;

    public void Initialize(Item item, InventoryPresenter inventoryPresenter, Tooltip tooltipInstance)
    {
        currentItem = item;
        itemIcon.sprite = item.Icon;
        presenter = inventoryPresenter;
        tooltip = tooltipInstance;

        UpdateItemCount();
    }

    private void UpdateItemCount()
    {
        if(currentItem.Quantity > 1)
        {
            itemCountText.text = currentItem.Quantity.ToString();
            itemCountText.gameObject.SetActive(true);
        }
        else
        {
            itemCountText.gameObject.SetActive(false);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        itemIcon.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
        itemIcon.raycastTarget = true;

        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("InventorySlot"))
        {
            InventorySlot targetSlot = eventData.pointerEnter.GetComponent<InventorySlot>();
            if (targetSlot != null)
            {
                presenter.SwapItems(currentItem, targetSlot.GetItem());
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(currentItem != null)
        {
            tooltip.ShowTooltip(currentItem.ItemName + "\n" + currentItem.Description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }

    public Item GetItem()
    {
        return currentItem;
    }

    public void SetItem(Item item)
    {
        currentItem = item;
        itemIcon.sprite = item.Icon;
    }
}
