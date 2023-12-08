using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI timerText;
	[SerializeField] private TextMeshProUGUI highScoreText;
	[SerializeField] private GameObject m_CongratsText;
	[SerializeField] private GameObject m_RestartButton;
	[SerializeField] private GameObject m_ExitButton;

	private void OnEnable()
	{
		GameManager.Instance.OnGameEnd += ActivateEndGameUI;
	}

	private void OnDisable()
	{
		GameManager.Instance.OnGameEnd -= ActivateEndGameUI;
	}

	private void Start()
	{
		m_CongratsText.SetActive(false);
		m_RestartButton.SetActive(false);
		m_ExitButton.SetActive(false);
	}
	private void Update()
	{
		UpdateTimerUI();
	}

	private void ActivateEndGameUI()
	{
		m_CongratsText.SetActive(true);
		m_RestartButton.SetActive(true);
		m_ExitButton.SetActive(true);
	}
	private void UpdateTimerUI()
	{
		if (GameManager.Instance)
		{
			timerText.text = $"Time: {GameManager.Instance.CurrentTime.ToString("F2")}";

			// Handle no high score case
			string highScoreDisplay = GameManager.Instance.HighScore > 0 ?
				GameManager.Instance.HighScore.ToString("F2") : "0";
			highScoreText.text = $"High Score: {highScoreDisplay}";
		}
	}
}
