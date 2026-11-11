using UnityEngine;

public class HealthDebugController : MonoBehaviour
{
    void Update()
    {
        // Press "-" -> Set John's health to 25%
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            GameObject john = GameObject.Find("John");
            if (john != null)
            {
                HealthSystem johnHealth = john.GetComponent<HealthSystem>();
                if (johnHealth != null)
                {
                    johnHealth.currentHealth = johnHealth.maxHealth * 0.25f;
                    Debug.Log("[DEBUG] John's health set to 25%");
                }
            }
        }

        // Press "=" -> Set Marco's health to 25%
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            GameObject marco = GameObject.Find("Marco");
            if (marco != null)
            {
                HealthSystem marcoHealth = marco.GetComponent<HealthSystem>();
                if (marcoHealth != null)
                {
                    marcoHealth.currentHealth = marcoHealth.maxHealth * 0.25f;
                    Debug.Log("[DEBUG] Marco's health set to 25%");
                }
            }
        }
    }
}
