using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayerFile(Player player, int saveNumber)
    {
        // Initialize file path to save files
        string filePath = Application.persistentDataPath + $"/PlayerSave{saveNumber}.txt";

        // Initialize player data for saving player information
        PlayerData playerData = new PlayerData(player);

        // Create a new filestream that will create the save file at the file path defined above
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            // Create a writer that will write any saved data into a JSON file including its binary information
            using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
            {
                // Save player data
                string saveData = JsonUtility.ToJson(playerData);
                binaryWriter.Write(saveData);
            }
        }
    }

    public static PlayerData LoadPlayerFile(int saveNumber)
    {
        // Initialize file path for loading files
        string filePath = Application.persistentDataPath + $"/PlayerSave{saveNumber}.txt";

        // Make sure the file exists at the file path to load the binary file
        if (File.Exists(filePath))
        {
            // Create a new filestream that will open the save file at the file path defined above
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                // Create a writer that will load any saved data from the binary information using JSON writer
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    // Load player data
                    string loadBinary = binaryReader.ReadString();
                    PlayerData playerData = JsonUtility.FromJson<PlayerData>(loadBinary);

                    return playerData;
                }
            }
        }

        else
        {
            return null;
        }
    }

    public static bool DoesFileExist(int saveNumber)
    {
        string filePath = Application.persistentDataPath + $"/PlayerSave{saveNumber}.txt";

        // Simple check if file does exist
        if (File.Exists(filePath))
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
