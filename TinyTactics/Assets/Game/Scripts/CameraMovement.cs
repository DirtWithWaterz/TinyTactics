using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviourPun
{
    public bool isPanning;
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float _speed;
    [SerializeField] private float _drag;
    [SerializeField] private float _dampAmount;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;
    private float targetOrthoSize;
    Camera cam;
    private Vector2 currentVelocity;
    private Vector2 remainingVelocity;
    private bool inputReleased;

    private void Awake(){
        if(!photonView.IsMine){
            GetComponent<Camera>().enabled = false;
            GetComponent<AudioListener>().enabled = false;
        }
        cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) { return; }

        float scrollValue = Mouse.current.scroll.ReadValue().y;
        targetOrthoSize -= scrollValue * zoomSpeed;
        targetOrthoSize = Mathf.Clamp(targetOrthoSize, 1f, 4.5f);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetOrthoSize, zoomSpeed);
        if (UserInput.instance.Panning)
        {
            isPanning = true;
            inputReleased = false;
        }
        else if (!UserInput.instance.Panning && currentVelocity != Vector2.zero)
        {
            isPanning = true;
            inputReleased = true;
        }
        else
        {
            isPanning = false;
        }

        if (isPanning)
        {
            Panning();
        }
    }
    public void Panning()
    {
        Vector2 moveVector;

        if (!inputReleased)
        {
            moveVector = -UserInput.instance.MouseInput * _speed;
        }
        else
        {
            moveVector = remainingVelocity;
        }
        Vector2 targetPosition = new Vector2(transform.position.x + moveVector.x, transform.position.y + moveVector.y);
        Vector2 smoothedPosition = Vector2.SmoothDamp(transform.position, targetPosition, ref currentVelocity, _dampAmount);

        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);

        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, 0);
        if (inputReleased)
        {
            remainingVelocity = new Vector2(
                Mathf.Lerp(currentVelocity.x, 0, _drag * Time.fixedDeltaTime),
                Mathf.Lerp(currentVelocity.y, 0, _drag * Time.fixedDeltaTime)
            );

            if (remainingVelocity.magnitude < 0.01f)
            {
                remainingVelocity = Vector2.zero;
                currentVelocity = Vector2.zero;
                isPanning = false;
            }
        }
    }
}