using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tether : MonoBehaviour
{
	[SerializeField] private Rigidbody2D player1;
	[SerializeField] private Rigidbody2D player2;
	[Range(2,20)][SerializeField] private int linkCount;
	[SerializeField] private Rigidbody2D linkPrefab;
	[Range(0.01f,5f)][SerializeField] private float jointDistance;
	[SerializeField] private float jointDamp;
	[SerializeField] private float jointFreq;

	private List<Rigidbody2D> links = new List<Rigidbody2D>();
	private LineRenderer line;
	private Vector3[] positions;

	private void Awake()
	{
		line = GetComponent<LineRenderer>();
	}

	private void Start()
	{
		//Create nodes for tether
		for (int i = 0; i < linkCount; i++)
		{
			Rigidbody2D link = Instantiate(linkPrefab, Vector3.Lerp(player1.position, player2.position, ((i+1) * 1f/linkCount)), Quaternion.identity);
			link.transform.SetParent(transform);
			links.Add(link);
			
			SpringJoint2D j = link.GetComponent<SpringJoint2D>();
			j.distance = jointDistance;
			j.dampingRatio = jointDamp;
			j.frequency = jointFreq;

			if (i == 0)
				j.connectedBody = player1;
			else if (i == linkCount-1) // Add a joint to the 2nd to last link + player2
			{
				SpringJoint2D j2 = link.gameObject.AddComponent<SpringJoint2D>();
				j.connectedBody = player2;
				j2.connectedBody = links[i - 1];
				j2.distance = jointDistance;
				j2.dampingRatio = jointDamp;
				j2.frequency = jointFreq;
			}
			else
			{
				j.connectedBody = links[i - 1];
			}
		}

		positions = new Vector3[2 + links.Count];
		line.positionCount = positions.Length;
	}

	private void FixedUpdate()
	{
		positions[0] = player1.position;
		for (int i = 0; i < links.Count; i++)
		{
			positions[i + 1] = links[i].position;
		}
		positions[positions.Length - 1] = player2.position;
		line.SetPositions(positions);
	}
}
