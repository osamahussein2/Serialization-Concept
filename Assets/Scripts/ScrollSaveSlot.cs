using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSaveSlot : MonoBehaviour
{
    // Organize any game object related code here
    [Header("GameObjects")]
    [SerializeField] private List<Text> saveSlotTexts;

    // Organize any typewriting related variables here
    [Header("Handle Typewriting")]
    [SerializeField] private float typeInSeconds = 0.2f;

    // Create private game objects here
    private RawImage darkRedSlotImage;
    private Player player;

    // Create private variables here
    private int slotSelected = 5; // Start at select slot "Do Not Save"
    private bool typewritingDone = true; // Typewriter check

    // Initialize any objects and save text files no matter if the object is active or not
    void Awake()
    {
        // Use the image component found on its own object
        darkRedSlotImage = GetComponent<RawImage>();

        // Find the player script inside the Player game object
        player = GameObject.Find("Player").GetComponent<Player>();

        // Make sure to update the save text slots if a save file number if found on awake
        ValidateSaveFile(0, 1, "Slot 1 - Player Saved 1", "Slot 1 - No Data");
        ValidateSaveFile(1, 2, "Slot 2 - Player Saved 2", "Slot 2 - No Data");
        ValidateSaveFile(2, 3, "Slot 3 - Player Saved 3", "Slot 3 - No Data");
        ValidateSaveFile(3, 4, "Slot 4 - Player Saved 4", "Slot 4 - No Data");
        ValidateSaveFile(4, 5, "Slot 5 - Player Saved 5", "Slot 5 - No Data");
    }

    // Update is called once per frame
    void Update()
    {
        // Make the player also be able to press ESCAPE to exit the save game canvas
        if (Input.GetKeyDown(KeyCode.Escape) && typewritingDone)
        {
            // Reset at "Do Not Save" selected slot as well
            slotSelected = 5;

            player.saveGameCanvas.gameObject.SetActive(false);
        }

        // Let's call the save slot functions to execute some save game logic
        SelectSaveSlot();
        SwitchSlots();
    }

    void SwitchSlots()
    {
        // Go up the selected slots (make sure typewriting is finished first)
        if (Input.GetKeyDown(KeyCode.UpArrow) && slotSelected > 0 && typewritingDone)
        {
            slotSelected -= 1;
        }

        // Go down the selected slots (make sure typewriting is finished first)
        else if (Input.GetKeyDown(KeyCode.DownArrow) && slotSelected >= 0 && slotSelected < 5 && typewritingDone)
        {
            slotSelected += 1;
        }
    }

    void SelectSaveSlot()
    {
        // Depending on the selected save slot, perform save slot or do not save tasks
        switch (slotSelected)
        {
            case 0:
                SaveSlot(0, 1, "Slot 1 - Player Saved 1", new Vector2(0.0f, 35.0f));
                break;

            case 1:
                SaveSlot(1, 2, "Slot 2 - Player Saved 2", new Vector2(0.0f, 5.0f));
                break;

            case 2:
                SaveSlot(2, 3, "Slot 3 - Player Saved 3", new Vector2(0.0f, -25.0f));
                break;

            case 3:
                SaveSlot(3, 4, "Slot 4 - Player Saved 4", new Vector2(0.0f, -55.0f));
                break;

            case 4:
                SaveSlot(4, 5, "Slot 5 - Player Saved 5", new Vector2(0.0f, -85.0f));
                break;

            case 5:
                DoNotSaveLogic();
                break;

            default:
                break;
        }
    }

    void ValidateSaveFile(int textIndex, int saveNumber, string saveValidText, string saveInvalidText)
    {
        // Make sure to update save slot texts to valid save files
        if (player.CheckIfFileExists(saveNumber))
        {
            if (saveSlotTexts[textIndex].text != saveValidText)
                saveSlotTexts[textIndex].text = saveValidText;
        }

        // Make sure to update save slot texts to invalid save files
        else if (!player.CheckIfFileExists(saveNumber))
        {
            if (saveSlotTexts[textIndex].text != saveInvalidText)
                saveSlotTexts[textIndex].text = saveInvalidText;
        }
    }

    void SaveSlot(int textIndex, int saveNumber, string saveText, Vector2 slotImagePosition)
    {
        // Position the slot image where the save slot text is
        if (darkRedSlotImage.rectTransform.anchoredPosition != slotImagePosition)
            darkRedSlotImage.rectTransform.anchoredPosition = slotImagePosition;

        // Check if the player presses ENTER or RETURN and the typewriting is finished
        if (Input.GetKeyDown(KeyCode.Return) && typewritingDone)
        {
            // Save game's information using a save number
            player.PlayerSave(saveNumber);

            // Start save slot text to be empty
            saveSlotTexts[textIndex].text = "";

            // Start typewriting the save slot text and set typewriting done to false (disables player input)
            StartCoroutine(TypewriteSaveSlot(textIndex, saveText, typeInSeconds));
            typewritingDone = false;
        }

        // Otherwise, if the save slot text does equal to some save text and typewriting is NOT set to done
        else if (!typewritingDone && saveSlotTexts[textIndex].text == saveText)
        {
            typewritingDone = true; // Set typewriting to DONE
        }
    }

    void DoNotSaveLogic()
    {
        // Position the slot image where the save slot text is
        if (darkRedSlotImage.rectTransform.anchoredPosition != new Vector2(0.0f, -125.0f))
            darkRedSlotImage.rectTransform.anchoredPosition = new Vector2(0.0f, -125.0f);

        // Disable the save game canvas
        if (Input.GetKeyDown(KeyCode.Return) && typewritingDone)
        {
            player.saveGameCanvas.gameObject.SetActive(false);
        }
    }

    // Perform coroutine for typewriter effect
    private IEnumerator TypewriteSaveSlot(int textIndex, string text, float seconds)
    {
        // Loop through the character array inside the passed in text parameter
        foreach (char character in text.ToCharArray())
        {
            // Set save slot [textIndex] to increment its text based on current character array
            saveSlotTexts[textIndex].text += character;
            yield return new WaitForSeconds(seconds); // Wait for a certain amount of seconds
        }
    }
}
