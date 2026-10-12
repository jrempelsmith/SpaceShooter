using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private Image healthDotTemplate;

    private void Awake()
    {
        enemy.OnLostHealth += Enemy_OnLostHealth;

        for (int i = 0; i < enemy.Health; i++)
        {
            Image healthDot = Instantiate(healthDotTemplate, transform);
            healthDot.gameObject.SetActive(true);
        }

        Destroy(healthDotTemplate.gameObject); // Destroy the template after instantiating enough health dots
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
