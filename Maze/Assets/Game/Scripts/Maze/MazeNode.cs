using UnityEngine;

public class MazeNode : MonoBehaviour
{
	[SerializeField] private GameObject[] m_Walls;
	[SerializeField] private MeshRenderer m_Floor;

	public void SetState(NodeState state)
	{
		switch (state)
		{
			case NodeState.Available:
				m_Floor.material.color = Color.white; break;
			case NodeState.Current:
				m_Floor.material.color = Color.yellow; break;
			case NodeState.Completed:
				m_Floor.material.color = Color.blue; break;
		}

	}
}
