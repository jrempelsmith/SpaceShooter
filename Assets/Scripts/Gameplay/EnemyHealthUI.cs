using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private Image healthDotTemplate;

    private void Start()
    {
        enemy.OnLostHealth += Enemy_OnLostHealth;
        enemy.OnHealthUpdated += Enemy_OnHealthUpdated;

        for (int i = 0; i < enemy.Health; i++)
        {
            Image healthDot = Instantiate(healthDotTemplate, transform);
            healthDot.gameObject.SetActive(true);
        }

        healthDotTemplate.gameObject.SetActive(false); // Deactivate the template after instantiating enough health dots
    }

    private void Enemy_OnHealthUpdated(object sender, System.EventArgs e)
    {
        // Clear out all children, except the template
        foreach (Transform child in transform)
        {
            if (child != healthDotTemplate.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // Instantiate enough health dots for the correct health
        for (int i = 0; i < enemy.Health; i++)
        {
            Image healthDot = Instantiate(healthDotTemplate, transform);
            healthDot.gameObject.SetActive(true);
        }
    }

    private void Enemy_OnLostHealth(object sender, System.EventArgs e)
    {
        Destroy(GetComponentInChildren<Image>().gameObject); // Destroy one of the health dots
    }

    private void Update()
    {
        transform.parent.position = enemy.transform.position + new Vector3(0, 0, 2); // Keep the health UI above the enemy
        transform.parent.rotation = Quaternion.Euler(90, 90, 0); // Keep the health UI facing the camera
    }
}
