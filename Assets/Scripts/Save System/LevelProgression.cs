using UnityEngine;

public class LevelProgression : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Load from the levelprogression save file
    }
}
