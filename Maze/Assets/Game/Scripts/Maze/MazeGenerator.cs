using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
	[SerializeField] private MazeNode m_NodePrefab;
	[SerializeField] private Vector2Int m_MazeSize;

	private void Start()
	{
		StartCoroutine(GenerateMaze(m_MazeSize));
	}
	IEnumerator GenerateMaze(Vector2Int size)
	{
		List<MazeNode> nodes = new List<MazeNode>();


		//Create Maze Nodes
		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				Vector3 nodePos = new Vector3(x - (size.x / 2f),0, y - (size.y / 2f));
				Instantiate(m_NodePrefab, nodePos, Quaternion.identity, transform);
				nodes.Add(m_NodePrefab);
				yield return null;
			}
		}

		List<MazeNode> currentPath = new List<MazeNode>();
		List<MazeNode> completedNodes = new List<MazeNode>();

		//Assigning random starting node
		currentPath.Add(nodes[Random.Range(0, nodes.Count)]);
		currentPath[0].SetState(NodeState.Current);
		Debug.Log(currentPath[0].transform.position);
	}
}
