using UnityEngine;

public interface ISaveable
{
    public int GetSaveID();
    public void SetSaveID(int id);

    public object GetSaveData();
    public void LoadFromSaveData(object saveData, Player player);
    public void RestoreReferencesAfterLoading(object saveData, ISaveable[] loadedISaveables);

    public SaveSystem.SaveType GetSaveType();

    public GameObject GetGameObject();
}
