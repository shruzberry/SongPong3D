using UnityEngine;
using Types;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    //____________CONTROLS_________________
    private InputMaster input;

    //____________REFERENCES_______________
    private AxisManager axisManager;

    //____________MOVEMENT_________________
    public float[] bounds;
    public Vector2 paddlePos;
    public float speed = 10.0f;
    private Vector2 direction;

    //____________ATTRIBUTES_______________
    public Paddles paddleNum;
    public float radius; // the half-width or radius of the paddle
    public float height;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INIT
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    
    void OnEnable()
    { 
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    /*void Awake()
    {
        axisManager = FindObjectOfType<AxisManager>();

        input = new InputMaster();
        input.Paddle.Move.performed += mov => direction = mov.ReadValue<Vector2>();
    }*/

    public void Init(Paddles num, float[] bounds)
    {
        this.bounds = bounds;
        this.paddleNum = num;

        radius = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * UPDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void Update()
    {
        movePaddle();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void movePaddle()
    {
        /*
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 paddlePos = mousePos; // set paddle pos

        paddlePos.x = Mathf.Clamp(paddlePos.x, bounds[0], bounds[1]); // clamp x
        paddlePos.y = Mathf.Clamp(paddlePos.y, bounds[2], bounds[3]); // clamp y

        transform.position = paddlePos;
        */
        //paddlePos = context.ReadValue<Vector2>();
        paddlePos += direction * speed * Time.deltaTime;
        //paddlePos.x = Mathf.Clamp(paddlePos.x, bounds[0], bounds[1]); // clamp x
        //paddlePos.y = Mathf.Clamp(paddlePos.y, bounds[2], bounds[3]); // clamp y

        transform.position = paddlePos;
    }
}
