using UnityEngine;
using Types;

public class Paddle : MonoBehaviour
{
    //____________REFERENCES_______________
    private AxisManager axisManager;

    //____________MOVEMENT_________________
    public float[] bounds;

    //____________ATTRIBUTES_______________
    public Paddles paddleNum;
    public float radius; // the half-width or radius of the paddle
    public float height;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INIT
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void Awake(){
        axisManager = FindObjectOfType<AxisManager>();
    }

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

   private void movePaddle()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 paddlePos = mousePos; // set paddle pos

        paddlePos.x = Mathf.Clamp(paddlePos.x, bounds[0], bounds[1]); // clamp x
        paddlePos.y = Mathf.Clamp(paddlePos.y, bounds[2], bounds[3]); // clamp y

        transform.position = paddlePos;
    }
}
