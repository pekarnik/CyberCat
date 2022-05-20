 using UnityEngine;

public class PlayerModel : MonoBehaviour {

    public static int LAYER_MASK = 1 << Config.Layers.PLAYER;
    private float height;

    Rigidbody rb;

    
    public float Height {
        get {
            return height;
        }
        private set {
            height =  value;
        }
    }

    [SerializeField]
    private float jumpHeight = 7.0f;
    [SerializeField]
    private float doubleJumpHeight = 5.0f;
    [SerializeField]
    private float mouseRotationSpeed = 2.0f;
    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private bool isDoubleJumpEnabled = true;

    [SerializeField]
    private float speedDowned = 0.5f;
    
    public float SpeedDowned {
        get => speedDowned;
    }

    public bool IsDoubleJumpEnabled {
        get => isDoubleJumpEnabled;
    }

    public float MoveSpeed {
        get => moveSpeed;
    }

    public float MouseRotationSpeed {
        get => mouseRotationSpeed;
    }

    public float DoubleJumpHeight {
        get => doubleJumpHeight;
    }
    public float JumpHeight {
        get => jumpHeight;
    }
    private bool isMoved = false;
    private InputEventController inputEventController;

    void OnMovement(Vector3 movement) {
        Vector3 movePosition = rb.position + (movement * moveSpeed * Time.fixedDeltaTime);

        rb.MovePosition(movePosition);
    }

    void Start() 
    {
        Height = GetComponent<MeshFilter>().mesh.bounds.extents.y; 
        inputEventController = new InputEventController(this);
        rb = GetComponent<Rigidbody>();
        
        inputEventController.inputEventManager.PlayerMovement += OnMovement;

    }

    void FixedUpdate() {
        inputEventController.OnUpdate();
    }

}
