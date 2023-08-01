using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void ButtonHost_Clicked()
    {
        NetworkManager.Singleton.StartHost();
        SceneManager.LoadScene(1);
    }
    
    public void ButtonClient_Clicked()
    {
        NetworkManager.Singleton.StartClient();
        SceneManager.LoadScene(1);
    }
        
    public void ButtonStartGame_Clicked()
    {
        SceneManager.LoadScene(1);
    }
    
    public void ButtonExitGame_Clicked()
    {
        Application.Quit();
    }
}
