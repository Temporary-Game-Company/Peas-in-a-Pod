using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string gameLevel;
    //[SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickClip;
    
   public void PlayButton()
   {
        SceneManager.LoadScene(gameLevel);
   }

   public void QuitButton()
   {
        Application.Quit();
   }

   public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(clickClip);
    }
}
