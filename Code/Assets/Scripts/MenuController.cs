using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionMenu;
    public GameObject playerMenu;
    public GameObject musicMenu;
    public GameObject touchesMenu;
    public GameObject quitMenu; // test

    public Button forwardButton;
    public Button backwardButton;
    public Button leftButton;
    public Button rightButton;
    public Button returnButton;
    public Button jouerButton;
    public Button quitButton;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        forwardButton.onClick.AddListener(() => StartRebind("Forward"));
        backwardButton.onClick.AddListener(() => StartRebind("Backward"));
        leftButton.onClick.AddListener(() => StartRebind("Left"));
        rightButton.onClick.AddListener(() => StartRebind("Right"));
        returnButton.onClick.AddListener(ReturnToOptions);

        PlayMusic();
    }

    void StartRebind(string keyName)
    {
        KeyBindingManager.Instance.StartRebind(keyName);
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
        playerMenu.SetActive(false);
        musicMenu.SetActive(false);
        touchesMenu.SetActive(false);
    }

    public void ShowOptionMenu()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
        playerMenu.SetActive(false);
        musicMenu.SetActive(false);
        touchesMenu.SetActive(false);
    }

    public void ShowPlayerMenu()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(false);
        playerMenu.SetActive(true);
        musicMenu.SetActive(false);
        touchesMenu.SetActive(false);
    }

    public void ShowMusicMenu()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(false);
        playerMenu.SetActive(false);
        musicMenu.SetActive(true);
        touchesMenu.SetActive(false);
    }

    public void ShowTouchesMenu()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(false);
        playerMenu.SetActive(false);
        musicMenu.SetActive(false);
        touchesMenu.SetActive(true);
    }

    public void ReturnToOptions()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
        playerMenu.SetActive(false);
        musicMenu.SetActive(false);
        touchesMenu.SetActive(false);
    }

    public void PlayMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
