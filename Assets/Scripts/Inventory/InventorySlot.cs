using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    public Item item;  // ���Կ� �ִ� ������
    public Image icon;  // ������ ������
    public Text quantityText;  // ������ ���� ǥ��
    public InventoryPresenter inventoryPresenter;
    public Tooltip tooltip;

    private Vector3 originalPosition;  // ���� ��ġ ����
    private bool isDragging = false;  // �巡�� ���� üũ

    public void Initialize(InventoryPresenter presenter, Tooltip tooltip)
    {
        inventoryPresenter = presenter;
        this.tooltip = tooltip;
        UpdateSlot();
    }

    public bool IsEmpty()
    {
        return item == null;
    }

    // ���� ������Ʈ (�����ܰ� ���� ǥ��)
    public void UpdateSlot()
    {
        if (item != null)
        {
            icon.sprite = item.GetIcon();
            SetIconAlpha(1f);

            // ������ ������ 2�� �̻��� ���� ������ ǥ��
            if (item.Quantity > 1)
            {
                quantityText.text = item.Quantity.ToString();
                quantityText.enabled = true;
            }
            else
            {
                quantityText.enabled = false;
            }
        }
        else
        {
            icon.sprite = null;
            SetIconAlpha(0f);
            quantityText.enabled = false;
        }
    }

    // ��Ŭ�� �̺�Ʈ ó�� - ��� ����
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && item != null)
        {
            if (inventoryPresenter.CanEquipItem(item))
            {
                inventoryPresenter.EquipItem(item);  // ��� ����
            }
        }
    }

    // ���콺 ���� �� ��� ���� ǥ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && !string.IsNullOrEmpty(item.Description))
        {
            if (tooltip != null)
            {
                tooltip.ShowTooltip(item);
            }
            else
            {
                Debug.LogError("Tooltip is not assigned.");
            }
        }
    }

    // ���콺�� ����� �� ��� ���� ����
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            tooltip.HideTooltip();
        }
    }

    // �巡�� ���� �� ���� ��ġ ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            originalPosition = transform.localPosition;
            isDragging = true;
            tooltip.HideTooltip();
        }
    }

    // �巡�� ��
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            transform.position = eventData.position;
        }
    }

    // �巡�� ���� �� ó��
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        // ����� ������ ���â�̸� ��� ����
        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("EquipmentSlot"))
        {
            EquipmentSlot equipmentSlot = eventData.pointerEnter.GetComponent<EquipmentSlot>();
            if (equipmentSlot != null && inventoryPresenter.CanEquipItem(item))
            {
                inventoryPresenter.EquipItem(item);  // ��� ����
            }
        }
        // ����� ���� �κ��丮�� �ƴϸ� ���� �ڸ��� ����
        else
        {
            transform.localPosition = originalPosition;
        }
    }

    // ������ �߰�
    public void AddItem(Item newItem)
    {
        item = newItem;
        UpdateSlot();
    }

    // ������ ����
    public void RemoveItem()
    {
        item = null;
        UpdateSlot();
    }

    private void SetIconAlpha(float alpha)
    {
        if (icon != null)
        {
            Color color = icon.color;
            color.a = alpha;
            icon.color = color;
        }
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        SetIconAlpha(0f);
        quantityText.text = "";
        quantityText.enabled = false;
    }
}
