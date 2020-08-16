using UnityEngine;

/**
 * The grid floor that the game takes place on
 */
public class GridFloor : MonoBehaviour
{
    //_____ SETTINGS ____________________
    //_____ REFERENCES __________________
    //_____ COMPONENTS __________________
    private MeshRenderer mesh_renderer;

    //_____ ATTRIBUTES __________________
    public Vector3 size;

    //_____ STATE  ______________________
    //_____ OTHER _______________________

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    // Start is called before the first frame update
    void Awake()
    {
        mesh_renderer = GetComponentInChildren<MeshRenderer>();
        size = Vector3.Scale(mesh_renderer.bounds.size, transform.localScale);
    }

}
