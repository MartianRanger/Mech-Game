using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    Transform player;
    public Transform gunEnd;
    public GameObject bullet;
	// Use this for initialization
	void Awake()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);//search target right at beginning, at a rate of .5 seconds
        Debug.Log("Really?");
    }

    // Update is called once per frame
    void UpdateTarget () {
        /*if (player = null)
            return;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.LookAt(player.transform.position);*/
	}
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("PIZZA");
        if(other.gameObject.tag=="Player")
        {
            StartCoroutine("Shooting");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopCoroutine("Shooting");
        }
    }
    IEnumerator Shooting()
    {
        while (true)
        {
            Instantiate(bullet, gunEnd.position, gunEnd.rotation);
            yield return new WaitForSeconds(2);
        }
    }
}
