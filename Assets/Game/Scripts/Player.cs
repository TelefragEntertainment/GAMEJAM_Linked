using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Id { get { return id; } }
    public enum PlayerStateEnum
	{
		Jump,
		Grounded
	}

    [SerializeField] private int id;
	[SerializeField] private float rotOffset;
	[SerializeField] private float gravity;
	[SerializeField] private SpriteRenderer sprRend;
	[SerializeField] private Color color;
	[SerializeField] private List<SpriteRenderer> lights;
	[SerializeField] private float oxygen;
	[SerializeField] private float oxygenMax = 30;

	[Header("SFX")]
	[SerializeField] private AudioClip sfx_alarm1;
	[SerializeField] private AudioClip sfx_alarm2;
	[SerializeField] private AudioClip sfx_jump;
	[SerializeField] private AudioClip sfx_o2Fill;

	private Rigidbody2D rb;
	private GameObject ground;
	private GameObject prevGround;
	private Animator ani;
	private PlayerStateEnum state;
	private float x_input;
	private float dirModifier;
	private float lightOffset;
	private float hangTime;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		ani = GetComponent<Animator>();
	}

	private void Start()
	{
		oxygen = oxygenMax;

		foreach (SpriteRenderer sr in lights)
		{
			sr.color = color;
			lightOffset = sr.transform.localPosition.x;
		}
	}

	private void Update()
	{
		oxygen -= Time.deltaTime;
		float n = Mathf.Ceil((oxygen / oxygenMax) * lights.Count);
		for (int i = lights.Count - 1; i >= 0; i--)
		{
			if (i < n)
			{
				lights[i].gameObject.SetActive(true);
			}
			else
			{
				lights[i].gameObject.SetActive(false);
			}

			lights[i].transform.localPosition = new Vector3(
				sprRend.flipX ? -lightOffset : lightOffset,
				lights[i].transform.localPosition.y,
				0);	

			if(n <= 2)
			{
				lights[i].GetComponent<LightFlasher>().enabled = true;
				AudioManager.PlayLoop(n <= 1 ? sfx_alarm2 : sfx_alarm1);
			}
			else
			{
				lights[i].GetComponent<LightFlasher>().enabled = false;
				lights[i].enabled = true;
			}
		}

		if (state == PlayerStateEnum.Grounded)
		{
			if(rb.velocity.magnitude > 0.02f)
			{
				ani.Play("run");
				ani.speed = Mathf.Clamp(rb.velocity.magnitude * 1.25f, 0.5f, 2);
			}
			else
			{
				ani.Play("idle");
			}

			sprRend.flipX = Vector3.Dot(rb.velocity, transform.right) < 0;
			x_input = Input.GetAxisRaw($"Horizontal{id}");

			if (Input.GetButtonDown($"Jump{id}"))
			{
				ani.Play("jump");
				state = PlayerStateEnum.Jump;
			}
		}
		else
		{
			hangTime += Time.fixedDeltaTime;
		}
	}

	private void FixedUpdate()
	{
		if (ground)
		{
			rb.AddForce(( ground.transform.position - transform.position).normalized * gravity);

			Vector2 vecToGround = transform.position - ground.transform.position;
			float angle = Mathf.Atan2(vecToGround.y, vecToGround.x) * Mathf.Rad2Deg + rotOffset;
			Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
			rb.SetRotation(Mathf.LerpAngle(rb.rotation, angle, 0.5f));

			if (x_input == 0)
				dirModifier = transform.position.y < ground.transform.position.y ? -1 : 1;
			rb.AddForce(transform.right * x_input * dirModifier * 0.3f);
			hangTime = 0;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (state == PlayerStateEnum.Jump)
		{
			// Land
			if (collision.gameObject.CompareTag("Terrain"))
			{
				if ((prevGround && collision.gameObject != prevGround) || !prevGround)
				{
					ground = collision.gameObject;
					state = PlayerStateEnum.Grounded;
					rb.drag = 0.5f;
					rb.angularDrag = 0.5f;
				}
			}
			
		}
		else if (state == PlayerStateEnum.Grounded) {
			//Fill o2
			if (collision.gameObject.CompareTag("o2"))
			{
				AudioManager.Play(sfx_o2Fill);
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (state == PlayerStateEnum.Grounded)
		{
			//Fill o2
			if (collision.gameObject.CompareTag("o2"))
			{
				oxygen = Mathf.Min(oxygen + Time.deltaTime * 4, oxygenMax);
			}
		}
	}

	public void Jump()
	{
		state = PlayerStateEnum.Jump;
		rb.drag = 0;
		rb.angularDrag = 0;
		rb.AddForce(transform.up * 10f);
		if (ground)
			prevGround = ground;
		ground = null;
	}

	public void PlayJumpAni()
	{
		ani.Play("jump");
		if (transform.parent != null)
		{
			transform.parent = null;
		}
	}
}
