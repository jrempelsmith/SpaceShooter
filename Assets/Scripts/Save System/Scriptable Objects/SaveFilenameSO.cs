using UnityEngine;

[CreateAssetMenu(fileName = "SaveFilenameSO", menuName = "Scriptable Objects/SaveFilenameSO")]
public class SaveFilenameSO : ScriptableObject
{
    public string saveFileName;
    public string saveMetadataFileName;
    public string levelProgressFileName;
    public string saveGameFileExtension;
}
