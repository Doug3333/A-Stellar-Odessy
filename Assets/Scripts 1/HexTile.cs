using UnityEngine;


public class HexTile : MonoBehaviour
{
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public void Highlight(bool enable)
    {
        // Visual indicator for the path (e.g., change color or material)
        GetComponent<Renderer>().material.color = enable ? Color.yellow : Color.white;
    }
}
