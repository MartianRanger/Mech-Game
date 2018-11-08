using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
public class HomingTurret : NetworkBehaviour
{
    private Transform target;
    [Header("Attributes")]
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    [Header("Unity Setup Fields")]
    public string tag = "Player";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);//search target right at beginning, at a rate of .5 seconds
    }
    void UpdateTarget()//a renewed search, that does not do every frame using InvokeRepeating ^
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");//finding enemy...
        float shortestDistance = Mathf.Infinity;//if havent found enemy, we have infinite distance to enemy.
        GameObject nearestEnemy = null;
        Debug.Log(player.transform.position);
            float distanceToEnemy = Vector3.Distance(transform.position, player.transform.position);//get distance of that enemy
        Debug.Log(distanceToEnemy);
        if (distanceToEnemy < shortestDistance) //if it's the shortest distance that we find
            {
                shortestDistance = distanceToEnemy;//set this distance as shortest enemy distance
                nearestEnemy = player;//nearest enemy that we find = enemy
            }
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
        //transform.LookAt(player.transform);

    }
    // Update is called once per frame
    void Update()
    {
        if (target == null)//if we dont have a target, dont do anything
            return;
        //Target lock on and rotation (First Method I tried)
        /*Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;//smooth rotate
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f); */
        Debug.Log(target.transform.position + " BOOGIE!");
        //Current Method right now
        Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.localRotation = Quaternion.Euler(0, targetPosition.y, 0);
        transform.LookAt(-targetPosition);

        if (fireCountdown <= 0) //time to shoot
        {
            Shoot();
            fireCountdown = 1f / fireRate;//how much fires each second
        }
        fireCountdown -= Time.deltaTime;//make sure every second fire countdown will be reduce by value
    }
    [Client]
    void Shoot()
    {
        //Debug.Log("It's Morphin' Time!");
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

    }
}
