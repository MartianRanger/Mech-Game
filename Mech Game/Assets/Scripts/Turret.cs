using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Turret : MonoBehaviour {
    GameObject player;
    public Transform gunEnd;
    public GameObject turretMissile;


    //p Update is called once per frame
    void Update () {
        
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

    //[Command]
    IEnumerator Shooting()
    {
        while (true)
        {
            Instantiate(turretMissile, gunEnd.position, gunEnd.rotation);

            //NetworkServer.Spawn(turretMissile);

            //Destroy(turretMissile, 2.0f);

            yield return new WaitForSeconds(2);
        }
    }
}
