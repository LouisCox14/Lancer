using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Entity targetEntity;

    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private Image healthBar;

    void Start()
    {
        UpdateUI(targetEntity.maxHp, targetEntity.hp);
    }
    
    void OnEnable()
    {
        targetEntity.onDamageTaken += OnDamageTaken;
        targetEntity.onHealthGained += OnHealthGained;

        UpdateUI(targetEntity.maxHp, targetEntity.hp);
    }

    void OnDisable()
    {
        targetEntity.onDamageTaken -= OnDamageTaken;
    }

    void OnDamageTaken(int maxHP, int newHP, Damage damageTaken)
    {
        UpdateUI(maxHP, newHP);
    }

    void OnHealthGained(int maxHP, int newHP, int healthGained)
    {
        UpdateUI(maxHP, newHP);
    }

    void UpdateUI(int maxHP, int newHP)
    {
        healthBar.fillAmount = (float)newHP / (float)maxHP;
        textComponent.text = newHP.ToString() + "/" + maxHP.ToString();
    }
}
