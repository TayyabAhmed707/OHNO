
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerOhno networkManager;
    [Header("UI")] 
    [SerializeField] private GameObject landingPagePanel;

    [SerializeField] private TMP_InputField ipAddressInputField;
    [SerializeField] private Button joinButton;

    private void OnEnable()
    {
        NetworkManagerOhno.OnClientConnected += HandleClientConnected;
        NetworkManagerOhno.OnClientDisconnected += HandleClientDisconnected;

    }
    private void OnDisable()
    {
        NetworkManagerOhno.OnClientConnected -= HandleClientConnected;
        NetworkManagerOhno.OnClientDisconnected -= HandleClientDisconnected;

    }

    public void JoinLobby()
    {
        string ipAddress = ipAddressInputField.text;
        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;
        
        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }
    
    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
    
}
