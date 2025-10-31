using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public bool PauseOpenClose { get; private set; }

    private PlayerInput _playerInput;
    private InputAction _pauseOpenClose;

    private InputAction _cancelAction;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _playerInput = GetComponent<PlayerInput>();
        _pauseOpenClose = _playerInput.actions["PauseOpenClose"];
        _cancelAction = _playerInput.actions["Cancel"]; 
    }

    public bool CancelPressed()
    {
        return _cancelAction != null && _cancelAction.WasPressedThisFrame();
    }


    private void Update()
    {
        PauseOpenClose = _pauseOpenClose.WasPressedThisFrame();
    }

    public void SwitchToGame()
    {
        _playerInput.SwitchCurrentActionMap("Move"); 
    }

    public void SwitchToUI()
    {
        _playerInput.SwitchCurrentActionMap("Actions"); 
    }
}

