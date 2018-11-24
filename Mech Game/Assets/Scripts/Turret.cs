using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Turret : MonoBehaviour {
    GameObject player;
    public Transform gunEnd;
    public GameObject bullet;
    //Use this for initialization

    void Awake()
    {
    }


    //p Update is called once per frame
    void Update () {
        
        //Debug.Log("Player is at " + player.transform.position);

        //player = GameObject.FindWithTag("Player").transform;


        if (NetworkManager.singleton.IsClientConnected())
        {
            Debug.Log("Client is connected detected");
            if(GameObject.FindWithTag("Player") != null  && player == null)
            {

            player = GameObject.FindWithTag("Player");
            player.gameObject.tag = "Player";
                Debug.Log("Player found: " + player.name);

            }
        }
        if (player != null  && player.transform != null)
        transform.LookAt(player.transform);

    }
    void OnTriggerEnter(Collider other)
    {
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
