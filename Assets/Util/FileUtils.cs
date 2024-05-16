using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileUtils
{
    private readonly string factionDirectoryPath = Application.persistentDataPath + "/FactionCustomizer";
    private readonly string factionJsonPath = Application.persistentDataPath + "/FactionCustomizer/CurrentFaction.json";
    private readonly string villageDirectoryPath = Application.persistentDataPath + "/Village";

    public CustomFactionObject LoadFactionFromFile()
    {
        CustomFactionObject loadedFaction;

        if (!Directory.Exists(factionDirectoryPath))
        {
            Directory.CreateDirectory(factionDirectoryPath);
        }

        if (!File.Exists(factionJsonPath))
        {
            string streamingJsonPath = Application.streamingAssetsPath + "/FactionCustomizer/DefaultFaction.json";
            File.Copy(streamingJsonPath, factionJsonPath, true);
        }

        Debug.Log("Loading Faction from json path:" + factionJsonPath);

        StreamReader reader = new(factionJsonPath);
        string configJson = reader.ReadToEnd();
        reader.Close();

        loadedFaction = JsonUtility.FromJson<CustomFactionObject>(configJson);

        return loadedFaction;
    }

    public void SaveFactionToFile(CustomFactionObject faction)
    {
        StreamWriter writer = new StreamWriter(factionJsonPath);

        string factionJson = JsonUtility.ToJson(faction, true);

        writer.Write(factionJson);
        writer.Flush();
        writer.Close();
    }

    public void SaveVillageToFile(VillageDataObject village, string fileName)
    {
        string villageFilePath = villageDirectoryPath +"/"+ fileName;

        if (!Directory.Exists(villageDirectoryPath))
        {
            Directory.CreateDirectory(villageDirectoryPath);
        }

        string json = JsonUtility.ToJson(village, true);

        File.WriteAllText(villageFilePath, json);
    }

    public VillageDataObject LoadVillageFromFile(string fileName)
    {
        VillageDataObject loadedVillage = new();

        string path = villageDirectoryPath + "/" + fileName;

        StreamReader reader = new(path);
        string configJson = reader.ReadToEnd();
        reader.Close();

        loadedVillage = JsonUtility.FromJson<VillageDataObject>(configJson);

        return loadedVillage;
    }

    public Sprite LoadSpriteFromFile(string fileName)
    {
        Texture2D SpriteTexture = LoadTexture(fileName);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), 100);
        return NewSprite;
    }

    public Texture2D LoadTexture(string fileName)
    {
        string filePath = Application.persistentDataPath + "/FactionCustomizer/" + fileName;

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(filePath))
        {
            FileData = File.ReadAllBytes(filePath);
            Tex2D = new Texture2D(2, 2);
            if (Tex2D.LoadImage(FileData))
            {
                return Tex2D;
            }
        }
        return null;
    }
}
