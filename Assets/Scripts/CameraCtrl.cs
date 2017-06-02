using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {

    public Transform target;
    public GameObject Sound;
    Vector3 offset;

    // How long the object should shake for.
    public float shake = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    public bool isTitle = false;

    // Use this for initialization
    void Start () {
        if(isTitle == false)
        {
            offset = transform.position - target.position;
            originalPos = transform.localPosition;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if(isTitle == false)
        {
            Vector3 movePos = Vector3.MoveTowards(transform.position, target.position + offset, 9f * Time.deltaTime);
            if (shake > 0)
            {
                Sound.GetComponent<AudioSource>().enabled = true;
                transform.position = movePos + Random.insideUnitSphere * shakeAmount;
                StartCoroutine(soundoff());
                shake -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shake = 0f;
                transform.position = movePos;
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 2f);
        }
    }
    IEnumerator soundoff()
    {
        yield return new WaitForSeconds(2.0f);
        Sound.GetComponent<AudioSource>().enabled = false;
    }

}
