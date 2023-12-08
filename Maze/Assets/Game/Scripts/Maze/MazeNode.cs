using UnityEngine;

public class MazeNode : MonoBehaviour
{
	[SerializeField] private GameObject[] m_Walls;
	[SerializeField] private MeshRenderer m_Floor;

	public void RemoveWall(int wallToRemove)
	{
		m_Walls[wallToRemove].gameObject.SetActive(false);
	}
	public void SetState(NodeState state)
	{
		switch (state)
		{
			case NodeState.Available:
				break;
			case NodeState.Current:
				break;
			case NodeState.Completed:
				break;
		}

	}
}
