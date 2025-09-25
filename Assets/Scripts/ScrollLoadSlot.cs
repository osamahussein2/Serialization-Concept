using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollLoadSlot : MonoBehaviour
{
    // Organize any game object related code here
    [Header("GameObjects")]
    [SerializeField] private List<Text> loadSlotTexts;
    [SerializeField] private Player player;
    [SerializeField] private PauseMenuSelection pauseMenuScript;

    // Create private game objects here
    private RawImage darkRedLoadSlotImage;

    // Create private variables here
    private int slotSelected = 5; // Start at select slot "Back"

    // Initialize any objects and load text files no matter if the object is active or not
    void Awake()
    {
        // Use the image component found on its own object
        darkRedLoadSlotImage = GetComponent<RawImage>();

        // Make sure to update the load text slots if a save file number if found on awake
        ValidateLoadFile(0, 1, "Slot 1 - Player Saved 1", "Slot 1 - No Data");
        ValidateLoadFile(1, 2, "Slot 2 - Player Saved 2", "Slot 2 - No Data");
        ValidateLoadFile(2, 3, "Slot 3 - Player Saved 3", "Slot 3 - No Data");
        ValidateLoadFile(3, 4, "Slot 4 - Player Saved 4", "Slot 4 - No Data");
        ValidateLoadFile(4, 5, "Slot 5 - Player Saved 5", "Slot 5 - No Data");
    }

    // Update is called once per frame
    void Update()
    {
        // Make the player also be able to press ESCAPE to exit the save game canvas
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Reset at "Back" selected slot as well
            slotSelected = 5;

            player.loadGameCanvas.gameObject.SetActive(false);
            player.pauseGameCanvas.gameObject.SetActive(true);
        }

        // Let's call the save slot functions to execute some save game logic
        SelectSaveSlot();
        SwitchSlots();

        // Update the load slot texts to match the ones from the save slot texts
        ValidateLoadFile(0, 1, "Slot 1 - Player Saved 1", "Slot 1 - No Data");
        ValidateLoadFile(1, 2, "Slot 2 - Player Saved 2", "Slot 2 - No Data");
        ValidateLoadFile(2, 3, "Slot 3 - Player Saved 3", "Slot 3 - No Data");
        ValidateLoadFile(3, 4, "Slot 4 - Player Saved 4", "Slot 4 - No Data");
        ValidateLoadFile(4, 5, "Slot 5 - Player Saved 5", "Slot 5 - No Data");
    }

    void SwitchSlots()
    {
        // Go up the selected slots
        if (Input.GetKeyDown(KeyCode.UpArrow) && slotSelected > 0)
        {
            slotSelected -= 1;
        }

        // Go down the selected slots
        else if (Input.GetKeyDown(KeyCode.DownArrow) && slotSelected >= 0 && slotSelected < 5)
        {
            slotSelected += 1;
        }
    }

    void SelectSaveSlot()
    {
        // Depending on the selected save slot, perform load slot or go back to pause menu tasks
        switch (slotSelected)
        {
            case 0:
                LoadSlot(new Vector2(0.0f, 35.0f), 1);
                break;

            case 1:
                LoadSlot(new Vector2(0.0f, 5.0f), 2);
                break;

            case 2:
                LoadSlot(new Vector2(0.0f, -25.0f), 3);
                break;

            case 3:
                LoadSlot(new Vector2(0.0f, -55.0f), 4);
                break;

            case 4:
                LoadSlot(new Vector2(0.0f, -85.0f), 5);
                break;

            case 5:
                GoBack();
                break;

            default:
                break;
        }
    }

    void ValidateLoadFile(int textIndex, int saveNumber, string saveValidText, string saveInvalidText)
    {
        // If player's save file does exist at a number, set its corresponding load text slot to valid save text
        if (player.CheckIfFileExists(saveNumber))
        {
            if (loadSlotTexts[textIndex].text != saveValidText)
                loadSlotTexts[textIndex].text = saveValidText;
        }

        /* However if the player's save file is not found at a number, set its corresponding load text slot to 
        invalid text */
        else if (!player.CheckIfFileExists(saveNumber))
        {
            if (loadSlotTexts[textIndex].text != saveInvalidText)
                loadSlotTexts[textIndex].text = saveInvalidText;
        }
    }

    void LoadSlot(Vector2 slotImagePosition, int saveNumber)
    {
        // Position the slot image where the load slot text is
        if (darkRedLoadSlotImage.rectTransform.anchoredPosition != slotImagePosition)
            darkRedLoadSlotImage.rectTransform.anchoredPosition = slotImagePosition;

        // If player presses ENTER or RETURN and the save file is valid
        if (Input.GetKeyDown(KeyCode.Return) && player.CheckIfFileExists(saveNumber))
        {
            // Reset at "Back" selected slot for load game canvas
            slotSelected = 5;

            // Reset at "Resume" selected slot for pause menu canvas
            pauseMenuScript.slotSelected = 0;

            // Load the player's save at a specific number and hide the load game canvas as well
            player.PlayerLoad(saveNumber);
            player.loadGameCanvas.gameObject.SetActive(false);
        }
    }

    void GoBack()
    {
        // Position the slot image where the back text is
        if (darkRedLoadSlotImage.rectTransform.anchoredPosition != new Vector2(0.0f, -125.0f))
            darkRedLoadSlotImage.rectTransform.anchoredPosition = new Vector2(0.0f, -125.0f);

        // Disable the load game canvas and enable pause game canvas again
        if (Input.GetKeyDown(KeyCode.Return))
        {
            player.loadGameCanvas.gameObject.SetActive(false);
            player.pauseGameCanvas.gameObject.SetActive(true);
        }
    }
}
