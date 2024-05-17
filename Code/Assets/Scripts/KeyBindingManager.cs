using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBindingManager : MonoBehaviour
{
    public static KeyBindingManager Instance;

    public Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    private Dictionary<string, KeyCode> defaultKeyBindings = new Dictionary<string, KeyCode>();

    public TextMeshProUGUI forwardText; // touche avant
    public TextMeshProUGUI backwardText; // touche arrière
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;

    public GameObject rebindPanel; // Panneau qui apparaît pendant la réassignation

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

        SetDefaultBindings();
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
                    RebindKey(keyToRebind, keyCode);
                    keyToRebind = null;
                    SaveBindings();
                    UpdateUI();
                    rebindPanel.SetActive(false); // Masquer le panneau de réassignation
                    EnableUI(true); // Réactiver les éléments UI interactifs
                    break;
                }
            }
        }
    }

    public void StartRebind(string keyName)
    {
        keyToRebind = keyName;
        rebindPanel.SetActive(true); // Afficher le panneau de réassignation
        EnableUI(false); // Désactiver les éléments UI interactifs
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

    void SetDefaultBindings()
    {
        defaultKeyBindings["Forward"] = KeyCode.W;
        defaultKeyBindings["Backward"] = KeyCode.S;
        defaultKeyBindings["Left"] = KeyCode.A;
        defaultKeyBindings["Right"] = KeyCode.D;

        // Initialiser keyBindings avec les valeurs par défaut
        foreach (var binding in defaultKeyBindings)
        {
            if (!keyBindings.ContainsKey(binding.Key))
            {
                keyBindings[binding.Key] = binding.Value;
            }
        }
    }

    void UpdateUI()
    {
        forwardText.text = keyBindings["Forward"].ToString();
        backwardText.text = keyBindings["Backward"].ToString();
        leftText.text = keyBindings["Left"].ToString();
        rightText.text = keyBindings["Right"].ToString();
    }

    void RebindKey(string keyName, KeyCode newKeyCode)
    {
        // Vérifiez si la nouvelle touche est déjà assignée à une autre action
        string existingKey = null;
        foreach (var binding in keyBindings)
        {
            if (binding.Value == newKeyCode)
            {
                existingKey = binding.Key;
                break;
            }
        }

        if (existingKey != null)
        {
            // Réinitialiser l'autre action à sa touche par défaut
            keyBindings[existingKey] = defaultKeyBindings[existingKey];
        }

        // Assigner la nouvelle touche
        keyBindings[keyName] = newKeyCode;
    }

    void EnableUI(bool enable)
    {
        // Activer ou désactiver tous les éléments UI interactifs sauf le panneau de réassignation
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (var button in buttons)
        {
            if (button.gameObject != rebindPanel)
            {
                button.interactable = enable;
            }
        }

        TMP_InputField[] inputFields = FindObjectsOfType<TMP_InputField>();
        foreach (var inputField in inputFields)
        {
            inputField.interactable = enable;
        }
    }
}
