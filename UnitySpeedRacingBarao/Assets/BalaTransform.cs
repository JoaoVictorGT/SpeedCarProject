using UnityEngine;

public class BalaTransform : MonoBehaviour
{

    [SerializeField] private float speed = 100f;

    Rigidbody m_rigidbody;

    private void Start()
    {

        m_rigidbody = GetComponent<Rigidbody>();
        
    }

    void Update()
    {
        m_rigidbody.linearVelocity = transform.forward * speed;
        //m_rigidbody.AddfForce(BalaTransform.forward * speed);
    }
}
