using Player.Movement;
using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public static event Action OnObstacleHit;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent(out PlayerMovement Player))
		{
			OnObstacleHit?.Invoke();
		}
	}
}
