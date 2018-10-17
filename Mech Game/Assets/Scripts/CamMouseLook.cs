using System.Collections;
using UnityEngine;

public class CamMouseLook : MonoBehaviour {

    Vector2 mouseLook;// keeps track of how much movement camera has made
    Vector2 smoothV;// smooths movement of mouse
    public float sensitivity = 5.0f;// mouse sensitivity
    public float smoothing = 2.0f;// mouse smoothing

    GameObject character;

	// Use this for initialization
	void Start () {

        character = this.transform.parent.gameObject;// character is camera's parent
	}
	
	// Update is called once per frame
	void Update () {

        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));// mouse delta (movement of mouse)

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing); //Lerp will allow it to move smoothly between two points
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);// to clamp looking up and down

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
    }
}
