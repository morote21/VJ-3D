using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplashScript : MonoBehaviour
{

    public ParticleSystem waterSplash;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            waterSplash.transform.position = new Vector3(other.transform.position.x, other.transform.position.y - 1, other.transform.position.z);
            waterSplash.Play();
        }
    }
}
