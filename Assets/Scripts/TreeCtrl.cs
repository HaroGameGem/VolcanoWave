using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TreeCtrl : MonoBehaviour {

    new Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    public void HitTree()
    {
        rigidbody.isKinematic = false;
        rigidbody.AddForce(new Vector3(Random.Range(-200f, 200f), Random.Range(400f, 800f), Random.Range(-200f, 200f)));
        rigidbody.AddTorque(new Vector3(Random.Range(60f, 180f), Random.Range(60f, 180f), Random.Range(60f, 180f)));
    }
}
