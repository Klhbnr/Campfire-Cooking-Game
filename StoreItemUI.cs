using UnityEngine;
using UnityEngine.UI; // Required for Button
using TMPro;

public class StoreItemUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private Button buyButton; // Add reference to the button

    // --- Private Variables ---
    private StoreItem currentItemData;
    private SupermarketUIManager uiManager; // Reference to the main manager

    /// <summary>
    /// Configures the UI element with item data and sets up the buy button.
    /// </summary>
    /// <param name="itemData">The data for the item to display.</param>
    /// <param name="manager">Reference to the SupermarketUIManager to handle the purchase.</param>
    public void Setup(StoreItem itemData, SupermarketUIManager manager)
    {
        this.currentItemData = itemData;
        this.uiManager = manager; // Store the reference

        // Update Text Fields
        if (itemNameText != null) itemNameText.text = currentItemData.itemName;
        if (itemPriceText != null) itemPriceText.text = $"{currentItemData.price} G"; // Or your currency format

        // Setup Button Listener
        if (buyButton != null)
        {
            // Clear previous listeners to prevent duplicates if Setup is called again
            buyButton.onClick.RemoveAllListeners();
            // Add a listener that calls our HandleBuyClick method when clicked
            buyButton.onClick.AddListener(HandleBuyClick);
            // Optional: Initially disable button if item data is invalid?
            // buyButton.interactable = (currentItemData != null && uiManager != null);
        }
        else
        {
            Debug.LogWarning("Buy Button is not assigned in the StoreItemUI prefab!", this.gameObject);
        }
    }

    /// <summary>
    /// Called when the Buy Button associated with this item is clicked.
    /// </summary>
    private void HandleBuyClick()
    {
        if (currentItemData != null && uiManager != null)
        {
            // Tell the SupermarketUIManager to attempt the purchase
            uiManager.AttemptPurchase(currentItemData);
        }
        else
        {
            Debug.LogError("Cannot handle buy click: ItemData or UIManager reference is missing.", this.gameObject);
        }
    }

    // Optional: Add a method to update button interactability based on player currency
    public void UpdateInteractability(int playerCurrency)
    {
        if (buyButton != null && currentItemData != null)
        {
            buyButton.interactable = (playerCurrency >= currentItemData.price);
        }
    }
}