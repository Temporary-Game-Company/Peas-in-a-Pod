using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private string mainMenu;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickClip;
    
    public void NextButton()
    {
        SceneManager.LoadScene(mainMenu);
    }
    
    public void PlayClick()
    {
        audioSource.PlayOneShot(clickClip);
    }
}
