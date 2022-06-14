using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LobbyItems : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject itemPopup;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject enteredObject = eventData.pointerEnter;

        if (enteredObject.CompareTag("ShopItem") || enteredObject.CompareTag("InventoryItem"))
        {
            itemPopup.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject enteredObject = eventData.pointerEnter;

        if (enteredObject.CompareTag("ShopItem") || enteredObject.CompareTag("InventoryItem"))
        {
            itemPopup.SetActive(false);
        }
    }
}
