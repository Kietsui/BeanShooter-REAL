using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class mmPlay : MonoBehaviour
{
    public NetworkManager networkManager;
    public GameObject menuPanel;
    public void Host()
    {
        menuPanel.SetActive(false);
        networkManager.StartHost();
    }

    public void SetIp(string ip)
    {
        networkManager.networkAddress = ip;
    }

    public void Join()
    {
        menuPanel.SetActive(false);
        networkManager.StartClient();
    }

    private void Start() {
        menuPanel.SetActive(true);
    }
}
