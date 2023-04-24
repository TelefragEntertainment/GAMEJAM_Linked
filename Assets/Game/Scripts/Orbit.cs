using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] private float speed;

	private void FixedUpdate()
	{
		transform.Rotate(new Vector3(0, 0, speed));
		foreach (Transform t in transform)
		{
			t.Rotate(new Vector3(0, 0, -speed));
		}
	}
}
