using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCtrl : MonoBehaviour {
    public event Action<int> EventPlayerTakeDamage;
    public event Action EventPlayerDie;

	public float maxVelocity = 20f;
	public float runSpeed = 6f;
    public float walkSpeed = 2f;
    public float dashSpeed = 12f;
	public float rotAngle = 360f;
    public GameObject HitEffect;
    public GameObject DashEffect;
    public GameObject hitSound;
    public float maxStamina = 20f;
    public float stamina;
    public float staminaRestoreAmount = 1.5f;
    public float staminaConsumeAmount = 1f;
    public float staminaDashAmount = 4f;

    new Rigidbody rigidbody;
    new Animator animator;
	
	public Vector3 moveVector;
    Vector3 dashVector;
    public bool isRun = false;
    public bool isDashing = false;
    public bool isDie = false;

    int hp;
    public int maxHP;
	
	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        stamina = maxStamina;
        hp = maxHP;
	}
	
	// Update is called once per frame
	void Update () {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        moveVector = new Vector3(h, 0f, v);
        animator.SetFloat("MoveAxis", moveVector.magnitude);

		if(moveVector != Vector3.zero && isDashing == false)
		{
			transform.rotation = Quaternion.RotateTowards(transform.localRotation,
				Quaternion.LookRotation(moveVector),
				rotAngle * Time.deltaTime);
		}			

        if(Input.GetKeyDown(KeyCode.E))
        {
            isRun = !isRun;
            this.gameObject.GetComponent<AudioSource>().enabled = true;
            animator.SetBool("IsRun", isRun);
        }

        if(Input.GetMouseButtonDown(0))
        {
            //Debug.Log(Input.mousePosition);

            if (isDashing == false)
            {
                RaycastHit hit = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("VolcanoAsh"))
                        return;

                    Vector3 target = hit.point - transform.position;
                    transform.LookAt(hit.point);
                    dashVector = target.normalized;
                }
                StartCoroutine(CoDash());
            }
        }

        if(transform.position.y < -10f)
        {
            EventPlayerDie();
        }

        if(GameManager.Instance.isGod)
        {
            hp = maxHP;
            stamina = maxStamina;
        }
	}
	
	void FixedUpdate()
	{
        if(isRun)
        {
            stamina -= staminaConsumeAmount * Time.deltaTime;
            if(stamina < 0f)
            {
                isRun = false;
                if (this.gameObject.GetComponent<AudioSource>().enabled == true)
                {
                    this.gameObject.GetComponent<AudioSource>().enabled = false;
                }
                animator.SetBool("IsRun", isRun);
                stamina = 0f;
            }
        }
        else
        {
            if (this.gameObject.GetComponent<AudioSource>().enabled == false)
            {
                this.gameObject.GetComponent<AudioSource>().enabled = true;
            }
            stamina += staminaRestoreAmount * Time.deltaTime;
            if(stamina > maxStamina)
            {
                stamina = maxStamina;
            }
        }

        if (isDashing)
        {

            Vector3 velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxVelocity);
            rigidbody.MovePosition(rigidbody.position + dashVector * dashSpeed * Time.fixedDeltaTime);
            rigidbody.velocity = velocity;
            return;
        }

        float speed = isRun ? runSpeed : walkSpeed;
        if (moveVector != Vector3.zero)
        {
            Vector3 velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxVelocity);
            rigidbody.MovePosition(rigidbody.position + moveVector * speed * Time.fixedDeltaTime);
            rigidbody.velocity = velocity;
        }
    }

    IEnumerator CoDash()
    {
        if (UseStamina(staminaDashAmount))
        {
            animator.SetTrigger("Dash");
            DashEffect.SetActive(true);
            isDashing = true;
            yield return new WaitForSeconds(0.5f);
            DashEffect.SetActive(false);
            isDashing = false;
        }
        yield return null;
    }

    bool UseStamina(float usedAmount)
    {
        float tmp = stamina - usedAmount;
        if (tmp < 0)
            return false;

        stamina = tmp;
        return true;
    }

    public bool TakeDamage()
    {
        if (isDashing)
            return false;

        if (hp > 0)
        {
            Debug.Log(hp);
            hitSound.GetComponent<AudioSource>().enabled = true;
            hp--;
            HitEffect.SetActive(true);
           
            StartCoroutine(hiteffect(0.5f));
            if (EventPlayerTakeDamage != null)
                EventPlayerTakeDamage(hp);
            if (hp == 0)
            {
                if (EventPlayerDie != null)
                    EventPlayerDie();

                isDie = true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator hiteffect(float time)
    {
        yield return new WaitForSeconds(time);
        if(HitEffect.activeSelf ==true)
        {
            HitEffect.SetActive(false);
            hitSound.GetComponent<AudioSource>().enabled = false;
        }       
    }
}
