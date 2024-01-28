using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    // This class needs to be able to start the game, reset the game and also keep track of the current state of the game

    #region Assignable Fields for the Main Scene
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Transform _canvasRoot;
    [SerializeField] private Camera _titleCamera;
    #endregion
    
    #region Game State Properties

    public Action<int> BananaScoreChanged;
    public Action<int> BananaHitScoreChanged;
    public Action<int> MonkeysSurvivedChanged;

    private int _bananasEaten;
    public int BananasEaten
    {
        get => _bananasEaten;
        private set
        {
            BananaScoreChanged?.Invoke(value);
            _bananasEaten = value;
        }
    }

    private int _bananasHit;
    public int BananasHit
    {
        get => _bananasHit;
        private set
        {
            BananaHitScoreChanged?.Invoke(value);
            _bananasHit = value;
        }
    }
    private int _monkeysSurvived;

    public int MonkeysSurvived
    {
        get => _monkeysSurvived;
        private set
        {
            MonkeysSurvivedChanged?.Invoke(value);
            _monkeysSurvived = value;
        }
    }

    public int SecondsSurvived { get; private set; }
    #endregion
    
    public Action OnGameStart;
    public Action OnGameReset;
    public Action OnGameEnd;

    public enum GameState
    {
        Title,
        Playing,
        End
    }

    private GameState _gameState;
    public GameState CurrentGameState
    {
        get => _gameState;
        private set => _gameState = value;
    }
    
    #region Private References

    private PlayerController _activePlayer;
    private TitleScreenUI _titleScreenUI;
    private PlayerUI _playerUI;
    private EndScreenUI _endScreenUI;
    #endregion

    void Start()
    {
        SetupGame();
    }

    public void ShowTitleScreen()
    {
        if (_titleScreenUI == null)
        {
            _titleScreenUI = GameManager.Instance.CreateInstance<TitleScreenUI>();
            _titleScreenUI.transform.SetParent(_canvasRoot, false);
            _titleScreenUI.startButton.onClick.AddListener(StartGame);
            _titleScreenUI.quitButton.onClick.AddListener(QuitGame);
        }
        
        _titleScreenUI.gameObject.SetActive(true);
        // Set the camera to the title screen camera
        _titleCamera.gameObject.SetActive(true);
        _titleCamera.tag = "MainCamera";
        if (_activePlayer != null)
        {
            _activePlayer.CameraComponent.tag = "Untagged";
        }
        CurrentGameState = GameState.Title;
    }

    private void AddScore()
    {
        BananasEaten++;
    }
    
    private void AddHitScore()
    {
        BananasHit++;
    }
    
    public void StartGame()
    {
        // Turn off the title scree
        _titleScreenUI.gameObject.SetActive(false);
        
        // Spawn or reset the player
        if (_activePlayer == null)
        {
            _activePlayer = GameManager.Instance.CreateInstance<PlayerController>(null, _playerSpawnPoint.position, _playerSpawnPoint.rotation);
            _activePlayer.OnBananaEaten += AddScore;
            _activePlayer.OnCaughtByMonkey += EndGame;
        }
        else
        {
            _activePlayer.transform.position = _playerSpawnPoint.position;
            _activePlayer.transform.rotation = _playerSpawnPoint.rotation;
        }
        
        _activePlayer.Setup();
        
        // Change the main Camera to the one on the PlayerCharacter instance
        _titleCamera.tag = "Untagged";
        _titleCamera.gameObject.SetActive(false);
        _activePlayer.gameObject.SetActive(true);
        _activePlayer.CameraComponent.tag = "MainCamera";
        
        // Reset score values
        BananasEaten = 0;
        BananasHit = 0;
        MonkeysSurvived = 0;
        SecondsSurvived = 0;
        
        // Monkeys already spawned
        // Nothing to do for now
        
        // Create the Player UI system
        if (_playerUI == null)
        {
            _playerUI = GameManager.Instance.CreateInstance<PlayerUI>();
            _playerUI.transform.SetParent(_canvasRoot, false);
        }
        _playerUI.gameObject.SetActive(true);
        _playerUI.SetToState(this);
        
        OnGameStart?.Invoke();
        // Set the game state to playing
        CurrentGameState = GameState.Playing;
    }

    public void EndGame()
    {
        // Deactivate the player
        _playerUI.gameObject.SetActive(false);
        _activePlayer.gameObject.SetActive(false);
        _titleCamera.gameObject.SetActive(true);
        _titleCamera.tag = "MainCamera";
        _activePlayer.CameraComponent.tag = "Untagged";
        
        ShowEndScreen();

        OnGameEnd?.Invoke();
        CurrentGameState = GameState.End;
    }
    
    public void ShowEndScreen()
    {
        if (_endScreenUI == null)
        {
            _endScreenUI = GameManager.Instance.CreateInstance<EndScreenUI>();
            _endScreenUI.transform.SetParent(_canvasRoot, false);
            _endScreenUI.resetButton.onClick.AddListener(SetupGame);
            _endScreenUI.quitButton.onClick.AddListener(QuitGame);
        }
        _endScreenUI.gameObject.SetActive(true);
        _endScreenUI.SetToState(this);
        _activePlayer.FreeCursor();
    }

    public void SetupGame()
    {
        // TODO something to do with all the monkeys so they can be reset too.
        
        OnGameReset?.Invoke();
        ShowTitleScreen();
        _endScreenUI?.gameObject?.SetActive(false);
        _activePlayer?.gameObject?.SetActive(false);
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }
}