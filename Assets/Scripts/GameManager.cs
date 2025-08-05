using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameStarted = false;
    private bool _isGameOver = false;

    [SerializeField] private DinoController[] dinoControllers;
    private StageManager _stageManager;
    private UIController _uiController;

    private InputAction _utilityAction;

    private void Awake()
    {
        dinoControllers = FindObjectsOfType<DinoController>();
        _stageManager = FindObjectOfType<StageManager>();
        _uiController = FindObjectOfType<UIController>();

        _utilityAction = InputSystem.actions.FindAction("Utility");
    }

    private void OnEnable()
    {
        _utilityAction.Enable();
        _utilityAction.performed += OnUtilityPerformed;
    }

    private void OnDisable()
    {
        _utilityAction.Disable();
        _utilityAction.performed -= OnUtilityPerformed;
    }

    private void OnUtilityPerformed(InputAction.CallbackContext ctx)
    {
        if (!_isGameStarted)
        {
            StartCoroutine(StartGame());
        }
        else if (_isGameOver)
        {
            Retry();
        }
    }

    private IEnumerator StartGame()
    {
        if (_isGameStarted) yield break;

        foreach (var dino in dinoControllers)
        {
            dino.RandomJump();
            dino.OnTouchObstacle += GameOver;
        }

        yield return new WaitUntil(() => dinoControllers.All(d => d.IsGrounded));

        foreach (var dino in dinoControllers)
        {
            dino.StartRunAnimation();
        }

        _stageManager.StartScrollStage();
        _uiController.TitleToIngame();
        _isGameStarted = true;
    }

    private void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        MainSceneDataStore.Instance.highScore = Mathf.Max(MainSceneDataStore.Instance.score, MainSceneDataStore.Instance.highScore);
        MainSceneDataStore.Instance.score = 0;
    }

    public void GameOver()
    {
        if (!_isGameStarted || _isGameOver) return;
        _uiController.IngameToGameOver();
        Time.timeScale = 0;
        _isGameOver = true;
    }
}