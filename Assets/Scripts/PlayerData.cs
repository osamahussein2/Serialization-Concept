using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows to save in a file
[Serializable]

public class PlayerData
{
    public float[] savedPosition;

    // Take in the player data needed for saving and loading
    public PlayerData(Player player)
    {
        savedPosition = new float[3];

        savedPosition[0] = player.savedPosition.x;
        savedPosition[1] = player.savedPosition.y;
        savedPosition[2] = player.savedPosition.z;
    }
}
