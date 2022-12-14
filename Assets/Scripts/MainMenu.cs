using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject gameName;
    [SerializeField] private GameObject mainMenu, playMenu, optionsMenu, soundMenu, customizeMenu, resetScreen, profilePage, howToPlay, credits, loadingScreen;
    [SerializeField] private Text loadingProgressText, roundsWon, roundsLost, gamesWon, gamesLost, pongsText;
    [SerializeField] private Slider loadingProgressBar;

    [Header("Volume")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Text volumeValue;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private float soundLevel;

    [Header("DropDowns")]
    [SerializeField] private Dropdown screenModeDropdown;
    [SerializeField] private Dropdown resolutionDropdown, playerColorDropdown, computerColorDropdown;

    private void Awake()
    {
        gameName.SetActive(true);
        mainMenu.SetActive(true);
        playMenu.SetActive(false);
        optionsMenu.SetActive(false);
        soundMenu.SetActive(false);
        customizeMenu.SetActive(false);
        resetScreen.SetActive(false);
        profilePage.SetActive(false);
        howToPlay.SetActive(false);
        credits.SetActive(false);
        loadingScreen.SetActive(false);

        // enable vsync by default
        QualitySettings.vSyncCount = 1;
    }

    private void Start()
    {
        // check player prefs
        CheckKeys();

        // set the volume
        soundLevel = PlayerPrefs.GetFloat("Volume");
        mixer.SetFloat("MasterVolume", Mathf.Log10(soundLevel) * 20);
        volumeSlider.value = soundLevel;
        volumeValue.text = soundLevel.ToString("p");

        // set the screen mode and resolution
        screenModeDropdown.value = PlayerPrefs.GetInt("Screen Mode");
        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
        playerColorDropdown.value = PlayerPrefs.GetInt("Player Color Dropdown");
        computerColorDropdown.value = PlayerPrefs.GetInt("Computer Color Dropdown");
    }

    private void Update()
    {
        if (credits.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndCredits();
            }
        }
    }

    // loads the scene level asynchronously
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        gameName.SetActive(false);
        playMenu.SetActive(false);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            loadingProgressBar.value = progress;
            loadingProgressText.text = progress.ToString("p");
            yield return null;
        }
    }

    public void PlayMenu()
    {
        mainMenu.SetActive(false);
        playMenu.SetActive(true);
    }

    public void PlayMenuBack()
    {
        mainMenu.SetActive(true);
        playMenu.SetActive(false);
    }

    public void Options()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void Quit()
    {
        PlayerPrefs.Save();
        // quits the game
        Application.Quit();
    }

    public void Profile()
    {
        mainMenu.SetActive(false);
        gameName.SetActive(false);
        profilePage.SetActive(true);

        // update the profile at start
        roundsWon.text = "Rounds Won: " + PlayerPrefs.GetInt("Rounds Won");
        roundsLost.text = "Rounds Lost: " + PlayerPrefs.GetInt("Rounds Lost");
        gamesWon.text = "Games Won: " + PlayerPrefs.GetInt("Games Won");
        gamesLost.text = "Games Lost: " + PlayerPrefs.GetInt("Games Lost");
        pongsText.text = "# of Pongs: " + PlayerPrefs.GetInt("Pongs");
    }

    public void ProfileBack()
    {
        mainMenu.SetActive(true);
        gameName.SetActive(true);
        profilePage.SetActive(false);
    }

    public void HowToPlay()
    {
        mainMenu.SetActive(false);
        gameName.SetActive(false);
        howToPlay.SetActive(true);
    }

    public void HowToPlayBack()
    {
        mainMenu.SetActive(true);
        gameName.SetActive(true);
        howToPlay.SetActive(false);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        gameName.SetActive(false);
        credits.SetActive(true);
    }

    public void EndCredits()
    {
        mainMenu.SetActive(true);
        gameName.SetActive(true);
        credits.SetActive(false);
    }

    public void Sound()
    {
        optionsMenu.SetActive(false);
        soundMenu.SetActive(true);
    }

    public void ResetSound()
    {
        // reset sound
        mixer.SetFloat("MasterVolume", 0.0f);
        volumeSlider.value = 1;
        PlayerPrefs.SetFloat("Volume", 1.0f);
    }

    public void ApplySound()
    {
        // apply sound
        // it will change the mixer between -80 decibels to 0 decibels
        mixer.SetFloat("MasterVolume", Mathf.Log10(soundLevel) * 20);
        PlayerPrefs.SetFloat("Volume", soundLevel);
    }

    public void SoundBack()
    {
        optionsMenu.SetActive(true);
        soundMenu.SetActive(false);
    }

    public void OptionsBack()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void SetVolume(float sliderValue)
    {
        soundLevel = sliderValue;

        if (soundLevel < 0.01)
        {
            volumeValue.text = "0.00 %";
        }
        else
        {
            volumeValue.text = soundLevel.ToString("p");
        }
    }

    public void Customize()
    {
        optionsMenu.SetActive(false);
        customizeMenu.SetActive(true);
    }

    public void ResetCustomizations()
    {
        Screen.SetResolution(1920, 1080, true);

        screenModeDropdown.value = 0;
        resolutionDropdown.value = 0;
        playerColorDropdown.value = 0;
        computerColorDropdown.value = 0;

        PlayerPrefs.SetInt("Screen Mode", screenModeDropdown.value);
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("Player Color Dropdown", playerColorDropdown.value);
        PlayerPrefs.SetInt("Computer Color Dropdown", computerColorDropdown.value);
        PlayerPrefs.SetString("Player Color", "#ffffff");
        PlayerPrefs.SetString("Computer Color", "#ffffff");
    }

    public void ApplyCustomizations()
    {
        // screen mode and resolution
        switch (screenModeDropdown.value)
        {
            case 0:
                switch (resolutionDropdown.value)
                {
                    case 0:
                        Screen.SetResolution(1920, 1080, true);
                        break;
                    case 1:
                        Screen.SetResolution(1280, 720, true);
                        break;
                }

                break;
            case 1:
                switch (resolutionDropdown.value)
                {
                    case 0:
                        Screen.SetResolution(1920, 1080, false);
                        break;
                    case 1:
                        Screen.SetResolution(1280, 720, false);
                        break;
                }

                break;
        }

        // player paddle color (in hex)
        switch (playerColorDropdown.value)
        {
            case 0: // white
                PlayerPrefs.SetString("Player Color", "#ffffff");
                break;
            case 1: // red
                PlayerPrefs.SetString("Player Color", "#ff3b3b");
                break;
            case 2: // blue
                PlayerPrefs.SetString("Player Color", "#47b3ff");
                break;
            case 3: // green
                PlayerPrefs.SetString("Player Color", "#3bd163");
                break;
            case 4: // yellow
                PlayerPrefs.SetString("Player Color", "#f0ff47");
                break;
            case 5: // pink
                PlayerPrefs.SetString("Player Color", "#ff75af");
                break;
        }

        // computer paddle color (in hex)
        switch (computerColorDropdown.value)
        {
            case 0: // white
                PlayerPrefs.SetString("Computer Color", "#ffffff");
                break;
            case 1: // red
                PlayerPrefs.SetString("Computer Color", "#ff3b3b");
                break;
            case 2: // blue
                PlayerPrefs.SetString("Computer Color", "#47b3ff");
                break;
            case 3: // green
                PlayerPrefs.SetString("Computer Color", "#3bd163");
                break;
            case 4: // yellow
                PlayerPrefs.SetString("Computer Color", "#f0ff47");
                break;
            case 5: // pink
                PlayerPrefs.SetString("Computer Color", "#ff75af");
                break;
        }

        PlayerPrefs.SetInt("Screen Mode", screenModeDropdown.value);
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("Player Color Dropdown", playerColorDropdown.value);
        PlayerPrefs.SetInt("Computer Color Dropdown", computerColorDropdown.value);
    }

    public void CustomizeBack()
    {
        optionsMenu.SetActive(true);
        customizeMenu.SetActive(false);
    }

    public void ResetScreen()
    {
        gameName.SetActive(false);
        optionsMenu.SetActive(false);
        resetScreen.SetActive(true);
    }

    public void ResetBack()
    {
        gameName.SetActive(true);
        optionsMenu.SetActive(true);
        resetScreen.SetActive(false);
    }

    private void CheckKeys()
    {
        if (!PlayerPrefs.HasKey("Background Music"))
        {
            PlayerPrefs.SetInt("Background Music", 0);
        }

        if (!PlayerPrefs.HasKey("Background Image"))
        {
            PlayerPrefs.SetInt("Background Image", 0);
        }

        if (!PlayerPrefs.HasKey("Rounds Won"))
        {
            PlayerPrefs.SetInt("Rounds Won", 0);
        }

        if (!PlayerPrefs.HasKey("Rounds Lost"))
        {
            PlayerPrefs.SetInt("Rounds Lost", 0);
        }

        if (!PlayerPrefs.HasKey("Games Won"))
        {
            PlayerPrefs.SetInt("Games Won", 0);
        }

        if (!PlayerPrefs.HasKey("Games Lost"))
        {
            PlayerPrefs.SetInt("Games Lost", 0);
        }

        if (!PlayerPrefs.HasKey("Pongs"))
        {
            PlayerPrefs.SetInt("Pongs", 0);
        }
    }

    public void ResetAllData()
    {
        // delete saved keys
        PlayerPrefs.DeleteAll();
        resetScreen.SetActive(false);
        mainMenu.SetActive(true);
        gameName.SetActive(true);

        // set the keys to initial values
        ResetSound();
        ResetCustomizations();
        CheckKeys();
    }
}
