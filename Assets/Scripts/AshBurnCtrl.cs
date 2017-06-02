using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using DG.Tweening;

public class AshBurnCtrl : MonoBehaviour {

    public static Vector3 beginScale = Vector3.zero;

    public Transform particleTransform;
    public ParticleSystem particle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PoolManager.Pools["AshBurnPool"].Despawn(transform);
            other.transform.GetComponent<PlayerCtrl>().TakeDamage();
        }
    }

    void OnSpawned()
    {
        if (beginScale == Vector3.zero)
            beginScale = transform.localScale;

        StartCoroutine(CoScaleTween());
        PoolManager.Pools["AshBurnPool"].Despawn(transform, 4f);
        particle.Play();
    }

    IEnumerator CoScaleTween()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(beginScale, 0.5f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(1.5f);
        transform.DOScale(0f, 2f);
        
    }
}
