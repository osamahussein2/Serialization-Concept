using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Organize any game object related code here
    [Header("GameObjects")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Text keyPressInstructionText;
    public Canvas saveGameCanvas;

    // Organize any private (shown in inspector) player related parameters here
    [Header("Player Parameters")]
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private float playerSpeed;

    // Organize any public variables here
    public Vector3 savedPosition;

    // Private parameters
    private bool onTypewriter = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize game object's active states
        keyPressInstructionText.gameObject.SetActive(false);
        saveGameCanvas.gameObject.SetActive(false);

        // Initialize player position
        transform.position = initialPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // The player should only move whenever the save game canvas isn't active yet
        if (!saveGameCanvas.gameObject.activeInHierarchy)
        {
            HandleMovement();
        }

        // Call the typewriter save logic
        ToggleTypewriterSave();
        TypewriterVisibility();
    }

    void HandleMovement()
    {
        // Let's move the player around for testing
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Also move the main camera with the player as well
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);

        transform.Translate(new Vector3(horizontal * playerSpeed * Time.deltaTime,
            vertical * playerSpeed * Time.deltaTime, 0.0f));
    }

    void ToggleTypewriterSave()
    {
        // If the key press instruction IS active, space key is pressed and the save game canvas ISN'T active
        if (keyPressInstructionText.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Space) &&
            !saveGameCanvas.gameObject.activeInHierarchy)
        {
            // Go ahead and deactivate key press instruction, but do activate the save game canvas for saving
            keyPressInstructionText.gameObject.SetActive(false);
            saveGameCanvas.gameObject.SetActive(true);
        }
    }

    void TypewriterVisibility()
    {
        /* Tell the player to press SPACE by activating the key press instruction only if save game canvas ISN'T
        active (while player is on the typewriter) */

        // The key press instruction active in hierarchy code prevents this block of code from executing every frame
        if (onTypewriter && !saveGameCanvas.gameObject.activeInHierarchy && 
            !keyPressInstructionText.gameObject.activeInHierarchy)
        {
            keyPressInstructionText.gameObject.SetActive(true);
        }

        // Hide the key press instruction if the save game canvas is active (while player is on the typewriter)
        else if (onTypewriter && saveGameCanvas.gameObject.activeInHierarchy &&
            keyPressInstructionText.gameObject.activeInHierarchy)
        {
            keyPressInstructionText.gameObject.SetActive(false);
        }

        // Disable the key press instruction after the player is no longer on the typewriter
        else if (!onTypewriter && keyPressInstructionText.gameObject.activeInHierarchy)
        {
            keyPressInstructionText.gameObject.SetActive(false);
        }
    }

    // Player Save function
    public void PlayerSave(int saveNumber)
    {
        SaveSystem.SavePlayerFile(this, saveNumber);
    }

    // Player Load function
    public void PlayerLoad(int saveNumber)
    {
        PlayerData playerData = SaveSystem.LoadPlayerFile(saveNumber);

        savedPosition.x = playerData.savedPosition[0];
        savedPosition.y = playerData.savedPosition[1];
        savedPosition.z = playerData.savedPosition[2];

        transform.position = savedPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Set on typewriter to true so that the key press instruction will be visible
        if (collision.gameObject.name == "Typewriter")
        {
            onTypewriter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Set on typewriter to false so that the key press instruction will be invisible
        if (collision.gameObject.name == "Typewriter")
        {
            onTypewriter = false;
        }
    }
}
