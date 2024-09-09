using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    public Item item;  // 슬롯에 있는 아이템
    public Image icon;  // 아이템 아이콘
    public Text quantityText;  // 아이템 수량 표시
    public InventoryPresenter inventoryPresenter;
    public Tooltip tooltip;

    private Vector3 originalPosition;  // 원래 위치 저장
    private bool isDragging = false;  // 드래그 상태 체크

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

    // 슬롯 업데이트 (아이콘과 수량 표시)
    public void UpdateSlot()
    {
        if (item != null)
        {
            icon.sprite = item.GetIcon();
            SetIconAlpha(1f);

            // 아이템 수량이 2개 이상일 때만 수량을 표시
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

    // 우클릭 이벤트 처리 - 장비 착용
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && item != null)
        {
            if (inventoryPresenter.CanEquipItem(item))
            {
                inventoryPresenter.EquipItem(item);  // 장비 착용
            }
        }
    }

    // 마우스 오버 시 장비 설명 표시
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

    // 마우스가 벗어났을 때 장비 설명 숨김
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            tooltip.HideTooltip();
        }
    }

    // 드래그 시작 시 원래 위치 저장
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            originalPosition = transform.localPosition;
            isDragging = true;
            tooltip.HideTooltip();
        }
    }

    // 드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            transform.position = eventData.position;
        }
    }

    // 드래그 종료 시 처리
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        // 드랍된 슬롯이 장비창이면 장비 착용
        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("EquipmentSlot"))
        {
            EquipmentSlot equipmentSlot = eventData.pointerEnter.GetComponent<EquipmentSlot>();
            if (equipmentSlot != null && inventoryPresenter.CanEquipItem(item))
            {
                inventoryPresenter.EquipItem(item);  // 장비 착용
            }
        }
        // 드랍된 곳이 인벤토리가 아니면 원래 자리로 복귀
        else
        {
            transform.localPosition = originalPosition;
        }
    }

    // 아이템 추가
    public void AddItem(Item newItem)
    {
        item = newItem;
        UpdateSlot();
    }

    // 아이템 제거
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
