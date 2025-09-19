using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayerFile(Player player, int saveNumber)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + $"/PlayerSave{saveNumber}.txt";

        FileStream fileStream = new FileStream(filePath, FileMode.Create);

        PlayerData playerData = new PlayerData(player);

        formatter.Serialize(fileStream, playerData);
        fileStream.Close();
    }

    public static PlayerData LoadPlayerFile(int saveNumber)
    {
        string filePath = Application.persistentDataPath + $"/PlayerSave{saveNumber}.txt";

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open);

            PlayerData playerData = formatter.Deserialize(fileStream) as PlayerData;
            fileStream.Close();

            return playerData;
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
