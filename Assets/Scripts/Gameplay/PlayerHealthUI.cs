using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Image healthDotTemplate;

    private void Start()
    {
        player.OnLostHealth += Player_OnLostHealth;
        player.OnHealthUpdated += Player_OnHealthUpdated; ;

        for (int i = 0; i < player.Health; i++)
        {
            Image healthDot = Instantiate(healthDotTemplate, transform);
            healthDot.gameObject.SetActive(true);
        }

        healthDotTemplate.gameObject.SetActive(false); // Deactivate the template after instantiating enough health dots
    }

    private void Player_OnHealthUpdated(object sender, System.EventArgs e)
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
        for (int i = 0; i < player.Health; i++)
        {
            Image healthDot = Instantiate(healthDotTemplate, transform);
            healthDot.gameObject.SetActive(true);
        }
    }

    private void Player_OnLostHealth(object sender, System.EventArgs e)
    {
        Destroy(GetComponentInChildren<Image>().gameObject); // Destroy one of the health dots
    }

    private void Update()
    {
        transform.parent.position = player.transform.position + new Vector3(-1, 0, 2); // Keep the health UI above the player
        transform.parent.rotation = Quaternion.Euler(90, 90, 0); // Keep the health UI facing the camera
    }
}
