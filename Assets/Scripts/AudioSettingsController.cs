using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsController : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer masterMixer;

    [Header("UI Components")]
    public Slider volumeSlider;
    public Toggle musicToggle;

    private float currentVolume = 0.75f;
    private bool isMusicOn = true;

    void Start()
    {
        // Carica eventuali impostazioni salvate (opzionale)
        LoadSettings();

        // Collega gli eventi
        volumeSlider.onValueChanged.AddListener(SetVolume);
        musicToggle.onValueChanged.AddListener(ToggleMusic);

        // Imposta i valori iniziali nell'UI
        volumeSlider.value = currentVolume;
        musicToggle.isOn = isMusicOn;

        // Applica il volume iniziale
        SetVolume(currentVolume);
    }

    void SetVolume(float volume)
    {
        currentVolume = volume;

        // Converte il valore lineare (0-1) in decibel per l'AudioMixer
        // Il log10(0.0001) * 20 = -80dB (silenzio)
        // Il log10(1) * 20 = 0dB (volume massimo)
        float dbVolume = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;
        masterMixer.SetFloat("MusicVolume", dbVolume);
    }

    void ToggleMusic(bool isOn)
    {
        isMusicOn = isOn;

        if (isOn)
        {
            // Ripristina il volume salvato
            SetVolume(currentVolume);
        }
        else
        {
            // Silenzia (-80dB = praticamente muto)
            masterMixer.SetFloat("MusicVolume", -80f);
        }
    }

    void LoadSettings()
    {
        // Opzionale: carica le impostazioni salvate tra una sessione e l'altra
        if (PlayerPrefs.HasKey("MusicVolume"))
            currentVolume = PlayerPrefs.GetFloat("MusicVolume");
        if (PlayerPrefs.HasKey("MusicOn"))
            isMusicOn = PlayerPrefs.GetInt("MusicOn") == 1;
    }

    void OnDestroy()
    {
        // Opzionale: salva le impostazioni
        PlayerPrefs.SetFloat("MusicVolume", currentVolume);
        PlayerPrefs.SetInt("MusicOn", isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}