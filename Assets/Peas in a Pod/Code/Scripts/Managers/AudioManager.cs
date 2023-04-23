using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TemporaryGameCompany;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource AudioSource;
    [SerializeField] List<AudioClip> BackgroundMusic;

    [SerializeField] AudioManagerSet Set;

    private int _repeatsInGame = 0;
    private int _currentTrack = 0;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        AudioSource.loop = false;
        AudioSource.clip = BackgroundMusic[_currentTrack];
        Loop();
    }

    public void PlayClip(AudioClip clip)
    {
        AudioSource.PlayOneShot(clip);
    }

    async private void Loop()
    {
        if (this == null) return;

        AudioSource.Play();
        await Task.Delay((int) AudioSource.clip.length*1000 + 100);
        
        if (!(SceneManager.GetActiveScene().name == "Menu"))
        {
            _repeatsInGame++;
            if (_repeatsInGame >= _currentTrack*2+2 && BackgroundMusic.Count > _currentTrack+1)
            {
                _currentTrack++;
            }
        }

        AudioSource.clip = BackgroundMusic[_currentTrack];
        Loop();

    }

    // Ensure no other audio manager is in use any longer.
    private void OnEnable()
    {
        while (Set.Get() != null)
        {
            Set.Get().OnDisable();
        }
        Set.Add(this);
    }

    private void OnDisable()
    {
        Set.Remove(this);
        Destroy(gameObject);
    }
}
