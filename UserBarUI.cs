using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserBarUI : MonoBehaviour
{

    public TextMeshProUGUI moneyTextBox;
    public TextMeshProUGUI ticketPriceTextBox;

    private float money;
    private float animationMoney;
    private int rateOfMoneyAnimate;
    private int ticketPrice;

    private void Start()
    {
        animationMoney = 0;
        money = 0;
        ticketPrice = 0;
        rateOfMoneyAnimate = 100;
    }
    public void UpdateUI(User user)
    {
        money = user.GetMoney();
        ticketPriceTextBox.text = user.GetTicketPrice().ToString("C0");
        EvaluateMoneyDifferenceAnimation();
    }

    public void SetMoneyTextBox(string textBox)
    {
        moneyTextBox.text = textBox;
    }
    private void Update()
    {
        MoneyChange();
        moneyTextBox.text = animationMoney.ToString("C0");
    }
    private void MoneyChange()
    {
        
        if (animationMoney != money)
        {
            if (animationMoney < money)
            {
                animationMoney += rateOfMoneyAnimate;
            }
            else
            {
                animationMoney -= rateOfMoneyAnimate;

            }
        }
        if (animationMoney  <= money + rateOfMoneyAnimate && animationMoney  >= money - rateOfMoneyAnimate)
        {
            animationMoney = money;
        }
    }
    private void EvaluateMoneyDifferenceAnimation()
    {
        if (Mathf.Abs(animationMoney-money) > 100000)
        {
            rateOfMoneyAnimate = 10000;
        }
        else if (Mathf.Abs(animationMoney - money) > 10000)
        {
            rateOfMoneyAnimate = 1000;
        }
        else if (Mathf.Abs(animationMoney - money) > 1000)
        {
            rateOfMoneyAnimate = 100;
        }
    }
}
