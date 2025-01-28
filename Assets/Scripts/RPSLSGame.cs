using System.Collections.Generic;

public enum RPSLSChoice
{
    Rock,
    Paper,
    Scissors,
    Lizard,
    Spock
}

public static class RPSLSGame
{
    private static readonly Dictionary<RPSLSChoice, List<RPSLSChoice>> rules = new()
    {
        { RPSLSChoice.Rock, new List<RPSLSChoice> { RPSLSChoice.Scissors, RPSLSChoice.Lizard } },
        { RPSLSChoice.Paper, new List<RPSLSChoice> { RPSLSChoice.Rock, RPSLSChoice.Spock } },
        { RPSLSChoice.Scissors, new List<RPSLSChoice> { RPSLSChoice.Paper, RPSLSChoice.Lizard } },
        { RPSLSChoice.Lizard, new List<RPSLSChoice> { RPSLSChoice.Paper, RPSLSChoice.Spock } },
        { RPSLSChoice.Spock, new List<RPSLSChoice> { RPSLSChoice.Scissors, RPSLSChoice.Rock } }
    };

    public static int DetermineRoundResult(RPSLSChoice player1Choice, RPSLSChoice player2Choice)
    {
        if (player1Choice == player2Choice) return 0; // Draw
        
        return rules[player1Choice].Contains(player2Choice) ? 1 : -1;
    }

}
