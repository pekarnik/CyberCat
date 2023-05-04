using UnityEngine;

public class NpcHealthController : MonoBehaviour
{
    [SerializeField] int baseHealth;
    [SerializeField] int currentHealth;
    [SerializeField] int bonusHealth = 0;

    void Awake() {
        currentHealth = baseHealth;
    }

    public void AddBonusHealth(int bonusHealth) {
        this.bonusHealth = bonusHealth;
        currentHealth += bonusHealth;
    }

    public void DecreaseHealth(int damage = 1) {
        if (currentHealth > 0) {
            if (currentHealth - damage == 0) {
                GameObject.Destroy(gameObject);
            } else {
                currentHealth -= damage;
            }
        }
    }

    public void IncreaseHealth(int heal) {
        if (currentHealth < baseHealth - heal) {
            currentHealth += heal;
        } else if (currentHealth > baseHealth - heal) {
            currentHealth = baseHealth;
        }
    }


}
