
public class LevelProgressContainer
{
    public bool[] levelCompleteStatus;

    // levels are 1, 2, or 3
    public void SetLevelComplete(int levelNumber)
    {
        levelCompleteStatus[levelNumber - 1] = true;
    }
}
