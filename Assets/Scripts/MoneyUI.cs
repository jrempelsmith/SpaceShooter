using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private Money money;

    private TextMeshProUGUI moneyText;

    private void Awake()
    {
        moneyText = GetComponent<TextMeshProUGUI>();
        money.OnMoneyChanged += Money_OnMoneyChanged;
    }

    private void Money_OnMoneyChanged(object sender, System.EventArgs e)
    {
        moneyText.text = "Ammo: " + money.CurrentMoney.ToString();
    }
}
