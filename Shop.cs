using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private static Shop instance;
    public static Shop Instance { get { return instance; } }

    public InventoryComponent playerInventory;
    public InventoryComponent shopInventory;

    public Canvas playerInventoryCanvas;
    public Canvas shopInventoryCanvas;

    public Text itemName;
    public Text itemDescription;
    public Text itemPrice;

    public Button BuyButton;
    public Button SellButton;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    ShopItem selectedItem;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();

        SetupInventoryCanvas();

        BuyButton.onClick.AddListener(BuyButtonClick);
        SellButton.onClick.AddListener(SellButtonClick);
    }

    void SetShopItems(InventoryComponent inventory, Canvas inventoryCanvas)
    {
        foreach (Transform child in inventoryCanvas.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in inventory.items)
        {
            Text newItem = new GameObject().AddComponent<Text>();
            ShopItem newShopItem = newItem.gameObject.AddComponent<ShopItem>();
            newShopItem.item = item;
            newShopItem.inventory = inventory;

            newItem.transform.SetParent(inventoryCanvas.transform, false);
            newItem.text = item.name;
            newItem.color = Color.green;
            newItem.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        }
    }

    void SetupInventoryCanvas()
    {
        SetShopItems(playerInventory, playerInventoryCanvas);
        SetShopItems(shopInventory, shopInventoryCanvas);
    }

    void UpdateInventoryCanvas(Canvas inventoryCanvas)
    {
        selectedItem.transform.SetParent(inventoryCanvas.transform);
        selectedItem.GetComponent<Text>().color = Color.green;
        selectedItem = null;
        itemName.text = "";
        itemDescription.text = "";
        itemPrice.text = "";
    }

    public void BuyButtonClick()
    {
        Debug.Log("Buying an item");
        Shop.Instance.OnBuyButtonClick();
    }

    public void SellButtonClick()
    {
        Debug.Log("Selling an item");
        Shop.Instance.OnSellButtonClick();
    }

    public void OnBuyButtonClick()
    {
        if (selectedItem && shopInventory == selectedItem.inventory)
        {
            playerInventory.BuyItem(selectedItem.item);
            shopInventory.SellItem(selectedItem.item);
            selectedItem.inventory = playerInventory;
            UpdateInventoryCanvas(playerInventoryCanvas);
        }
    }

    public void OnSellButtonClick()
    {
        if (selectedItem && playerInventory == selectedItem.inventory)
        {
            playerInventory.SellItem(selectedItem.item);
            shopInventory.BuyItem(selectedItem.item);
            selectedItem.inventory = shopInventory;
            UpdateInventoryCanvas(shopInventoryCanvas);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            
            m_PointerEventData = new PointerEventData(m_EventSystem);
            
            m_PointerEventData.position = Input.mousePosition;

            
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(m_PointerEventData, results);

            foreach (RaycastResult result in results)
            {
                ShopItem trySelectItem = result.gameObject.GetComponent<ShopItem>();

                if (trySelectItem)
                {
                    if (selectedItem)
                    {
                        selectedItem.GetComponent<Text>().color = Color.black;
                    }

                    selectedItem = trySelectItem;
                    selectedItem.GetComponent<Text>().color = Color.red;
                    itemName.text = selectedItem.item.name;
                    itemDescription.text = selectedItem.item.description;
                    itemPrice.text = selectedItem.item.price.ToString();
                }
            }
        }
    }
}
