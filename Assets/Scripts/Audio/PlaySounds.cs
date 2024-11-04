using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PlaySounds : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundEffectsSource;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;

    [Header("Sound Clips")]
    [SerializeField] private Button _buttonMusicOnOff;
    [SerializeField] private Button _buttonSoundFXOnOff;
    [SerializeField] private AudioClip _musicClip;
    [SerializeField] private AudioClip _footWalkingClip;
    [SerializeField] private AudioClip _footWalkDirtClip;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _mineActiveClip;
    [SerializeField] private AudioClip _explosionClip;
    [SerializeField] private Color _onColorButton;
    [SerializeField] private Color _offColorButton;

    private AudioHandler _audioHandler;

    private void Awake()
    {
        _audioHandler = new AudioHandler(_audioMixerGroup);
    }

    private void Start()
    {
        _audioHandler.Initialize();
        OnPlayMusicClip(_musicClip);

        _buttonMusicOnOff.onClick.AddListener(OnOffMusic);
        _buttonSoundFXOnOff.onClick.AddListener(OnOffSoundFX);
       
        if (_audioHandler.IsSoundFXOn())        
            _buttonSoundFXOnOff.GetComponent<Image>().color = _onColorButton;
        else     
            _buttonSoundFXOnOff.GetComponent<Image>().color = _offColorButton;

        if (_audioHandler.IsMusicOn())       
            _buttonMusicOnOff.GetComponent<Image>().color = _onColorButton;      
        else       
            _buttonMusicOnOff.GetComponent<Image>().color = _offColorButton;
    }

    private void OnDestroy()
    {
        _buttonMusicOnOff.onClick.RemoveListener(OnOffMusic);
        _buttonSoundFXOnOff.onClick.RemoveListener (OnOffSoundFX);
    }

    private void OnOffSoundFX()
    {
        if(_audioHandler.IsSoundFXOn())
        {
            _audioHandler.OffSoundFX();
            _buttonSoundFXOnOff.GetComponent<Image>().color = _offColorButton;
        }
        else
        {
            _audioHandler?.OnSoundFX();
            _buttonSoundFXOnOff.GetComponent<Image>().color = _onColorButton;
        }
    }

    private void OnOffMusic()
    {
        if(_audioHandler.IsMusicOn())
        {
            _audioHandler?.OffMusic();
            _buttonMusicOnOff.GetComponent<Image>().color = _offColorButton;
        }
        else
        {
            _audioHandler?.OnMusic();
            _buttonMusicOnOff.GetComponent<Image>().color = _onColorButton;
        }
    }

    public void OnFootSoundClip()
    {
        _soundEffectsSource.volume = 0.4f;
        _soundEffectsSource.loop = true;  
        
        OnPlaySoundFX(_footWalkingClip);       
    }

    public void OnFootDirtSoundClip()
    {
        _soundEffectsSource.volume = 0.4f;        
        _soundEffectsSource.loop = true;

        OnPlaySoundFX(_footWalkDirtClip);
    }

    public void OffFootSoundClip()
    {
        _soundEffectsSource.volume = 1;
        _soundEffectsSource.loop = false;
        OffPlaySoundFX();
    }

    public void OnJumpSoundClip()
    {        
        _soundEffectsSource.PlayOneShot(_jumpClip);
    }

    public void OffJumpSoundClip()
    {
        OffPlaySoundFX();
    }

    public void OnSoundExplosion()
    {
        _soundEffectsSource.PlayOneShot(_explosionClip);
    }

    public void OnSoundMineActive()
    {
        _soundEffectsSource.PlayOneShot(_mineActiveClip);
    }

    private void OnPlaySoundFX(AudioClip audioClip)
    {
        _soundEffectsSource.clip = audioClip;
        _soundEffectsSource.Play();
        //AudioSource.PlayClipAtPoint(audioClip, position);
    }

    private void OffPlaySoundFX()
    {
        _soundEffectsSource.Stop();
    }

    private void OnPlayMusicClip(AudioClip audioClip)
    {
        _musicSource.clip = audioClip;
        _musicSource.Play();
    }
}
