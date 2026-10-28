using UnityEngine;

public interface ISaveable
{
    public object GetSaveData();
    public void LoadFromSaveData(object saveData, Player player);

    public SaveSystem.SaveType GetSaveType();

    public GameObject GetGameObject();
}
