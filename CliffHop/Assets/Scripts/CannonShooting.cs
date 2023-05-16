using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShooting : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    [SerializeField] float bulletSpeed;

    private bool shoot;

    void Start()
    {
        shoot = false;
    }

    void Update()
    {
        if (shoot)
        {  
                var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                if (gameObject.CompareTag("CannonX"))
                    bullet.GetComponent<Rigidbody>().velocity = -bulletSpawnPoint.right * bulletSpeed;
                else if (gameObject.CompareTag("CannonZ"))
                    bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
                shoot = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shoot = true;
        }
    }
}
