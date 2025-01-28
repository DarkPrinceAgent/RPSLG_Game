using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int PlayerScore { get; private set; }

    public void PlayRound(RPSLSChoice playerChoice, RPSLSChoice computerChoice)
    {
        var result = RPSLSGame.DetermineRoundResult(playerChoice, computerChoice);
        
        UpdateScores(result);
    }

    public RPSLSChoice GetComputerChoice()
    {
        return (RPSLSChoice)Random.Range(0, 5);
    }

    private void UpdateScores(int result)
    {
        if (result == 1) PlayerScore++;
    }

    public void ResetGame()
    {
        PlayerScore = 0;
    }
}