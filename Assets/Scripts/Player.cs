using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Organize any game object related code here
    [Header("GameObjects")]
    public Camera mainCamera;
    [SerializeField] private Text keyPressInstructionText;
    [SerializeField] private Slider healthBar;

    // Organize any canvas related code here
    [Header("Canvases")]
    public Canvas saveGameCanvas;
    public Canvas loadGameCanvas;
    public Canvas pauseGameCanvas;

    // Organize any public variables that are hidden from inspector here
    [HideInInspector] public float playerHealth;

    // Organize any private (shown in inspector) player related parameters here
    [Header("Player Parameters")]
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private float playerSpeed;

    [Tooltip("This determines the speed in which the player's health drains or increases")]
    [SerializeField] private float playerHealthModifierSpeed;

    // Private parameters
    private bool onTypewriter = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize game object's active states
        keyPressInstructionText.gameObject.SetActive(false);
        saveGameCanvas.gameObject.SetActive(false);
        pauseGameCanvas.gameObject.SetActive(false);
        loadGameCanvas.gameObject.SetActive(false);

        // Initialize player position
        transform.position = initialPosition;

        // Just set the player health to the health bar's max value
        playerHealth = healthBar.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        // Call pause game toggle
        ToggleGamePaused();

        // The player should only move whenever the save game canvas, load game canvas and pause game canvas ARE NOT active
        if (!saveGameCanvas.gameObject.activeInHierarchy && !loadGameCanvas.gameObject.activeInHierarchy && 
            !pauseGameCanvas.gameObject.activeInHierarchy)
        {
            HandleMovement();
        }

        // Call the typewriter save logic
        ToggleTypewriterSave();
        TypewriterVisibility();

        // Allow the player to modify their player by holding key to test if player's health will be saved and loaded
        ModifyPlayerHealth();
    }

    void ToggleGamePaused()
    {
        // Check if player presses ESCAPE when the save game canvas, load game canvas and the pause game canvas are all NOT visible
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseGameCanvas.gameObject.activeInHierarchy && 
            !saveGameCanvas.gameObject.activeInHierarchy && !loadGameCanvas.gameObject.activeInHierarchy)
        {
            // Activate the pause game canvas for pausing the game
            pauseGameCanvas.gameObject.SetActive(true);
        }
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
        // If the key press instruction IS active, space key is pressed, the save game canvas and pause game canvas ARE NOT active
        if (keyPressInstructionText.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Space) &&
            !saveGameCanvas.gameObject.activeInHierarchy && !loadGameCanvas.gameObject.activeInHierarchy && 
            !pauseGameCanvas.gameObject.activeInHierarchy)
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

    void ModifyPlayerHealth()
    {
        // Decrement player health only if the player's health is greater than 0
        if (Input.GetKey(KeyCode.K) && playerHealth > 0.0f)
        {
            playerHealth -= playerHealthModifierSpeed * Time.deltaTime;
        }

        // Increment player health only if the player's health is less than the health bar's max health
        if (Input.GetKey(KeyCode.L) && playerHealth < healthBar.maxValue)
        {
            playerHealth += playerHealthModifierSpeed * Time.deltaTime;
        }

        // Set the health bar value to the player's health value only if it's not set already
        if (healthBar.value != playerHealth) healthBar.value = playerHealth;
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

        // Load player's position from save file
        transform.position = new Vector3(playerData.savedPosition[0], playerData.savedPosition[1],
            playerData.savedPosition[2]);

        // Load the camera's position from save file
        mainCamera.transform.position = new Vector3(playerData.savedCameraPosition[0], 
            playerData.savedCameraPosition[1], playerData.savedCameraPosition[2]);

        // Load the player's health from save file
        playerHealth = playerData.savedPlayerHealth;
    }

    public bool CheckIfFileExists(int saveNumber)
    {
        // Make sure the save file number is valid to return true
        if (SaveSystem.DoesFileExist(saveNumber))
        {
            return true;
        }

        // Otherwise, save file isn't valid
        else
        {
            return false;
        }
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
