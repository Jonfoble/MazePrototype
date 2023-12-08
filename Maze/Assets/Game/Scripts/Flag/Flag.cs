using Player.Movement;
using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
	public static event Action OnGameWon;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent(out PlayerMovement Player))
		{
			OnGameWon?.Invoke();
		}
	}
}
