using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlasher : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprRend;
    [SerializeField] private float time;
    [SerializeField] private float startDelay;

    private float delay = 0.1f;

	private void Start()
	{
        delay = startDelay;
	}

	void Update()
    {
        delay -= Time.deltaTime;
        if(delay <= 0f)
		{
            delay = time;
            sprRend.enabled = !sprRend.enabled;
		}
    }

    public void SetTime(float t)
	{
        if(time != t)
            time = t;
	}
}
