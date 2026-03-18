using UnityEngine;
using UnityEngine.UI; // need for the image (health bar)
using TMPro;

//this script displays the players health on the ui
public class HealthUI : MonoBehaviour
{
    [Header("References")]
    public Health targetHealth;          // Reference to the player Health script
    public Image healthFillImage;        // the Fill image
    public TextMeshProUGUI healthText;   // the text that shows health number

    private void Start()
    {
        //if the health reference was not set manuallay try find the player automatically
        if (targetHealth == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                targetHealth = playerObj.GetComponent<Health>(); //if it finds the player this will get his health
            }
        }
    }

    private void Update()
    {
        if (targetHealth == null) return; //if we dont have the reference for the health the code stops

        
        float pct = (float)targetHealth.CurrentHealth / targetHealth.maxHealth;// convert Health to a value between 0 and 1
        pct = Mathf.Clamp01(pct); // make sure the value stays between 0 and 1

        if (healthFillImage != null)
            healthFillImage.fillAmount = pct;

        if (healthText != null)
            healthText.text = $"{targetHealth.CurrentHealth} / {targetHealth.maxHealth}";
    }
}
