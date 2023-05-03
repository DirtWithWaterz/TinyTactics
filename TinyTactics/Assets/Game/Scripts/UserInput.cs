using UnityEngine.InputSystem;
using UnityEngine;
using Photon.Pun;

public class UserInput : MonoBehaviourPun
{
    public static UserInput instance;

    public bool Panning { get; private set; }

    public float MouseWheel { get; private set; }
    public Vector3 MousePosition { get; private set; }
    public Vector2 MouseInput { get; private set; }
    public bool Interact { get; private set; }

    [SerializeField] private Camera cam;
    private PlayerInput _playerInput;
    private InputAction _panbutton;
    private InputAction _mouseInput;
    private InputAction _mousepos;
    private InputAction _interactbutton;

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
        _interactbutton = _playerInput.actions["Interact"];
        _mouseInput = _playerInput.actions["MouseInput"];
        _mousepos = _playerInput.actions["MousePos"];
    }
    private void Update()
    {  
        Panning = _panbutton.IsPressed();
        Interact = _interactbutton.WasPressedThisFrame();
        MouseInput = _mouseInput.ReadValue<Vector2>();
        Vector2 rawMousePos = _mousepos.ReadValue<Vector2>();
        MousePosition = cam.ScreenToWorldPoint(new Vector3(rawMousePos.x, rawMousePos.y, 0));
    }
}
