using UnityEngine;

public class Comet : MonoBehaviour, ISaveable
{
    private float orbitalSpeed;

    private float horizontalRadius;
    private float verticalRadius;

    private float currentAngle;
    private int directionMultiplier;

    private float perlinNoiseXCoord_1;
    private float perlinNoiseXCoord_2;
    [SerializeField] private float perlinFrequencyMultiplier;
    [SerializeField] private float perlinAmplitudeMultiplier;

    private void Awake()
    {
        perlinNoiseXCoord_1 = Random.Range(0f, 1000f);
        perlinNoiseXCoord_2 = Random.Range(0f, 1000f);
        currentAngle = Random.Range(0f, 360f);
        directionMultiplier = Random.Range(0, 2) == 0 ? 1 : -1;
    }

    private void Update()
    {
        // Calculations assume the comet is orbiting (0,0,0)

        currentAngle += orbitalSpeed * Mathf.Deg2Rad * Time.deltaTime * directionMultiplier;

        float xCoord = horizontalRadius * Mathf.Cos(currentAngle);
        float zCoord = verticalRadius * Mathf.Sin(currentAngle);

        float perlinValue1 = Mathf.PerlinNoise(perlinNoiseXCoord_1, Time.time * perlinFrequencyMultiplier);
        float perlinValue2 = Mathf.PerlinNoise(perlinNoiseXCoord_2, Time.time * perlinFrequencyMultiplier);

        float xAdjustment = (perlinValue1 - 0.5f) * perlinAmplitudeMultiplier;
        float zAdjustment = (perlinValue2 - 0.5f) * perlinAmplitudeMultiplier;

        xCoord += xAdjustment;
        zCoord += zAdjustment;

        transform.position = new Vector3(xCoord, transform.position.y, zCoord);
        transform.forward = Vector3.Cross(transform.position, Vector3.up * directionMultiplier);
    }

    public void RunSetup(float orbitalSpeed, float horizontalRadius, float verticalRadius)
    {
        this.orbitalSpeed = orbitalSpeed;
        this.horizontalRadius = horizontalRadius;
        this.verticalRadius = verticalRadius;

        transform.position = new Vector3(horizontalRadius * Mathf.Cos(currentAngle), 0, verticalRadius * Mathf.Sin(currentAngle));
        transform.forward = Vector3.Cross(transform.position, Vector3.up * directionMultiplier);
    }

    public object GetSaveData()
    {
        CometSaveData cometSaveData = new CometSaveData
        {
            currentAngle = currentAngle,
            horizontalRadius = horizontalRadius,
            verticalRadius = verticalRadius,
            directionMultiplier = directionMultiplier,
            perlinNoiseXCoord_1 = perlinNoiseXCoord_1,
            perlinNoiseXCoord_2 = perlinNoiseXCoord_2
        };

        return cometSaveData;
    }

    public void LoadFromSaveData(object saveData, Player player)
    {
        CometSaveData cometSaveData = (CometSaveData)saveData;

        currentAngle = cometSaveData.currentAngle;
        horizontalRadius = cometSaveData.horizontalRadius;
        verticalRadius = cometSaveData.verticalRadius;
        directionMultiplier = cometSaveData.directionMultiplier;
        perlinNoiseXCoord_1 = cometSaveData.perlinNoiseXCoord_1;
        perlinNoiseXCoord_2 = cometSaveData.perlinNoiseXCoord_2;
    }

    SaveSystem.SaveType ISaveable.GetSaveType()
    {
        return SaveSystem.SaveType.Comet;
    }

    GameObject ISaveable.GetGameObject()
    {
        return gameObject;
    }
}
