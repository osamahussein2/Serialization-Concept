using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows to save in a file
[Serializable]

public class PlayerData
{
    public float[] savedPosition;
    public float[] savedCameraPosition;

    public float savedPlayerHealth;

    public bool savedCarriedKnife;
    public bool savedCarriedPistol;

    public float[] savedKnifePosition;
    public float[] savedPistolPosition;

    public int savedWeaponIndex;

    // Take in the player data needed for saving and loading
    public PlayerData(Player player)
    {
        // Create new floating point arrays for player's and camera's saved positions
        savedPosition = new float[3];
        savedCameraPosition = new float[3];

        savedPlayerHealth = new float();

        savedCarriedKnife = new bool();
        savedCarriedPistol = new bool();

        savedKnifePosition = new float[3];
        savedPistolPosition = new float[3];

        savedWeaponIndex = new int();

        // Set saved position to the player's current position
        savedPosition[0] = player.transform.position.x;
        savedPosition[1] = player.transform.position.y;
        savedPosition[2] = player.transform.position.z;

        // Set saved camera position to the camera's current position
        savedCameraPosition[0] = player.mainCamera.transform.position.x;
        savedCameraPosition[1] = player.mainCamera.transform.position.y;
        savedCameraPosition[2] = player.mainCamera.transform.position.z;

        savedPlayerHealth = player.playerHealth;

        savedCarriedKnife = player.carryingKnife;
        savedCarriedPistol = player.carryingPistol;

        // Set saved knife position to the knife's current position
        savedKnifePosition[0] = player.knife.transform.position.x;
        savedKnifePosition[1] = player.knife.transform.position.y;
        savedKnifePosition[2] = player.knife.transform.position.z;

        // Set saved pistol position to the pistol's current position
        savedPistolPosition[0] = player.pistol.transform.position.x;
        savedPistolPosition[1] = player.pistol.transform.position.y;
        savedPistolPosition[2] = player.pistol.transform.position.z;

        // Set saved weapon index to the player's selected weapon index
        savedWeaponIndex = player.selectedWeapon;
    }
}
