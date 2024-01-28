using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenUI : MonoBehaviour
{
    public Button resetButton;
    public Button quitButton;
    public TextMeshProUGUI scoreText;
    
    public void SetToState(GameStateController stateController)
    {
        scoreText.text =
            $"{stateController.BananasEaten} bananas eaten\n" +
            $"{stateController.BananasHit} Monkeys Hit\n";
    }
}
