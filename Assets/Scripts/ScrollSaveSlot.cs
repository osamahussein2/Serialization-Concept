using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSaveSlot : MonoBehaviour
{
    // Organize any game object related code here
    [Header("GameObjects")]
    [SerializeField] private List<Text> saveSlotTexts;

    // Organize any scrolling related variables here
    [Header("Scrolling Parameters")]
    [SerializeField] private float typeInSeconds = 0.2f;

    // Create private game objects here
    private RawImage darkRedSlotImage;
    private Player player;

    // Create private variables here
    private int slotSelected = 5; // Start at select slot "Do Not Save"
    private bool typewritingDone = true; // Typewriter check

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Use the image component found on its own object
        darkRedSlotImage = GetComponent<RawImage>();

        // Find the player script inside the Player game object
        player = GameObject.Find("Player").GetComponent<Player>();
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
        // Go down the selected slots (make sure typewriting is finished first)
        if (Input.GetKeyDown(KeyCode.UpArrow) && slotSelected > 0 && typewritingDone)
        {
            slotSelected -= 1;
        }

        // Go up the selected slots (make sure typewriting is finished first)
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
                SaveSlot1();
                break;

            case 1:
                SaveSlot2();
                break;

            case 2:
                SaveSlot3();
                break;

            case 3:
                SaveSlot4();
                break;

            case 4:
                SaveSlot5();
                break;

            case 5:
                DoNotSaveLogic();
                break;

            default:
                break;
        }
    }

    void SaveSlot1()
    {
        // Position the slot image where the save slot text is
        if (darkRedSlotImage.rectTransform.anchoredPosition != new Vector2(0.0f, 35.0f))
            darkRedSlotImage.rectTransform.anchoredPosition = new Vector2(0.0f, 35.0f);

        if (Input.GetKeyDown(KeyCode.Return) && typewritingDone)
        {
            //player.PlayerSave(1);

            saveSlotTexts[0].text = "";

            StartCoroutine(TypewriteSaveSlot(0, "Slot 1 - Player Saved 1", typeInSeconds));
            typewritingDone = false;
        }

        else if (!typewritingDone && saveSlotTexts[0].text == "Slot 1 - Player Saved 1")
        {
            typewritingDone = true;
        }
    }

    void SaveSlot2()
    {
        // Position the slot image where the save slot text is
        if (darkRedSlotImage.rectTransform.anchoredPosition != new Vector2(0.0f, 5.0f))
            darkRedSlotImage.rectTransform.anchoredPosition = new Vector2(0.0f, 5.0f);

        if (Input.GetKeyDown(KeyCode.Return) && typewritingDone)
        {
            //player.PlayerSave(2);

            saveSlotTexts[1].text = "";

            StartCoroutine(TypewriteSaveSlot(1, "Slot 2 - Player Saved 2", typeInSeconds));
            typewritingDone = false;
        }

        else if (!typewritingDone && saveSlotTexts[1].text == "Slot 2 - Player Saved 2")
        {
            typewritingDone = true;
        }
    }

    void SaveSlot3()
    {
        // Position the slot image where the save slot text is
        if (darkRedSlotImage.rectTransform.anchoredPosition != new Vector2(0.0f, -25.0f))
            darkRedSlotImage.rectTransform.anchoredPosition = new Vector2(0.0f, -25.0f);

        if (Input.GetKeyDown(KeyCode.Return) && typewritingDone)
        {
            //player.PlayerSave(3);

            saveSlotTexts[2].text = "";

            StartCoroutine(TypewriteSaveSlot(2, "Slot 3 - Player Saved 3", typeInSeconds));
            typewritingDone = false;
        }

        else if (!typewritingDone && saveSlotTexts[2].text == "Slot 3 - Player Saved 3")
        {
            typewritingDone = true;
        }
    }

    void SaveSlot4()
    {
        // Position the slot image where the save slot text is
        if (darkRedSlotImage.rectTransform.anchoredPosition != new Vector2(0.0f, -55.0f))
            darkRedSlotImage.rectTransform.anchoredPosition = new Vector2(0.0f, -55.0f);

        if (Input.GetKeyDown(KeyCode.Return) && typewritingDone)
        {
            //player.PlayerSave(4);

            saveSlotTexts[3].text = "";

            StartCoroutine(TypewriteSaveSlot(3, "Slot 4 - Player Saved 4", typeInSeconds));
            typewritingDone = false;
        }

        else if (!typewritingDone && saveSlotTexts[3].text == "Slot 4 - Player Saved 4")
        {
            typewritingDone = true;
        }
    }

    void SaveSlot5()
    {
        // Position the slot image where the save slot text is
        if (darkRedSlotImage.rectTransform.anchoredPosition != new Vector2(0.0f, -85.0f))
            darkRedSlotImage.rectTransform.anchoredPosition = new Vector2(0.0f, -85.0f);

        if (Input.GetKeyDown(KeyCode.Return) && typewritingDone)
        {
            //player.PlayerSave(5);

            saveSlotTexts[4].text = "";

            StartCoroutine(TypewriteSaveSlot(4, "Slot 5 - Player Saved 5", typeInSeconds));
            typewritingDone = false;
        }

        else if (!typewritingDone && saveSlotTexts[4].text == "Slot 5 - Player Saved 5")
        {
            typewritingDone = true;
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
