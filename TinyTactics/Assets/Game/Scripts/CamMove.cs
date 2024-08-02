using UnityEngine;

public class CamMove : MonoBehaviour
{

    Camera cam;

    DefaultControl DefaultControl;
    public bool invertMouse = true;

    [SerializeField] float sensitivity = 1;

    void Awake(){

        DefaultControl = new DefaultControl();

        DefaultControl.Player.MouseScroll.performed += x => Zoom(x.ReadValue<float>());
        DefaultControl.Player.MousePan.performed += x => Pan(x.ReadValue<Vector2>());

        cam = transform.GetComponent<Camera>();
    }

    void Pan(Vector2 PanVal){

        if(DefaultControl.Player.MouseMiddle.IsPressed()){
            if(invertMouse){
                transform.Translate((PanVal * Time.deltaTime * sensitivity)*cam.orthographicSize);
            }
            else{
                transform.Translate(((PanVal * Time.deltaTime * sensitivity)*-1)*cam.orthographicSize);
            }
        }
    }

    void Zoom(float mouseScrollVal){

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 2, 30);

        cam.orthographicSize += -mouseScrollVal * Time.deltaTime;
    }

#region - Enable / Disable -

    void OnEnable(){

        DefaultControl.Enable();
    }
    void OnDisable(){

        DefaultControl.Disable();
    }

#endregion

}

