using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private GameStateController _stateController;
    [SerializeField] private TextMeshProUGUI _bananaScoreText;
    [SerializeField] private TextMeshProUGUI _bananaScoreTextShadow;
    [SerializeField] private TextMeshProUGUI _monkeyCountText;
    [SerializeField] private TextMeshProUGUI _monkeyCountTextShadow;
    [SerializeField] private CountUpTimer _timeControl;
    
    public void SetToState(GameStateController stateController)
    {
        _stateController = stateController;
        _stateController.BananaScoreChanged += UpdateBananaScore;
        _stateController.MonkeysSurvivedChanged += UpdateMonkeyCount;
        SetBananaScoreText(stateController.BananasEaten);
        _timeControl.Reset();
    }

    public void OnEnable()
    {
        _timeControl.Reset();
    }
    
    private void SetBananaScoreText(int newScore)
    {
        _bananaScoreText.text = $"{newScore}";
        _bananaScoreTextShadow.text = $"{newScore}";
    }
    
    private void UpdateBananaScore(int newScore)
    {
        SetBananaScoreText(newScore);
    }

    private void SetMonkeysCount(int newCount)
    {
        _monkeyCountText.text = $"{newCount} MONKEYS";
        _monkeyCountTextShadow.text = $"{newCount} MONKEYS";
    }
    private void UpdateMonkeyCount(int newCount)
    {
        SetMonkeysCount(newCount);
    }
    
    private void OnDisable()
    {
        _stateController.BananaScoreChanged -= UpdateBananaScore;
        _stateController.MonkeysSurvivedChanged -= UpdateMonkeyCount;
    }
    
    
}
