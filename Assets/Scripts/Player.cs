using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Organize any game object related code here
    [Header("GameObjects")]
    public Camera mainCamera;
    [SerializeField] private Text keyPressInstructionText;
    [SerializeField] private Slider healthBar;

    // Weapon objects
    public List<GameObject> knives;
    public List<GameObject> pistols;

    // Organize any text object related code here
    [Header("Text Objects")]
    [SerializeField] private List<Text> knifeKeyPressInstructionTexts;
    [SerializeField] private List<Text> pistolKeyPressInstructionTexts;

    // Text object allowing the player to type anything in the inspector
    public Text typeAnythingInGame;

    // Organize any image/sprite object related code here
    [Header("Image/Sprite Objects")]
    [SerializeField] private Image currentWeaponImage;
    public List<Sprite> weaponSprite;

    // Gives the player ability to change the color of the triangle
    public SpriteRenderer triangle;

    // Any sprites we want to put in the inspector for weapons
    [SerializeField] private Sprite knifeSprite;
    [SerializeField] private Sprite pistolSprite;

    // Organize any canvas related code here
    [Header("Canvases")]
    public Canvas saveGameCanvas;
    public Canvas loadGameCanvas;
    public Canvas pauseGameCanvas;

    // Organize any public variables that are hidden from inspector here
    [HideInInspector] public float playerHealth;
    [HideInInspector] public bool carryingKnife = false;
    [HideInInspector] public bool carryingPistol = false;
    [HideInInspector] public int selectedWeapon = 0;
    [HideInInspector] public int knifeNumber = 0;
    [HideInInspector] public int pistolNumber = 0;

    // Organize any private (shown in inspector) player related parameters here
    [Header("Player Parameters")]
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private float playerSpeed;

    [Tooltip("This determines the speed in which the player's health drains or increases")]
    [SerializeField] private float playerHealthModifierSpeed;

    // Private parameters
    private bool onTypewriter = false;
    private bool onKnife = false;
    private bool onPistol = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize game object's active states
        keyPressInstructionText.gameObject.SetActive(false);
        saveGameCanvas.gameObject.SetActive(false);
        pauseGameCanvas.gameObject.SetActive(false);
        loadGameCanvas.gameObject.SetActive(false);

        foreach (Text knifeKeyText in knifeKeyPressInstructionTexts) knifeKeyText.gameObject.SetActive(false);
        foreach (Text pistolKeyText in pistolKeyPressInstructionTexts) pistolKeyText.gameObject.SetActive(false);

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

        // Get input to change triangle color
        ChangeTriangleColor();

        // Call the typewriter save logic
        ToggleTypewriterSave();
        TypewriterVisibility();

        // Allow the player to modify their player by holding key to test if player's health will be saved and loaded
        ModifyPlayerHealth();

        // Update knife
        PickOrDropKnife();
        HandleKnife();
        KnifeVisibility();

        // Update pistol
        PickOrDropPistol();
        HandlePistol();
        PistolVisibility();

        // Handle weapons
        HandleWeapon();
        SwitchCurrentlyHeldWeapon();
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

    void ChangeTriangleColor()
    {
        // Press T to change triangle's color only when the save game canvas, load game canvas and the pause game canvas are all NOT visible
        if (Input.GetKeyDown(KeyCode.T) && !pauseGameCanvas.gameObject.activeInHierarchy &&
            !saveGameCanvas.gameObject.activeInHierarchy && !loadGameCanvas.gameObject.activeInHierarchy)
        {
            // Create local variables exclusively inside this block to change the color of the triangle
            float randomRed = Random.Range(0.0f, 1.0f), 
                randomGreen = Random.Range(0.0f, 1.0f), 
                randomBlue = Random.Range(0.0f, 1.0f);

            triangle.color = new Color(randomRed, randomGreen, randomBlue);
        }
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

    void PickOrDropKnife()
    {
        // Pick up the knife if player is on knife (but not on pistol) and hasn't picked it up yet
        if (Input.GetKeyDown(KeyCode.G) && !carryingKnife && onKnife && !onPistol)
        {
            weaponSprite.Add(knifeSprite);
            selectedWeapon = 1;

            carryingKnife = true;
        }

        // Otherwise, drop the knife when the player has it and decides to put it down
        else if (Input.GetKeyDown(KeyCode.G) && carryingKnife && !onPistol && selectedWeapon == 1)
        {
            weaponSprite.Remove(knifeSprite);
            selectedWeapon = 0;

            carryingKnife = false;
        }
    }

    void HandleKnife()
    {
        // If the player isn't carrying a knife, set the knife down
        if (!carryingKnife)
        {
            if (knives[knifeNumber].transform.parent != null) knives[knifeNumber].transform.SetParent(null);
        }

        // If the player is carrying a pistol, set it to move with the player
        else
        {
            if (knives[knifeNumber].transform.parent != transform)
                knives[knifeNumber].transform.SetParent(transform);
        }
    }

    void KnifeVisibility()
    {
        // Enable the knife key press instruction after the player is ON the knife
        if (onKnife && !knifeKeyPressInstructionTexts[knifeNumber].gameObject.activeInHierarchy && !carryingKnife)
        {
            knifeKeyPressInstructionTexts[knifeNumber].gameObject.SetActive(true);
        }

        // Disable the knife key press instruction after the player is no longer on the knife
        else if (!onKnife && knifeKeyPressInstructionTexts[knifeNumber].gameObject.activeInHierarchy &&
            !carryingKnife || carryingKnife)
        {
            knifeKeyPressInstructionTexts[knifeNumber].gameObject.SetActive(false);
        }
    }

    void PickOrDropPistol()
    {
        // Pick up the pistol if player is on pistol (but not on knife) and hasn't picked it up yet
        if (Input.GetKeyDown(KeyCode.G) && !carryingPistol && onPistol && !onKnife)
        {
            weaponSprite.Add(pistolSprite);
            selectedWeapon = 1;

            carryingPistol = true;
        }

        // Otherwise, drop the pistol when the player has it and decides to put it down
        else if (Input.GetKeyDown(KeyCode.G) && carryingPistol && !onKnife && selectedWeapon == 1)
        {
            weaponSprite.Remove(pistolSprite);
            selectedWeapon = 0;

            carryingPistol = false;
        }
    }

    void HandlePistol()
    {
        // If the player isn't carrying a pistol, set the pistol down
        if (!carryingPistol)
        {
            if (pistols[pistolNumber].transform.parent != null) pistols[pistolNumber].transform.SetParent(null);
        }

        // If the player is carrying a pistol, set it to move with the player
        else
        {
            if (pistols[pistolNumber].transform.parent != transform) 
                pistols[pistolNumber].transform.SetParent(transform);
        }
    }

    void PistolVisibility()
    {
        // Enable the pistol key press instruction after the player is ON the pistol
        if (onPistol && !pistolKeyPressInstructionTexts[pistolNumber].gameObject.activeInHierarchy &&
            !carryingPistol)
        {
            pistolKeyPressInstructionTexts[pistolNumber].gameObject.SetActive(true);
        }

        // Disable the pistol key press instruction after the player is no longer on the pistol
        else if (!onPistol && pistolKeyPressInstructionTexts[pistolNumber].gameObject.activeInHierarchy &&
            !carryingPistol || carryingPistol)
        {
            pistolKeyPressInstructionTexts[pistolNumber].gameObject.SetActive(false);
        }
    }

    void HandleWeapon()
    {
        // Set the image UI sprite to the weapon sprite at the selected weapon index
        if (currentWeaponImage.sprite != weaponSprite[selectedWeapon])
            currentWeaponImage.sprite = weaponSprite[selectedWeapon];

        switch (selectedWeapon)
        {
            case 0:

                // Disable current knife and pistol gameobjects if their carried booleans are true
                if (carryingKnife && knives[knifeNumber].activeInHierarchy) knives[knifeNumber].SetActive(false);
                if (carryingPistol && pistols[pistolNumber].activeInHierarchy) pistols[pistolNumber].SetActive(false);

                break;

            case 1:

                // Enable current knife and pistol gameobjects if their carried booleans are true
                if (carryingKnife && !knives[knifeNumber].activeInHierarchy) knives[knifeNumber].SetActive(true);
                if (carryingPistol && !pistols[pistolNumber].activeInHierarchy) pistols[pistolNumber].SetActive(true);

                break;
        }
    }

    void SwitchCurrentlyHeldWeapon()
    {
        // Get the last weapon index
        int finalWeaponIndex = weaponSprite.Count - 1;

        // Switch to previous weapon (only if the weapon sprite count is more than 1)
        if (Input.GetKeyDown(KeyCode.O) && selectedWeapon > 0 && weaponSprite.Count > 1)
        {
            selectedWeapon -= 1;
        }

        // Switch to next weapon (only if the weapon sprite count is more than 1)
        if (Input.GetKeyDown(KeyCode.P) && selectedWeapon >= 0 && selectedWeapon < finalWeaponIndex && 
            weaponSprite.Count > 1)
        {
            selectedWeapon += 1;
        }
    }

    void LoadKnifeSprite()
    {
        /* Only check if player is carrying knife to change all weapon object's parents depending on whatever the 
        object is originally parented to before loading */
        if (carryingKnife)
        {
            switch (knifeNumber)
            {
                case 0:

                    // Detach the other knife number from the player
                    if (knives[1].transform.parent != null) knives[1].transform.SetParent(null);

                    // Only attach the knife number we need to the player
                    if (knives[knifeNumber].transform.parent != transform)
                        knives[knifeNumber].transform.SetParent(transform);

                    // Only detach the pistol from the player if the loaded state has player carrying knife
                    foreach (GameObject pistol in pistols)
                        if (pistol.transform.transform.parent != null) pistol.transform.SetParent(null);

                    break;

                case 1:

                    // Detach the other knife number from the player
                    if (knives[0].transform.parent != null) knives[0].transform.SetParent(null);

                    // Only attach the knife number we need to the player
                    if (knives[knifeNumber].transform.parent != transform)
                        knives[knifeNumber].transform.SetParent(transform);

                    // Only detach the pistol from the player if the loaded state has player carrying knife
                    foreach (GameObject pistol in pistols)
                        if (pistol.transform.transform.parent != null) pistol.transform.SetParent(null);

                    break;
            }
        }

        // If the player carryKnife saved value is true but the weapon sprite count is 1 or below
        if (carryingKnife && weaponSprite.Count <= 1)
        {
            // Add the knife sprite in the weapon sprite list
            weaponSprite.Add(knifeSprite);
        }

        // aOtherwise, if the player carryKnife saved value is false but the weapon sprite count is greater than 1
        else if (!carryingKnife && weaponSprite.Count > 1)
        {
            // Remove the knife sprite from the weapon sprite list
            weaponSprite.Remove(knifeSprite);
        }
    }

    void LoadPistolSprite()
    {
        /* Only check if player is carrying pistol to change all weapon object's parents depending on whatever the 
        object is originally parented to before loading */
        if (carryingPistol)
        {
            switch (pistolNumber)
            {
                case 0:

                    // Detach the other pistol number from the player
                    if (pistols[1].transform.parent != null) pistols[1].transform.SetParent(null);

                    // Only attach the pistol number we need to the player
                    if (pistols[pistolNumber].transform.parent != transform)
                        pistols[pistolNumber].transform.SetParent(transform);

                    // Only detach the knife from the player if the loaded state has player carrying pistol
                    foreach (GameObject knife in knives)
                        if (knife.transform.transform.parent != null) knife.transform.SetParent(null);

                    break;

                case 1:

                    // Detach the other pistol number from the player
                    if (pistols[0].transform.parent != null) pistols[0].transform.SetParent(null);

                    // Only attach the pistol number we need to the player
                    if (pistols[pistolNumber].transform.parent != transform)
                        pistols[pistolNumber].transform.SetParent(transform);

                    // Only detach the knife from the player if the loaded state has player carrying pistol
                    foreach (GameObject knife in knives)
                        if (knife.transform.transform.parent != null) knife.transform.SetParent(null);

                    break;
            }
        }

        // If the player carryKnife saved value is true but the weapon sprite count is 1 or below
        if (carryingPistol && weaponSprite.Count <= 1)
        {
            // Add the knife sprite in the weapon sprite list
            weaponSprite.Add(pistolSprite);
        }

        // Otherwise, if the player carryKnife saved value is false but the weapon sprite count is greater than 1
        else if (!carryingPistol && weaponSprite.Count > 1)
        {
            // Remove the knife sprite from the weapon sprite list
            weaponSprite.Remove(pistolSprite);
        }

        /* This needs to be checked again just in case if the player is trying to load a state where they have the 
        knife, but before loading the game, the player had the pistol with them */

        // If the player carryKnife saved value is true but the weapon sprite count is 1 or below
        if (carryingKnife && weaponSprite.Count <= 1)
        {
            // Add the knife sprite in the weapon sprite list
            weaponSprite.Add(knifeSprite);
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

        // Load player's position from save file
        transform.position = new Vector3(playerData.savedPosition[0], playerData.savedPosition[1],
            playerData.savedPosition[2]);

        // Load the camera's position from save file
        mainCamera.transform.position = new Vector3(playerData.savedCameraPosition[0], 
            playerData.savedCameraPosition[1], playerData.savedCameraPosition[2]);

        // Load the player's health from save file
        playerHealth = playerData.savedPlayerHealth;

        // Load the last carrying knife boolean value from save file
        carryingKnife = playerData.savedCarriedKnife;
        carryingPistol = playerData.savedCarriedPistol;

        // Make sure to load the correct knife and pistol numbers to assign them as a child of the player
        knifeNumber = playerData.knifeNumber;
        pistolNumber = playerData.pistolNumber;

        // Load the 1st knife last position from save file
        knives[0].transform.position = new Vector3(playerData.savedKnifePosition[0],
            playerData.savedKnifePosition[1], playerData.savedKnifePosition[2]);

        // Load the 2nd knife last position from save file
        knives[1].transform.position = new Vector3(playerData.savedKnifePosition[3],
            playerData.savedKnifePosition[4], playerData.savedKnifePosition[5]);

        // Load the 1st pistol last position from save file
        pistols[0].transform.position = new Vector3(playerData.savedPistolPosition[0],
            playerData.savedPistolPosition[1], playerData.savedPistolPosition[2]);

        // Load the 2nd pistol last position from save file
        pistols[1].transform.position = new Vector3(playerData.savedPistolPosition[3],
            playerData.savedPistolPosition[4], playerData.savedPistolPosition[5]);

        // Load the player's selected weapon value
        selectedWeapon = playerData.savedWeaponIndex;

        LoadKnifeSprite();
        LoadPistolSprite();

        // Load the triangle's color from save file
        triangle.color = new Color(playerData.savedTriangleColor[0], playerData.savedTriangleColor[1],
            playerData.savedTriangleColor[2]);

        // Load the saved typed text in-game from the save file
        typeAnythingInGame.text = playerData.savedTypedTextInGame;
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

        // For every knife the player is inside of, set it to the correct knife number to show the key press instruction
        for (int i = 0; i < knives.Count; i++)
        {
            if (collision.gameObject.name == knives[i].name && !carryingKnife)
            {
                knifeNumber = i;
                onKnife = true;
            }
        }

        // For every pistol the player is inside of, set it to the correct pistol number to show the key press instruction
        for (int i = 0; i < pistols.Count; i++)
        {
            if (collision.gameObject.name == pistols[i].name && !carryingPistol)
            {
                pistolNumber = i;
                onPistol = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Set on typewriter to false so that the key press instruction will be invisible
        if (collision.gameObject.name == "Typewriter")
        {
            onTypewriter = false;
        }

        // Without this if statement, Unity will print an error in console after quitting game
        if (!knives[0].IsDestroyed() && !knives[1].IsDestroyed())
        {
            /* For every knife the player is outside of, set it to the correct knife number to hide the key press
            instruction */
            for (int i = 0; i < knives.Count; i++)
            {
                if (collision.gameObject.name == knives[i].name && !carryingKnife)
                {
                    knifeNumber = i;
                    onKnife = false;
                }
            }
        }

        // Without this if statement, Unity will print an error in console after quitting game
        if (!pistols[0].IsDestroyed() && !pistols[1].IsDestroyed())
        {
            /* For every pistol the player is outside of, set it to the correct pistol number to hide the key press
            instruction */
            for (int i = 0; i < pistols.Count; i++)
            {
                if (collision.gameObject.name == pistols[i].name && !carryingPistol)
                {
                    pistolNumber = i;
                    onPistol = false;
                }
            }
        }
    }
}
