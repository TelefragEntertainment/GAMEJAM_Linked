using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
	{
		get { return instance; }
	}
    private static GameManager instance;

	public CameraController CamController { get { return camController; } }

	[SerializeField] private CameraController camController;
	[SerializeField] private Player[] players;
	
	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		List<Transform> playerTransforms = new List<Transform>();
		foreach (Player p in players)
			playerTransforms.Add(p.transform);
		camController.SetTarget(playerTransforms);
	}
}
