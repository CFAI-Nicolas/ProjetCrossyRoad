using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBindingManager : MonoBehaviour
{
    public static KeyBindingManager Instance;

    public Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();

    public TextMeshProUGUI forwardText; // touche avant
    public TextMeshProUGUI backwardText; // touche arrière
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;

    private string keyToRebind;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadBindings();
    }

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(keyToRebind) && Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode) && keyCode != KeyCode.Escape)
                {
                    keyBindings[keyToRebind] = keyCode;
                    keyToRebind = null;
                    SaveBindings();
                    UpdateUI();
                    break;
                }
            }
        }
    }

    public void StartRebind(string keyName)
    {
        keyToRebind = keyName;
    }

    void SaveBindings()
    {
        foreach (var binding in keyBindings)
        {
            PlayerPrefs.SetString(binding.Key, binding.Value.ToString());
        }
    }

    void LoadBindings()
    {
        keyBindings["Forward"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Forward", "W"));
        keyBindings["Backward"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Backward", "S"));
        keyBindings["Left"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A"));
        keyBindings["Right"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D"));
    }

    void UpdateUI()
    {
        forwardText.text = keyBindings["Forward"].ToString();
        backwardText.text = keyBindings["Backward"].ToString();
        leftText.text = keyBindings["Left"].ToString();
        rightText.text = keyBindings["Right"].ToString();
    }
}
