using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour {

    private Vector3 goSpace;
    private Vector3 mouseSpace;
    private Vector3 mousePosition;

    private GameObject currentTouchCube;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnMouseDrag()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 goSpace=Camera.main.WorldToScreenPoint(this.transform.position);
            Vector3 mouseSpace =new Vector3(Input.mousePosition.x, Input.mousePosition.y, goSpace.z);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mouseSpace);

            this.transform.position = mousePosition;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer==8)
        {
            currentTouchCube = other.gameObject;
        }
    }
    void OnMouseUp()
    {
        this.transform.position = currentTouchCube.transform.position;
    }
}
