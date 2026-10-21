using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveSystem : MonoBehaviour
{
    
    void Start()
    {
        if (TryLoad())
        {
            Debug.Log("Load succeeded!");
        }
        else
        {
            Debug.Log("Load failed.");
        }

        

        //string contents = "My fav story is ...";
        //File.WriteAllText("C:\\Users\\jrempelsmith\\Documents\\Save.txt", contents);
    }

    public bool TryLoad()
    {
        //string fileContents;

        //try
        //{
        //    fileContents = File.ReadAllText("C:\\Users\\jrempelsmith\\Documents\\GameSave.txt");
        //}
        //catch
        //{
        //    Debug.Log("File not found!");
        //    return false;
        //}

        //Debug.Log(fileContents);
        //return true;

        string fileContents;

        string path = Path.Combine(Application.persistentDataPath, "GameSave.txt");
        Debug.Log(path);


        if (!File.Exists(path))
        {
            return false;
        }

        fileContents = File.ReadAllText(path);
        
        Debug.Log(fileContents);
        return true;
    }
}
