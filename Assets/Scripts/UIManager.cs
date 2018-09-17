using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private UnlockLevel _UL;
    private MapManager _map;
    private SoundManager _sound;
    private Image _BG;
    private GameObject _mainMenu;
    private GameObject _levelMenu;
    private GameObject _ingame;
    private GameObject _pauseGame;
    private GameObject _gameClear;
    private GameObject _earnedStarSet;
    private Button _btnPlay;
    private Button _btnMoreGame;
    private Button _btnExit;
    private Button _btnPause;
    private Button _btnBack;
    private Button _btnMusic;
    private Button _btnReplayPG;
    private Button _btnHomePG;
    private Button _btnNext;
    private Button _btnReplayGC;
    private Button _btnToLevel;
    private Button _btnHomeGC;
    public Button[] _btnLevel;
    private Dictionary<Music, Sprite> _dicMusic = new Dictionary<Music, Sprite>();

    void Start()
    {
        _UL = GameObject.FindWithTag("UnlockLvl").GetComponent<UnlockLevel>();
        _map = GameObject.FindWithTag("Map").GetComponent<MapManager>();
        _sound = GameObject.FindWithTag("Sound").GetComponent<SoundManager>();
        _BG = transform.Find("BG").GetComponent<Image>();
        _mainMenu = transform.Find("MainMenu").gameObject;
        _levelMenu = transform.Find("LevelMenu").gameObject;
        _ingame = transform.Find("Ingame").gameObject;
        _pauseGame = transform.Find("PauseGame").gameObject;
        _gameClear = transform.Find("GameClear").gameObject;
        _earnedStarSet = _gameClear.transform.Find("panel/StarSet").gameObject;
        AddSprite();
        CreateBtn();
        CreateBtnLvl();
        ClickBtnLvl();
    }

    void AddSprite()
    {
        foreach (Music music in Enum.GetValues(typeof(Music)))
            _dicMusic.Add(music, Resources.Load<Sprite>("Images/UI/btnMusic" + music.ToString().ToLower()));
    }

    void CreateBtn()
    {
        _btnPlay = _mainMenu.transform.Find("btnPlay").GetComponent<Button>();
        _btnMoreGame = _mainMenu.transform.Find("btnMoreGame").GetComponent<Button>();
        _btnExit = _mainMenu.transform.Find("btnExit").GetComponent<Button>();
        _btnPause = _ingame.transform.Find("btnPause").GetComponent<Button>();
        _btnBack = _pauseGame.transform.Find("Panel/btnBack").GetComponent<Button>();
        _btnMusic = _pauseGame.transform.Find("Panel/btnMusic").GetComponent<Button>();
        _btnReplayPG = _pauseGame.transform.Find("Panel/btnReplay").GetComponent<Button>();
        _btnHomePG = _pauseGame.transform.Find("Panel/btnHome").GetComponent<Button>();
        _btnNext = _gameClear.transform.Find("btnNext").GetComponent<Button>();
        _btnReplayGC = _gameClear.transform.Find("btnReplay").GetComponent<Button>();
        _btnToLevel = _gameClear.transform.Find("btnLevel").GetComponent<Button>();
        _btnHomeGC = _gameClear.transform.Find("btnHome").GetComponent<Button>();
        ClickBtn();
    }

    void ClickBtn()
    {
        _btnPlay.onClick.AddListener(Play);
        _btnExit.onClick.AddListener(Exit);
        _btnPause.onClick.AddListener(Pause);
        _btnBack.onClick.AddListener(BackToGame);
        _btnReplayPG.onClick.AddListener(Replay);
        _btnHomePG.onClick.AddListener(Home);
        _btnMusic.onClick.AddListener(MusicSetting);
        _btnReplayGC.onClick.AddListener(Replay);
        _btnNext.onClick.AddListener(NextLvl);
        _btnToLevel.onClick.AddListener(ToLvlMenu);
        _btnHomeGC.onClick.AddListener(Home);
        _btnMoreGame.onClick.AddListener(MoreGame);
    }

    void Active(GameObject turnOn, GameObject turnOff)
    {
        turnOn.SetActive(true);
        turnOff.SetActive(false);
    }

    void Clicked()
    {
        _sound.Play(SoundManager.Sound.Click);
    }

    void Play()
    {
        Active(_levelMenu, _mainMenu);
        _UL.StarEarned();
        Clicked();
    }

    void CreateBtnLvl()
    {
        _btnLevel = new Button[_UL.level];
        for (int i = 0; i < _UL.level; i++)
        {
            _btnLevel[i] = transform.Find("LevelMenu/Tab1/btnLvl_" + (i + 1).ToString())
                .GetComponent<Button>();
            _UL.GetLock(i, _btnLevel[i]);
        }
        _UL.Unlock();
    }

    void ClickBtnLvl()
    {
        for (int i = 0; i < _btnLevel.Length; i++)
            _btnLevel[i].onClick.AddListener(ChooseLevel);
    }

    void ChooseLevel()
    {
        Clicked();
        Time.timeScale = 1;
        string btnName = EventSystem.current.currentSelectedGameObject.name;
        _UL.presentLvl = int.Parse(btnName.Substring(btnName.LastIndexOf("_") + 1));
        if (_UL.presentLvl <= _UL.highestLvl)
        {
            _sound.Play(SoundManager.Sound.Main);
            _map.CreateMap();
            Active(_ingame, _levelMenu);
            _BG.gameObject.SetActive(false);
        }
    }

    void Exit()
    {
        Clicked();
        Application.Quit();
    }

    void Pause()
    {
        Clicked();
        Time.timeScale = 0;
        Active(_pauseGame, _ingame);
        _sound.StopLoop();
    }

    void BackToGame()
    {
        Clicked();
        _sound.Play(SoundManager.Sound.Main);
        Time.timeScale = 1;
        Active(_ingame, _pauseGame);
        _gameClear.SetActive(false);
    }

    void DeactiveStar()
    {
        foreach (Transform child in _earnedStarSet.transform)
            child.gameObject.SetActive(false);
    }

    void Replay()
    {
        BackToGame();
        _map.DeactiveAll();
        _map.CreateMap();
        DeactiveStar();
    }

    void Home()
    {
        Clicked();
        Time.timeScale = 1;
        _map.DeactiveAll();
        Active(_mainMenu, _pauseGame);
        _ingame.SetActive(false);
        _gameClear.SetActive(false);
        _BG.gameObject.SetActive(true);
        DeactiveStar();
    }

    void MusicSetting(float volume, Music music)
    {
        _btnMusic.GetComponent<Image>().sprite = _dicMusic[music];
        AudioListener.volume = volume;
    }

    void MusicSetting()
    {
        Clicked();
        if (AudioListener.volume == 1)
            MusicSetting(0, Music.Off);
        else MusicSetting(1, Music.On);
    }

    public void GameClear(Player player)
    {
        Active(_gameClear, _ingame);
        for (int i = 0; i < player.starEarned; i++)
            _earnedStarSet.transform.GetChild(i).gameObject.SetActive(true);
    }

    void NextLvl()
    {
        Clicked();
        _sound.Play(SoundManager.Sound.Main);
        Time.timeScale = 1;
        _UL.presentLvl += 1;
        _map.DeactiveAll();
        _map.CreateMap();
        DeactiveStar();
        Active(_ingame, _gameClear);
    }

    void ToLvlMenu()
    {
        Clicked();
        _map.DeactiveAll();
        Active(_levelMenu, _gameClear);
        _BG.gameObject.SetActive(true);
        DeactiveStar();
    }

    void MoreGame()
    {
        Clicked();
#if UNITY_ANDROID
        //Application.OpenURL("market://details?id=Polaris285");
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Polaris285");
#endif
    }

    enum Music
    {
        On, Off
    }
}
