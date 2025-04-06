using UnityEngine;
using UnityEngine.UI; // Not strictly needed here unless accessing Button properties directly
using TMPro;
using System.Collections.Generic;

// Make the StoreItem class visible outside if needed, or keep nested
[System.Serializable]
public class StoreItem
{
    public string itemName;
    public int price;
    // public string itemID; // Good to add unique IDs later
}

// Add this attribute to ensure an AudioSource exists
[RequireComponent(typeof(AudioSource))]
public class SupermarketUIManager : MonoBehaviour
{
    // --- Existing UI Elements ---
    [Header("UI Elements")]
    [SerializeField] private GameObject supermarketPanel;
    [SerializeField] private TextMeshProUGUI playerCurrencyText;
    [SerializeField] private GameObject storeItemPrefab;
    [SerializeField] private Transform itemContainer;

    // --- Existing Input Settings ---
    [Header("Input Settings")]
    [SerializeField] private KeyCode toggleKey = KeyCode.O;

    // --- Existing Store Content ---
    [Header("Store Content")]
    [SerializeField] private List<StoreItem> availableItems = new List<StoreItem>();

    // --- Existing Player Data ---
    [Header("Player Data (Placeholder)")]
    [SerializeField] private int placeholderPlayerCurrency = 500;

    // --- NEW: Audio Feedback ---
    [Header("Audio Feedback")]
    [Tooltip("Sound to play on successful purchase.")]
    [SerializeField] private AudioClip purchaseSuccessSound;
    [Tooltip("Sound to play when purchase fails (e.g., not enough gold).")]
    [SerializeField] private AudioClip purchaseFailSound;

    // --- Private Variables ---
    private bool isSupermarketOpen = false;
    private AudioSource audioSource; // Reference to the AudioSource component
    // Keep track of the instantiated UI elements to update interactability
    private List<StoreItemUI> instantiatedItemUIs = new List<StoreItemUI>();


    // --- Unity Methods ---
    void Awake() // Use Awake for component fetching
    {
         // Get the AudioSource component attached to this GameObject
         audioSource = GetComponent<AudioSource>();
         if(audioSource == null)
         {
             Debug.LogError("SupermarketUIManager requires an AudioSource component.", this.gameObject);
         }
    }

    void Start()
    {
        // Ensure panel starts closed
        if (supermarketPanel != null)
        {
            supermarketPanel.SetActive(false);
            isSupermarketOpen = false;
        } else { Debug.LogError("Supermarket Panel not assigned!", this.gameObject); }

        UpdateCurrencyDisplay(); // Initial update

        // Validate other essential references
        if (playerCurrencyText == null) Debug.LogError("Player Currency Text not assigned!", this.gameObject);
        if (storeItemPrefab == null) Debug.LogError("Store Item Prefab not assigned!", this.gameObject);
        if (itemContainer == null) Debug.LogError("Item Container not assigned!", this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleSupermarket();
        }
    }

    // --- Public Methods ---
    public void ToggleSupermarket()
    {
        isSupermarketOpen = !isSupermarketOpen;

        if (supermarketPanel != null)
        {
            supermarketPanel.SetActive(isSupermarketOpen);

            if (isSupermarketOpen)
            {
                Debug.Log("Supermarket Opened");
                PopulateItemList();
                UpdateCurrencyDisplay();
                UpdateAllButtonInteractability(); // Check buttons when opening
                // Optional: Cursor/Pause logic
            }
            else
            {
                Debug.Log("Supermarket Closed");
                ClearItemList();
                // Optional: Cursor/Pause logic
            }
        }
    }

    /// <summary>
    /// Called by StoreItemUI instances when their Buy button is clicked.
    /// </summary>
    /// <param name="itemToBuy">The data of the item being purchased.</param>
    public void AttemptPurchase(StoreItem itemToBuy)
    {
        if (itemToBuy == null)
        {
            Debug.LogError("AttemptPurchase called with null item data.");
            return;
        }

        Debug.Log($"Attempting to buy {itemToBuy.itemName} for {itemToBuy.price} G. Current Gold: {placeholderPlayerCurrency}");

        // Check if player can afford it
        if (placeholderPlayerCurrency >= itemToBuy.price)
        {
            // --- Purchase Success ---
            placeholderPlayerCurrency -= itemToBuy.price; // Deduct cost
            UpdateCurrencyDisplay(); // Update UI text

            // Confirmation Message & Sound
            Debug.Log($"Successfully purchased {itemToBuy.itemName}!");
            PlaySound(purchaseSuccessSound); // Play success sound

            // TODO: Add the purchased item to the player's inventory system here!

            // Update button interactability AFTER currency changes
            UpdateAllButtonInteractability();

        }
        else
        {
            // --- Purchase Failed ---
            Debug.Log($"Insufficient funds to purchase {itemToBuy.itemName}. Need {itemToBuy.price} G.");
            PlaySound(purchaseFailSound); // Play failure sound
        }
    }


    // --- Helper Methods ---

    void UpdateCurrencyDisplay()
    {
        if (playerCurrencyText != null)
        {
            playerCurrencyText.text = $"Gold: {placeholderPlayerCurrency}";
        }
    }

    void ClearItemList()
    {
        if (itemContainer == null) return;
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }
        instantiatedItemUIs.Clear(); // Clear the list of UI scripts
    }

    void PopulateItemList()
    {
        if (itemContainer == null || storeItemPrefab == null) return;

        ClearItemList(); // Clear old items and the UI list

        Debug.Log($"Populating store with {availableItems.Count} items.");

        foreach (StoreItem item in availableItems)
        {
            GameObject itemGO = Instantiate(storeItemPrefab, itemContainer);
            StoreItemUI itemUI = itemGO.GetComponent<StoreItemUI>();

            if (itemUI != null)
            {
                // Pass the item data AND a reference to this manager script
                itemUI.Setup(item, this);
                instantiatedItemUIs.Add(itemUI); // Add to our list
            }
            else
            {
                Debug.LogError($"Instantiated item prefab is missing StoreItemUI script!", itemGO);
                Destroy(itemGO); // Clean up broken instance
            }
        }
    }

    /// <summary>
    /// Updates the interactable state of all buy buttons based on current currency.
    /// </summary>
    void UpdateAllButtonInteractability()
    {
         foreach(StoreItemUI itemUI in instantiatedItemUIs)
         {
             itemUI.UpdateInteractability(placeholderPlayerCurrency);
         }
    }


    /// <summary>
    /// Plays a given audio clip if the AudioSource and clip are valid.
    /// </summary>
    /// <param name="clipToPlay">The AudioClip to play.</param>
    private void PlaySound(AudioClip clipToPlay)
    {
        if (audioSource != null && clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay); // PlayOneShot is good for UI sounds
        }
        else if (clipToPlay != null)
        {
             Debug.LogWarning("AudioSource missing, cannot play sound.", this.gameObject);
        }
         // else: No warning if the clip itself isn't assigned, might be intentional
    }

    // --- Keep existing Open/Close methods (ensure they call populate/clear) ---
     public void OpenSupermarket()
    {
         if (supermarketPanel != null && !isSupermarketOpen)
         {
             // Simplified - ToggleSupermarket handles the logic now
             ToggleSupermarket();
         }
    }

    public void CloseSupermarket()
    {
         if (supermarketPanel != null && isSupermarketOpen)
         {
              // Simplified - ToggleSupermarket handles the logic now
             ToggleSupermarket();
         }
    }
}