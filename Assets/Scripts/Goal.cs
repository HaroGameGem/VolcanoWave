using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    float startYPos;
    float yOffset;
    
	// Use this for initialization
	void Start () {
        startYPos = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        yOffset = Mathf.Sin(Time.time);
        transform.position = new Vector3(transform.position.x, startYPos + yOffset, transform.position.z);

        Vector3 rot = transform.eulerAngles + (new Vector3(20f, 0f, 33f)) * Time.deltaTime;
        if (rot.x >= 360)
            rot.x = 0f;
        if (rot.z >= 360)
            rot.z = 0f;
        transform.rotation = Quaternion.Euler(rot);

	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.GameWin();
        }
    }
}
