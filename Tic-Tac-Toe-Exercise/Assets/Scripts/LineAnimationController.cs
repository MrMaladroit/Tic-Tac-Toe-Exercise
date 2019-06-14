using UnityEngine;

public class LineAnimationController : MonoBehaviour
{
    [SerializeField] LineFactory lineFactory;
    [SerializeField] float lineWidth = 2f;

    public Transform[] postions;

    private Line line;

    public void DrawLine()
    {
        Vector2 start = postions[0].position;
        Vector2 end = postions[1].position;
        line = lineFactory.GetLine(start, end, lineWidth, Color.white);
    }
}