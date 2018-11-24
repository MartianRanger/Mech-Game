using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    private void Update()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 20;
        Destroy(gameObject, 5.0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Its a HIT!");
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if (health != null)
        {
            Debug.Log("Health LOSSSSSS");
            health.TakeDamage(10);
        }

        Destroy(gameObject);
    }
}
