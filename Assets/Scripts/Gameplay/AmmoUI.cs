using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private Ammo ammo;

    private TextMeshProUGUI ammoText;

    private void Awake()
    {
        ammoText = GetComponent<TextMeshProUGUI>();
        ammo.OnAmmoChanged += Ammo_OnAmmoChanged;
    }

    private void Ammo_OnAmmoChanged(object sender, System.EventArgs e)
    {
        ammoText.text = "Ammo: " + ammo.CurrentAmmo.ToString();
        if (ammo.CurrentAmmo <= 5)
        {
            ammoText.color = Color.red;

        }
        else if (ammo.CurrentAmmo <= 10)
        {
            ammoText.color = Color.yellow;
        }
        else
        {
            ammoText.color = Color.white;
        }
    }
}
