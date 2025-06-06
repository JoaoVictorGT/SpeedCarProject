using System;
using UnityEngine;

public class BalaTransform : MonoBehaviour
{


    public GameObject bulletPrefab; // prefab Bala
    public Transform firePoint; // ponto de disparo
    public float speed = 100f;
    

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){
            Shoot();
        }
        
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet. GetComponent<Rigidbody>();

        if (rb != null){
            rb.linearVelocity = firePoint.forward * speed;
        }
    }
}
