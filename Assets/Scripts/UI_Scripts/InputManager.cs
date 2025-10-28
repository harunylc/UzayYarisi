// using UnityEngine;
// using UnityEngine.InputSystem;
//
// public class InputManager : MonoBehaviour
// {
//     public static InputManager instance;
//     
//     public bool PauseOpenClose {get; private set;}
//
//     private PlayerInputActions _playerInputActions;
//
//     private InputAction _pauseOpenClose;
//
//     private void Awake()
//     {
//         
//         if (instance == null)
//         {
//             instance = this;
//         }
//         
//         // _playerInputActions = new PlayerInputActions();
//         // _playerInputActions.Actions.Enable(); 
//         _playerInputActions = GetComponent<PlayerInput>();
//         _pauseOpenClose = _playerInputActions.actions["PauseOpenClose"];
//         _pauseOpenClose = _playerInputActions.Actions.PauseOpenClose;
//     }
//
//     private void Update()
//     {
//         PauseOpenClose = _pauseOpenClose.WasPressedThisFrame();
//     }
//
// }
// using UnityEngine;
// using UnityEngine.InputSystem;
//
// public class InputManager : MonoBehaviour
// {
//     public static InputManager instance;
//
//     public bool PauseOpenClose { get; private set; }
//
//     private PlayerInput _playerInput; 
//     private InputAction _pauseOpenClose;
//
//     private void Awake()
//     {
//         if (instance == null)
//             instance = this;
//
//         _playerInput = GetComponent<PlayerInput>(); 
//         _pauseOpenClose = _playerInput.actions["PauseOpenClose"];
//     }
//
//     private void Update()
//     {
//         PauseOpenClose = _pauseOpenClose.WasPressedThisFrame();
//     }
// }
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

    // 🔹 Map değiştirici fonksiyonlar
    public void SwitchToGame()
    {
        _playerInput.SwitchCurrentActionMap("Move"); // ✅ senin oyun haritan
    }

    public void SwitchToUI()
    {
        _playerInput.SwitchCurrentActionMap("Actions"); // ✅ senin UI haritan
    }
}

