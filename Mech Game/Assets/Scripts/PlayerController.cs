using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public LayerMask layerMask; //where the raycast will hit
    public float moveSpeed = 2;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 currentLookTarget = Vector3.zero;
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        //var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
        //var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //transform.Rotate(0, x, 0);
        characterController.Move(moveDirection * Time.deltaTime * moveSpeed);


        if (Input.GetButtonDown("Fire1"))
        {
            CmdFire();
        }
    }

    //for animation and raycasting
    public void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (moveDirection == Vector3.zero)
        {
            //if the mech is not moving play the the animation for idle
            //bodyAnimator.SetBool("IsMoving", false);
        }
        else
        {
            //if the mech is moving play the the animation for walking
            //bodyAnimator.SetBool("IsMoving", true);
        }

        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);

        if (Physics.Raycast(ray, out hit, 1000, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.point != currentLookTarget)
            {
                currentLookTarget = hit.point;

                Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                // 2
                Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
                // 3
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10.0f);
            }
        }

    }

    // This [Command] code is called on the Client …
    // … but it is run on the Server!
    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraFollow>().setTarget(gameObject.transform);
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
