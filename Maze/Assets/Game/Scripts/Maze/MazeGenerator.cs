using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : Singleton<MazeGenerator> // This class applies Depth First Search Algorithm When Generating The Maze
{
	[SerializeField] private MazeNode nodePrefab;
	[SerializeField] private Vector2Int mazeSize;
	[SerializeField] private GameObject flagPrefab;
	[SerializeField] private GameObject obstaclePrefab;
	[SerializeField] private int numberOfObstacles; // Number of obstacles to place

	[SerializeField] private float nodeSize = 1.0f; //this should match the size of the prefab as well

	public Action<Transform> OnFlagSpawned;

	public event Action<List<MazeNode>> OnMazeReady;

	private void Start()
	{
		List<MazeNode> nodes = GenerateMaze(mazeSize);
		SpawnObstacles(nodes);
		SpawnFlagAtEnd(nodes);
		OnMazeReady?.Invoke(nodes);
	}
	private List<MazeNode> GenerateMaze(Vector2Int size)
	{
		List<MazeNode> nodes = CreateNodes(size);
		GeneratePathThroughMaze(nodes, size);
		return nodes;
	}

	private List<MazeNode> CreateNodes(Vector2Int size)
	{
		List<MazeNode> nodes = new List<MazeNode>();
		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				Vector3 nodePos = new Vector3(x * nodeSize - (size.x * nodeSize / 2f), 0, y * nodeSize - (size.y * nodeSize / 2f));
				MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
				nodes.Add(newNode);
			}
		}
		return nodes;
	}

	private void GeneratePathThroughMaze(List<MazeNode> nodes, Vector2Int size)
	{
		List<MazeNode> currentPath = new List<MazeNode>();
		List<MazeNode> completedNodes = new List<MazeNode>();

		currentPath.Add(nodes[UnityEngine.Random.Range(0, nodes.Count)]);
		currentPath[0].SetState(NodeState.Current);

		while (completedNodes.Count < nodes.Count)
		{
			MazeNode currentNode = currentPath[currentPath.Count - 1];
			Vector2Int currentNodePos = GetNodePosition(nodes.IndexOf(currentNode), size);

			List<int> possibleNextNodeIndices = GetPossibleNextNodeIndices(currentNodePos, size, nodes, completedNodes, currentPath);

			if (possibleNextNodeIndices.Count > 0)
			{
				int chosenNodeIndex = possibleNextNodeIndices[UnityEngine.Random.Range(0, possibleNextNodeIndices.Count)];
				HandleChosenNode(chosenNodeIndex, nodes, currentNode, currentPath, currentNodePos, size);
			}
			else
			{
				MarkNodeAsCompleted(currentNode, currentPath, completedNodes);
			}
		}
	}

	private Vector2Int GetNodePosition(int index, Vector2Int size)
	{
		int x = index / size.y;
		int y = index % size.y;
		return new Vector2Int(x, y);
	}

	private List<int> GetPossibleNextNodeIndices(Vector2Int currentNodePos, Vector2Int size, List<MazeNode> nodes, List<MazeNode> completedNodes, List<MazeNode> currentPath)
	{
		List<int> possibleNextNodeIndices = new List<int>();

		// Check in all four directions
		if (currentNodePos.x < size.x - 1) AddNodeIndexIfValid(currentNodePos.x + 1, currentNodePos.y, size, nodes, completedNodes, currentPath, possibleNextNodeIndices);
		if (currentNodePos.x > 0) AddNodeIndexIfValid(currentNodePos.x - 1, currentNodePos.y, size, nodes, completedNodes, currentPath, possibleNextNodeIndices);
		if (currentNodePos.y < size.y - 1) AddNodeIndexIfValid(currentNodePos.x, currentNodePos.y + 1, size, nodes, completedNodes, currentPath, possibleNextNodeIndices);
		if (currentNodePos.y > 0) AddNodeIndexIfValid(currentNodePos.x, currentNodePos.y - 1, size, nodes, completedNodes, currentPath, possibleNextNodeIndices);

		return possibleNextNodeIndices;
	}

	private void AddNodeIndexIfValid(int x, int y, Vector2Int size, List<MazeNode> nodes, List<MazeNode> completedNodes, List<MazeNode> currentPath, List<int> possibleNextNodeIndices)
	{
		int index = x * size.y + y;
		if (!completedNodes.Contains(nodes[index]) && !currentPath.Contains(nodes[index]))
		{
			possibleNextNodeIndices.Add(index);
		}
	}

	private void HandleChosenNode(int chosenNodeIndex, List<MazeNode> nodes, MazeNode currentNode, List<MazeNode> currentPath, Vector2Int currentNodePos, Vector2Int size)
	{
		MazeNode chosenNode = nodes[chosenNodeIndex];
		Vector2Int chosenNodePos = GetNodePosition(chosenNodeIndex, size);

		// Determine direction from currentNode to chosenNode and remove walls accordingly
		if (chosenNodePos.x > currentNodePos.x)
		{
			// Node is to the right
			chosenNode.RemoveWall(1);
			currentNode.RemoveWall(0);
		}
		else if (chosenNodePos.x < currentNodePos.x)
		{
			// Node is to the left
			chosenNode.RemoveWall(0);
			currentNode.RemoveWall(1);
		}
		else if (chosenNodePos.y > currentNodePos.y)
		{
			// Node is above
			chosenNode.RemoveWall(3);
			currentNode.RemoveWall(2);
		}
		else if (chosenNodePos.y < currentNodePos.y)
		{
			// Node is below
			chosenNode.RemoveWall(2);
			currentNode.RemoveWall(3);
		}

		currentPath.Add(chosenNode);
		chosenNode.SetState(NodeState.Current);
	}

	private void MarkNodeAsCompleted(MazeNode currentNode, List<MazeNode> currentPath, List<MazeNode> completedNodes)
	{
		completedNodes.Add(currentNode);
		currentNode.SetState(NodeState.Completed);
		currentPath.RemoveAt(currentPath.Count - 1);
	}

	private void SpawnFlagAtEnd(List<MazeNode> nodes)
	{
		if (flagPrefab != null && nodes.Count > 0)
		{
			MazeNode endNode = nodes[nodes.Count - 1];
			Vector3 spawnPosition = endNode.transform.position;
			GameObject Flag = Instantiate(flagPrefab, spawnPosition, Quaternion.identity);
			OnFlagSpawned?.Invoke(Flag.transform);
		}
	}
	private void SpawnObstacles(List<MazeNode> nodes)
	{
		HashSet<int> occupiedIndices = new HashSet<int>
	{
		0, // Excluding the first node (Player Spawn)
        nodes.Count - 1 // Excluding the last node (Flag Spawn)
    };

		for (int i = 0; i < numberOfObstacles; i++)
		{
			int index;
			do
			{
				index = UnityEngine.Random.Range(0, nodes.Count);
			}
			while (occupiedIndices.Contains(index));

			occupiedIndices.Add(index);
			Instantiate(obstaclePrefab, nodes[index].transform.position, Quaternion.identity, transform);
		}
	}
}
