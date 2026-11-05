
using System.Collections.Generic;

public class AlienSaveData
{
    public int saveID;

    public int connectedCometID;
    public List<int> connectedAsteroidIDs;

    public Vector3Data position;
    public Vector3Data targetPoint;
    public int health;
}
