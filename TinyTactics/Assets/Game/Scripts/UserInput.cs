using UnityEngine.InputSystem;
using UnityEngine;
using Photon.Pun;

public class UserInput : MonoBehaviourPun
{
    public static UserInput instance;

    public bool Panning { get; private set; }
    public Vector2 MouseInput { get; private set; }

    private PlayerInput _playerInput;
    private InputAction _panbutton;
    private InputAction _mousedelta;

    private void Awake()
    {
        if (!photonView.IsMine) this.enabled = false;
        if (instance == null) { instance = this; }
        _playerInput = GetComponent<PlayerInput>();
        SetupInputActions();
    }
    public void SetupInputActions()
    {
        _panbutton = _playerInput.actions["PanButton"];
        _mousedelta = _playerInput.actions["Mouse"];
    }
    private void Update()
    {
        Panning = _panbutton.IsPressed();
        MouseInput = _mousedelta.ReadValue<Vector2>();
    }
}
