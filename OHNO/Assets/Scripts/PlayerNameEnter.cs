using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameEnter : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button continueButton;
    
    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";
    
    private void Start()
    {
        SetUpInputField();
    }

    private void SetUpInputField()
    {
        if(!PlayerPrefs.HasKey(PlayerPrefsNameKey))
        {
            return;
        }
        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        ValidatePlayerName(defaultName);
    }

    public void ValidatePlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;
        
        PlayerPrefs.SetString(PlayerPrefsNameKey,DisplayName);
    }
}
