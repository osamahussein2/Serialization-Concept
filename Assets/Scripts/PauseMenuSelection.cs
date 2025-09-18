using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PauseMenuSelection : MonoBehaviour
{
    // Create private game objects here
    private RawImage darkRedPauseSelectionImage;
    private Player player;

    // Create private variables here
    private int slotSelected = 0; // Start at select "Resume" option

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Use the image component found on its own object
        darkRedPauseSelectionImage = GetComponent<RawImage>();

        // Find the player script inside the Player game object
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // Detect ESCAPE key press when pause menu is active
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Reset at "Resume" selected option as well
            slotSelected = 0;

            player.pauseGameCanvas.gameObject.SetActive(false);
        }

        // Execute selection of pause menu option and logic for those options
        SwitchOption();
        PauseMenuLogic();
    }

    void SwitchOption()
    {
        // Go down the selected options
        if (Input.GetKeyDown(KeyCode.UpArrow) && slotSelected > 0)
        {
            slotSelected -= 1;
        }

        // Go up the selected options
        else if (Input.GetKeyDown(KeyCode.DownArrow) && slotSelected >= 0 && slotSelected < 2)
        {
            slotSelected += 1;
        }
    }

    void PauseMenuLogic()
    {
        // Depending on the selected pause menu option, execute different pause menu states
        switch (slotSelected)
        {
            case 0: // Resume

                ResumeGame();
                break;

            case 1: // Load

                LoadGame();
                break;

            case 2: // Quit

                QuitGame();
                break;

            default:
                break;
        }
    }

    void ResumeGame()
    {
        // Position the slot image where the pause menu selected option text is
        if (darkRedPauseSelectionImage.rectTransform.anchoredPosition != new Vector2(0.0f, 50.0f))
            darkRedPauseSelectionImage.rectTransform.anchoredPosition = new Vector2(0.0f, 50.0f);

        // Once the player presses ENTER or RETURN, hide the pause game canvas to resume play
        if (Input.GetKeyDown(KeyCode.Return))
        {
            player.pauseGameCanvas.gameObject.SetActive(false);
        }
    }

    void LoadGame()
    {
        // Position the slot image where the pause menu selected option text is
        if (darkRedPauseSelectionImage.rectTransform.anchoredPosition != new Vector2(0.0f, 0.0f))
            darkRedPauseSelectionImage.rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);

        // Once the player presses ENTER or RETURN, let's make load game canvas visible
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Pause game should be invisible when going to the load screen
            player.pauseGameCanvas.gameObject.SetActive(false);

            player.loadGameCanvas.gameObject.SetActive(true);
        }
    }

    void QuitGame()
    {
        // Position the slot image where the pause menu selected option text is
        if (darkRedPauseSelectionImage.rectTransform.anchoredPosition != new Vector2(0.0f, -50.0f))
            darkRedPauseSelectionImage.rectTransform.anchoredPosition = new Vector2(0.0f, -50.0f);

        // Once the player presses ENTER or RETURN, quit playing in the editor for now
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Application.Quit();
            
            EditorApplication.isPlaying = false;
        }
    }
}
