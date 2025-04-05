using System;
using UnityEngine;

public class BalaTransform : MonoBehaviour
{
    //[SerializeField] private float speed = 100f;

    //Rigidbody m_Rigidbody;

    public GameObject bulletPrefab; // prefab Bala
    public Transform firePoint; // ponto de disparo
    public float speed = 100f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //m_Rigidbody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){
            Shoot();
        }
        //m_Rigidbody.linearVelocity = transform.forward * speed;
        //m_Rigidbody.AddForce(transform.forward * speed);

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
