using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Player player;

    private void Awake()
    {
        player.OnLostHealth += Player_OnLostHealth;
    }

    private void Player_OnLostHealth(object sender, System.EventArgs e)
    {
        Destroy(GetComponentInChildren<Image>().gameObject); // Destroy one of the health dots
    }
}
