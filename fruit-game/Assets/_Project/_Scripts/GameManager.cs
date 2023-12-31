using System.Collections;
using System.Collections.Generic;
using LeaderboardCreatorDemo;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }


    [SerializeField] private Transform clearer;
    [SerializeField] private Transform clearStartPoint;
    [SerializeField] private Transform clearEndPoint;
    [SerializeField] private static bool gameOver;
    [SerializeField] private static LeaderboardManager leaderboardManager;
    public static int Score;
    public static int newScore;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private static HighScore highScore;

    private float elapsedTime;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        leaderboardManager = FindObjectOfType<LeaderboardManager>();
        highScore = FindObjectOfType<HighScore>();
    }
    private void Start()
    {
        clearer.position = clearStartPoint.position;
        clearer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !leaderboardManager._usernameInputField.isFocused) { Restart(); }

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime);

        Score = (int)Mathf.Lerp(Score, newScore, t);

        if (t >= 1f) { elapsedTime = 0f; }
        scoreText.text = Score.ToString();


        if (gameOver)
        {
            clearer.gameObject.SetActive(true);
            clearer.position = Vector3.MoveTowards(clearer.position, clearEndPoint.position, 5 * Time.deltaTime);

            if (clearer.position == clearEndPoint.position)
            {
                gameOver = false;
                clearer.position = clearStartPoint.position;
                clearer.gameObject.SetActive(false);
            }
        }
    }
    public static void Restart()
    {
        gameOver = true;

        leaderboardManager.UploadEntry();

        highScore.UpdateScore(newScore);
        newScore = 0;
    }

}
