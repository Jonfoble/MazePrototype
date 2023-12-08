using UnityEngine;
using Cinemachine;

public class CameraAssigner : MonoBehaviour
{
	private CinemachineVirtualCamera m_Camera;

	private void OnEnable()
	{
		m_Camera = GetComponent<CinemachineVirtualCamera>();
		GameManager.Instance.OnPlayerSpawned += AssignCameraTarget;
	}

	private void OnDisable()
	{
		GameManager.Instance.OnPlayerSpawned -= AssignCameraTarget;
	}

	private void AssignCameraTarget(Transform playerTransform)
	{
		m_Camera.Follow = playerTransform;
	}
}
