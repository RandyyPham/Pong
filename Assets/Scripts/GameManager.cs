using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private Ball ball;
    [SerializeField] private Paddle playerPaddle, computerPaddle;

    [Header("UI")]
    [SerializeField] private Text playerScoreText;
    [SerializeField] private Text computerScoreText, songTitle, backgroundName, playPauseText, winText, scoreText;
    [SerializeField] private Canvas pauseScreen, winScreen;

    [Space]
    [SerializeField] private AudioSource lose, win;
    [SerializeField] private AudioSource[] backgroundMusic;
    [SerializeField] private Image[] backgroundImage;
    private int playerScore, computerScore, winScore, indexOfCurrentMusic, indexOfCurrentBackground, roundsWon, roundsLost, gamesWon, gamesLost;
    private bool isPlaying;

    private void Awake()
    {
        pauseScreen.enabled = false;
        winScreen.enabled = false;

        indexOfCurrentMusic = PlayerPrefs.GetInt("Background Music");
        indexOfCurrentBackground = PlayerPrefs.GetInt("Background Image");

        backgroundMusic[indexOfCurrentMusic].Play();
        backgroundImage[indexOfCurrentBackground].enabled = true;
        updateBackgrounds(indexOfCurrentBackground);

        songTitle.text = backgroundMusic[indexOfCurrentMusic].name;
        backgroundName.text = backgroundImage[indexOfCurrentBackground].name;
    }

    private void Start()
    {
        isPlaying = true;

        winScore = 11;
        roundsWon = PlayerPrefs.GetInt("Rounds Won");
        roundsLost = PlayerPrefs.GetInt("Rounds Lost");
        gamesWon = PlayerPrefs.GetInt("Games Won");
        gamesLost = PlayerPrefs.GetInt("Games Lost");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseScreen.isActiveAndEnabled)
        {
            Time.timeScale = 0;
            pauseScreen.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseScreen.isActiveAndEnabled)
        {
            Time.timeScale = 1;
            pauseScreen.enabled = false;
        }
    }

    public void PlayerScores()
    {
        playerScore++;
        playerScoreText.text = playerScore.ToString();
        win.Play();

        roundsWon++;
        PlayerPrefs.SetInt("Rounds Won", roundsWon);

        Reset();

        if (playerScore >= winScore)
        {
            // player 1 wins!
            gamesWon++;
            PlayerPrefs.SetInt("Games Won", gamesWon);

            Time.timeScale = 0;
            winScreen.enabled = true;
            winText.text = "Player 1 wins!";
            scoreText.text = "Player 1 has scored " + playerScoreText.text + " points!";
        }
    }

    public void ComputerScores()
    {
        computerScore++;
        computerScoreText.text = computerScore.ToString();
        lose.Play();

        roundsLost++;
        PlayerPrefs.SetInt("Rounds Lost", roundsLost);

        Reset();

        if (computerScore >= winScore)
        {
            // player 2 wins!
            gamesLost++;
            PlayerPrefs.SetInt("Games Lost", gamesLost);

            Time.timeScale = 0;
            winScreen.enabled = true;
            winText.text = "Player 2 wins!";
            scoreText.text = "Player 2 has scored " + computerScoreText.text + " points!";
        }
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    private void Reset()
    {
        playerPaddle.ResetPosition();
        computerPaddle.ResetPosition();
        ball.ResetPosition();
        ball.AddStartingForce();
    }

    public void PrevSong()
    {
        int index = indexOfCurrentMusic;

        // check if we are at the beginning of the list
        if (index == 0)
        {
            // if we are at the beginning of the list, then go to the last song in the list
            index = backgroundMusic.Length - 1;
            // stops the current song being played (which is the first song)
            backgroundMusic[0].Stop();
        }
        else
        {
            // stops the current song being played
            backgroundMusic[index].Stop();
            index--;
        }

        // plays the prev song
        PlaySong(index);
    }

    public void NextSong()
    {
        int index = indexOfCurrentMusic;

        // check if we are at the end of the list
        if (index == backgroundMusic.Length - 1)
        {
            // if we are at the end of the list, then go to the first song in the list
            index = 0;
            // stops the current song being played (which is the last song)
            backgroundMusic[backgroundMusic.Length - 1].Stop();
        }
        else
        {
            // stops the current song being played
            backgroundMusic[index].Stop();
            index++;
        }

        // plays the next song
        PlaySong(index);
    }

    public void PlayPause()
    {
        if (isPlaying)
        {
            backgroundMusic[indexOfCurrentMusic].Pause();
            playPauseText.text = "Play";
            isPlaying = false;
        }
        else
        {
            backgroundMusic[indexOfCurrentMusic].UnPause();
            playPauseText.text = "Pause";
            isPlaying = true;
        }
    }

    private void PlaySong(int index)
    {
        backgroundMusic[index].Play();
        songTitle.text = backgroundMusic[index].name;
        playPauseText.text = "Pause";
        isPlaying = true;
        indexOfCurrentMusic = index;

        PlayerPrefs.SetInt("Background Music", indexOfCurrentMusic);
    }

    public void PrevBackground()
    {
        int index = indexOfCurrentBackground;

        if (index == 0)
        {
            index = backgroundImage.Length - 1;

            backgroundImage[0].enabled = false;
        }
        else
        {
            backgroundImage[index].enabled = false;
            index--;
        }

        ApplyBackground(index);
    }

    public void NextBackground()
    {
        int index = indexOfCurrentBackground;

        if (index == backgroundImage.Length - 1)
        {
            index = 0;

            backgroundImage[backgroundImage.Length - 1].enabled = false;
        }
        else
        {
            backgroundImage[index].enabled = false;
            index++;
        }

        ApplyBackground(index);
    }

    private void ApplyBackground(int index)
    {
        backgroundImage[index].enabled = true;
        backgroundName.text = backgroundImage[index].name;
        indexOfCurrentBackground = index;

        PlayerPrefs.SetInt("Background Image", indexOfCurrentBackground);
    }

    private void updateBackgrounds(int index)
    {
        for (int i = 0; i < backgroundImage.Length; i++)
        {
            if (i != index)
            {
                backgroundImage[i].enabled = false;
            }
        }
    }
}
