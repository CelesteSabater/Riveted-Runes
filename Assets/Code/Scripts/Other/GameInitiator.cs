using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using RivetedRunes.Managers;
using RivetedRunes.Managers.TimeManager;
using RivetedRunes.Managers.ControlsManager;
using RivetedRunes.UI;
using RivetedRunes.Disposable;
using RivetedRunes.Systems.AudioSystem;
using RivetedRunes.UtilityAI;
using RivetedRunes.UtilityAI.Stats;

namespace RivetedRunes.Other
{
    public class GameInitiator : MonoBehaviour
    {
        [Header("Binder")]
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private GameObject _mainCamera;
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private Light _directionalLight;
        [SerializeField] private GameEvents _gameEvents;

        [Header("Creation")]
        [SerializeField] private AudioSystem _audioSystem;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private ControlsManager _controlsManager;
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _enviorement;
        [SerializeField] private AIBrain _aiBrain;
        [SerializeField] private StatDecayManager _statDecayManager;

        private int _currentStep, _maxSteps;

        private async void Start() {
            _currentStep = 0;
            _maxSteps = 3;
            BindObjects();

            using (LoadingScreenDisposable loadingScreenDisposable = new LoadingScreenDisposable(_loadingScreen))
            {
                await InitializeObjects();
                LoadingScreenStep(loadingScreenDisposable);

                await CreateObjects();
                LoadingScreenStep(loadingScreenDisposable);

                await PrepareGame();
                LoadingScreenStep(loadingScreenDisposable);
            }

            await BeginGame();

            Destroy(gameObject);
        }

        private void BindObjects()
        {
            _eventSystem = Instantiate(_eventSystem);
            _gameEvents = Instantiate(_gameEvents);
            _loadingScreen = Instantiate(_loadingScreen);
            _mainCamera = Instantiate(_mainCamera);
            _directionalLight = Instantiate(_directionalLight);
        }

        private async UniTask InitializeObjects()
        {
            await UniTask.Yield();
        }

        private async UniTask CreateObjects()
        {
            _player = Instantiate(_player);
            _enviorement = Instantiate(_enviorement);
            _audioSystem = Instantiate(_audioSystem);
            _timeManager = Instantiate(_timeManager);
            _controlsManager = Instantiate(_controlsManager);
            _aiBrain = Instantiate(_aiBrain);
            _statDecayManager = Instantiate(_statDecayManager);

            await UniTask.Yield();
        }

        private async UniTask PrepareGame()
        {
            await UniTask.Yield();
        }

        private async UniTask BeginGame()
        {
            _audioSystem.StartMusic();
            _controlsManager.ActivateCameraControls();

            await UniTask.Yield();
        }

        private void LoadingScreenStep(LoadingScreenDisposable loadingScreenDisposable)
        {
            _currentStep++;
            loadingScreenDisposable.UpdateLoadingProgress((float)_currentStep/_maxSteps);
        }
    } 
}