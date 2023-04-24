using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField] private List<Transform> targets = new List<Transform>();
	[SerializeField] private float distMin;
	[SerializeField] private float distMax;

	Vector3 min = Vector3.one * Mathf.Infinity;
	Vector3 max = Vector3.one * Mathf.NegativeInfinity;
	float dist;
	private Camera cam;

	private void Awake()
	{
		cam = GetComponent<Camera>();
	}

	private void Start()
	{
		
	}

	public void SetTarget(List<Transform> targetList)
	{
		Debug.Log($"CameraController:: {targetList.Count} targets set.");
		targets = targetList;
		min = Vector3.one * Mathf.Infinity;
		max = Vector3.one * Mathf.NegativeInfinity;
		dist = 0f;
		for (int i = 0; i < targets.Count; i++)
		{
			min = new Vector3(
				Mathf.Min(min.x, targets[i].position.x),
				Mathf.Min(min.y, targets[i].position.y),
				cam.transform.position.z);
			max = new Vector3(
				Mathf.Max(max.x, targets[i].position.x),
				Mathf.Max(max.y, targets[i].position.y),
				cam.transform.position.z);
		}
		dist = Mathf.Max(max.x - min.x, max.y - min.y);
	}

	private void UpdateTarget()
	{
		min = Vector3.one * Mathf.Infinity;
		max = Vector3.one * Mathf.NegativeInfinity;
		dist = 0f;
		for (int i = 0; i < targets.Count; i++)
		{
			min = new Vector3(
				Mathf.Min(min.x, targets[i].position.x),
				Mathf.Min(min.y, targets[i].position.y),
				cam.transform.position.z);
			max = new Vector3(
				Mathf.Max(max.x, targets[i].position.x),
				Mathf.Max(max.y, targets[i].position.y),
				cam.transform.position.z);
		}
		dist =Mathf.Max(max.x - min.x, max.y - min.y);
	}

	private void Update()
	{
		UpdateTarget();
		transform.position = Vector3.Lerp(transform.position, Vector3.Lerp(min, max, 0.5f), Time.deltaTime * 0.5f);
		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, Mathf.Clamp(dist * 3, distMin, distMax), Time.deltaTime);
	}
}
