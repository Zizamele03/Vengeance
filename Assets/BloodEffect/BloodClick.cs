using UnityEngine;

public class BloodClick : MonoBehaviour
{
    public GameObject bloodPrefab; // assign a particle prefab or decal prefab
    public float bloodLifetime = 1f; // optional, how long blood lasts

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Instantiate blood at the hit point
                GameObject blood = Instantiate(bloodPrefab, hit.point, Quaternion.LookRotation(hit.normal));

                // Optional: destroy blood after some time
                Destroy(blood, bloodLifetime);
            }
        }
    }
}
