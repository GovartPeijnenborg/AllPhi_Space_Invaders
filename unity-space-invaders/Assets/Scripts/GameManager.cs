using UnityEngine;
using UnityEngine.UI;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _livesText;

    private Player _player;
    private Invaders _invaders;
    private MysteryShip _mysteryShip;
    private Bunker[] _bunkers;

    private int _score;
    private int _lives;

    public int Score => _score;
    public int Lives => _lives;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _invaders = FindObjectOfType<Invaders>();
        _mysteryShip = FindObjectOfType<MysteryShip>();
        _bunkers = FindObjectsOfType<Bunker>();

        NewGame();
    }

    private void Update()
    {
        if (_lives <= 0 && Input.GetKeyDown(KeyCode.Return)) {
            NewGame();
        }
    }

    private void NewGame()
    {
        _gameOverUI.SetActive(false);

        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        _invaders.ResetInvaders();
        _invaders.gameObject.SetActive(true);

        for (int i = 0; i < _bunkers.Length; i++) {
            _bunkers[i].ResetBunker();
        }

        Respawn();
    }

    private void Respawn()
    {
        Vector3 position = _player.transform.position;
        position.x = 0f;
        _player.transform.position = position;
        _player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        _gameOverUI.SetActive(true);
        _invaders.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this._score = score;
        _scoreText.text = score.ToString().PadLeft(4, '0');
    }

    private void SetLives(int lives)
    {
        this._lives = Mathf.Max(lives, 0);
        _livesText.text = this._lives.ToString();
    }

    public void OnPlayerKilled(Player player)
    {
        SetLives(_lives - 1);

        player.gameObject.SetActive(false);

        if (_lives > 0) {
            Invoke(nameof(NewRound), 1f);
        } else {
            GameOver();
        }
    }

    public void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);

        SetScore(_score + invader.score);

        if (_invaders.GetAliveCount() == 0) {
            NewRound();
        }
    }

    public void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        SetScore(_score + mysteryShip.Score);
    }

    public void OnBoundaryReached()
    {
        if (_invaders.gameObject.activeSelf)
        {
            _invaders.gameObject.SetActive(false);

            OnPlayerKilled(_player);
        }
    }

}
