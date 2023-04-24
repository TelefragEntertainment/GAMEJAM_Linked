using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
	[SerializeField] private float delay;
    [SerializeField] private List<ParticleSystem> fx_explosions;
    [SerializeField] private List<Rigidbody2D> station_parts;
    [SerializeField] private List<GameObject> hideOnExplo;
	[SerializeField] private List<Player> players;
	[SerializeField] private AudioClip sfx_explode;

	private void Start()
	{
		Invoke(nameof(Jump), delay);
		Invoke(nameof(Explode), delay + 1.5f);
	}

	public void Jump()
	{
		players[0].PlayJumpAni();
		players[1].PlayJumpAni();
	}

	public void Explode()
	{
		AudioManager.Play(sfx_explode);
		foreach (GameObject g in hideOnExplo)
		{
			g.SetActive(false);
		}
		foreach (ParticleSystem ps in fx_explosions)
		{
			ps.Play();
		}
		foreach (Rigidbody2D rb in station_parts)
		{
			rb.gameObject.SetActive(true);
			rb.AddForce(Random.insideUnitCircle * Random.Range(50, 100));
			rb.angularVelocity = Random.Range(-100, 100);
		}
	}
}
