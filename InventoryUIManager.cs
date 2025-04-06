using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("The parent panel GameObject for the inventory UI.")]
    public GameObject inventoryPanel; // Assign the InventoryPanel from the Hierarchy here

    private bool isInventoryOpen = false; // Track the state

    void Start()
    {
        // Make sure the inventory starts closed
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
            isInventoryOpen = false;
        }
        else
        {
            Debug.LogError("Inventory Panel is not assigned in the InventoryUIManager script!");
        }
    }

    void Update()
    {
        // Check if the Inventory key ('I' by default) is pressed down
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null) return; // Safety check

        isInventoryOpen = !isInventoryOpen; // Flip the state
        inventoryPanel.SetActive(isInventoryOpen); // Set the panel's active state

        // Optional: Add logic here later if needed, like pausing the game
        // or changing cursor visibility when the inventory opens/closes.
        // Time.timeScale = isInventoryOpen ? 0f : 1f; // Example: Pause game
        // Cursor.visible = isInventoryOpen;
        // Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // You might add methods here later to update the displayed items dynamically
    // void UpdateDisplay(List<InventoryItem> items) { ... }
}
