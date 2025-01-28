using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;

    [Header("Game UI Elements")]
    [SerializeField] private Text computerChoiceText;
    [SerializeField] private Text playerScoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private GameObject resultObject;
    [SerializeField] private Text resultText;
    [SerializeField] private Button[] choiceButtons;
    
    [Header("Timer")]
    [SerializeField] private Slider timerSlider;
    [SerializeField] private float roundTime = 1f;
    [SerializeField] private Text timerText;

    [Header("References")]
    [SerializeField] private GameManager gameManager;

    private Coroutine timerCoroutine;
    private bool isGameActive;
    private void Start()
    {
        ShowMenu();
        
        computerChoiceText.text = "";
        resultText.text = "";
        timerText.text = "Timer: "+ roundTime;
        timerSlider.gameObject.SetActive(false);
    }

    public void PlayGame()
    {
        isGameActive = true;
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        resultObject.SetActive(false);
        gameManager.ResetGame();
        UpdateScores();
        ClearSelections();
        StartNewRound();
    }
    
    private void StartNewRound()
    {
        ClearSelections();
        SetButtonsInteractable(true);
        timerSlider.gameObject.SetActive(true);
        timerSlider.value = 1f;
        
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(CountdownTimer());
    }
    public void OnRockButtonClick() => PlayerSelect(RPSLSChoice.Rock);
    public void OnPaperButtonClick() => PlayerSelect(RPSLSChoice.Paper);
    public void OnScissorButtonClick() => PlayerSelect(RPSLSChoice.Scissors);
    public void OnLizardButtonClick() => PlayerSelect(RPSLSChoice.Lizard);
    public void OnSpockButtonClick() => PlayerSelect(RPSLSChoice.Spock);
    public void PlayerSelect(RPSLSChoice choice)
    {
        if (!isGameActive) return;
        
        isGameActive = false;
        StopCoroutine(timerCoroutine);
        
        // Disable buttons during processing
        SetButtonsInteractable(false);

        // Get computer choice and display
        RPSLSChoice computerChoice = gameManager.GetComputerChoice();
        computerChoiceText.text = computerChoice.ToString();

        // Play round
        gameManager.PlayRound(choice,computerChoice);
        UpdateScores();

        // Show result
        StartCoroutine(ProcessResult(choice, computerChoice));
    }

    private IEnumerator ProcessResult(RPSLSChoice playerChoice, RPSLSChoice computerChoice)
    {
        resultObject.SetActive(true);
        int result = RPSLSGame.DetermineRoundResult(playerChoice, computerChoice);
        
        if (result == 1)
        {
            resultText.text = "You win! Next round...";
            yield return new WaitForSeconds(1.5f);
            isGameActive = true;
            StartNewRound();
        }
        else if (result == -1)
        {
            resultText.text = "You lose! Returning to menu...";
            PlayerPrefs.SetInt(RPSLSConstants.HIGHSCOREVALUE, gameManager.PlayerScore);
            yield return new WaitForSeconds(2f);
            ShowMenu();
        }
        else
        {
            resultText.text = "It's a tie! Try again...";
            yield return new WaitForSeconds(1.5f);
            isGameActive = true;
            StartNewRound();
        }
        
    }

    private void UpdateScores()
    {
        playerScoreText.text = "Player: "+gameManager.PlayerScore;
    }

    private void ClearSelections()
    {
        computerChoiceText.text = "";
        resultText.text = "";
        resultObject.SetActive(false);
    }

    private void ShowMenu()
    {
        isGameActive = false;
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerSlider.gameObject.SetActive(false);
        
        highScoreText.text = "High Score : "+PlayerPrefs.GetInt(RPSLSConstants.HIGHSCOREVALUE, 0).ToString();
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        ClearSelections();
        
    }
    
    private IEnumerator CountdownTimer()
    {
        float timer = roundTime;
        
        while (timer > 0 && isGameActive)
        {
            timer -= Time.deltaTime;
            timerSlider.value = 1-timer / roundTime;
            yield return null;
        }

        if (isGameActive)
        {
            // Timeout occurred
            SetButtonsInteractable(false);
            resultObject.SetActive(true);
            resultText.text = "Time's up!";
            yield return new WaitForSeconds(2f);
            ShowMenu();
        }
    }

    private void SetButtonsInteractable(bool state)
    {
        foreach (Button button in choiceButtons)
        {
            button.interactable = state;
        }
    }
}