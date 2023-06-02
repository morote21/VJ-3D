using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnimation : MonoBehaviour
{

    [SerializeField] int rotationSpeed;
    private float alphaReduction;
    private bool picked;
    // Start is called before the first frame update
    void Start()
    {
        alphaReduction = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);
        if (picked)
        {
            transform.Translate(0, 4 * Time.deltaTime, 0, Space.World);
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                // Get all the materials used by the Renderer
                Material[] materials = renderer.materials;
                
                // Iterate through each material
                for (int i = 0; i < materials.Length; i++)
                {
                    // Assign the new color to the material
                    float newAlpha = materials[i].color.a - (alphaReduction * Time.deltaTime);
                    materials[i].color = new Color(materials[i].color.r, materials[i].color.g, materials[i].color.b, newAlpha);
                }
                if (materials[0].color.a <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void pickupAnimation()
    {
        rotationSpeed *= 9;
        picked = true;
    }

    public bool getPicked()
    {
        return picked;
    }
}
