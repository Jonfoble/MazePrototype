using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	[SerializeField] private GameObject playerPrefab;
	public Action<Transform> OnPlayerSpawned;
	public Action OnGameEnd;
	public float CurrentTime { get; private set; }
	public float HighScore  { get; private set; }

	private bool gameRunning = false;

	private void OnEnable()
	{
		MazeGenerator.Instance.OnMazeReady += SpawnPlayerAtStart;
		Flag.OnGameWon += HandleGameWon;
		Obstacle.OnObstacleHit += ResetGame;
	}

	private void OnDisable()
	{
		MazeGenerator.Instance.OnMazeReady -= SpawnPlayerAtStart;
		Flag.OnGameWon -= HandleGameWon;
		Obstacle.OnObstacleHit -= ResetGame;

	}
	private void Start()
	{
		CurrentTime = 0f;

		if (PlayerPrefs.HasKey("HighScore"))
		{
			HighScore = PlayerPrefs.GetFloat("HighScore");
		}
		else
		{
			HighScore = 100f;
		}

		StartGame();
	}
	private void Update()
	{
		if (gameRunning)
		{
			CurrentTime += Time.deltaTime;
		}
	}
	private void StartGame()
	{
		gameRunning = true;
		CurrentTime = 0f;
	}

	private void HandleGameWon()
	{
		gameRunning = false;
		if (CurrentTime < HighScore)
		{
			HighScore = CurrentTime;
			PlayerPrefs.SetFloat("HighScore", HighScore);
		}
		OnGameEnd?.Invoke();
	}
	private void SpawnPlayerAtStart(List<MazeNode> nodes)
	{
		if (playerPrefab != null && nodes.Count > 0)
		{
			MazeNode startNode = nodes[0];
			Vector3 spawnPosition = startNode.transform.position;
			spawnPosition.y += 1.0f; // Adjust for height

			GameObject Player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
			OnPlayerSpawned?.Invoke(Player.transform);
		}
	}
	public void ResetGame()
	{
		// Reset the game
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void ExitGame()
	{
		Application.Quit();
	}
}
