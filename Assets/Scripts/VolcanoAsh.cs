using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class VolcanoAsh : MonoBehaviour
{
    static GameObject ashBurnPrefab;

    new Rigidbody rigidbody;
    new Collider trigger;
    TrailRenderer trail;

    public Vector3 targetPos;
    public float maxheight = 100f;
    public float speedMultiply = 1f;

    private float tx;
    private float ty;
    private float tz;
    private float v;
    private float g = 9.8f;
    private bool IsSh = false;
    private float tic = 0.5f;
    private float t;
    private Vector3 StartPos;
    private Vector3 EndPos;
    private float dat;

    public GameObject[] arrModel;
    int activeModelNum;

    void Awake()
    {        
        if (ashBurnPrefab == null)
            ashBurnPrefab = Resources.Load("Prefabs/AshBurn") as GameObject;

        rigidbody = GetComponent<Rigidbody>();
        trigger = GetComponent<Collider>();
        trail = GetComponent<TrailRenderer>();
    }

    void OnSpawned()
    {
        activeModelNum = Random.Range(0, 4);
        arrModel[activeModelNum].SetActive(true);
        transform.localScale = Vector3.one * Random.Range(1f, 3f);
        trail.enabled = true;
    }

    void OnDespawned()
    {
        arrModel[activeModelNum].SetActive(false);
        trail.Clear();
        transform.position = new Vector3(0f, 75f, 0f);
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        trigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        trigger.enabled = false;
        Vector3 spawnPos = transform.position;
        spawnPos.y = -0.2f;
        PoolManager.Pools["AshBurnPool"].Spawn(ashBurnPrefab, spawnPos, Quaternion.identity);
        StartCoroutine(CoDespawn());

        if(other.CompareTag("Tree"))
        {
            TreeCtrl tree = other.GetComponent<TreeCtrl>();
            tree.HitTree();
        }
    }

    IEnumerator CoDespawn()
    {
        yield return new WaitForSeconds(1f);
        PoolManager.Pools["VolcanoAshPool"].Despawn(this.transform);
    }

    public void Init(Vector3 targetPos, float height, float speedMultiply)
    {
        rigidbody.isKinematic = false;

        this.targetPos = targetPos;
        maxheight = height;
        this.speedMultiply = speedMultiply;

        StartPos = this.transform.position;
        var dh = targetPos.y - StartPos.y;
        var mh = maxheight - StartPos.y;
        ty = Mathf.Sqrt(2 * g * mh);
        float a = g;
        float b = -2 * ty;
        float c = 2 * dh;
        dat = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);

        tx = -(StartPos.x - targetPos.x) / dat;
        tz = -(StartPos.z - targetPos.z) / dat;
        this.tic = 0;
    }

    void FixedUpdate()
    {
        if (rigidbody.isKinematic == true)
            return;

        this.tic += Time.deltaTime * speedMultiply;
        var _tx = StartPos.x + tx * tic;
        var _ty = StartPos.y + ty * tic - 0.5f * g * tic * tic;
        var _tz = StartPos.z + tz * tic;

        var tpos = new Vector3(_tx, _ty, _tz);
        rigidbody.MovePosition(tpos);
    }
}
