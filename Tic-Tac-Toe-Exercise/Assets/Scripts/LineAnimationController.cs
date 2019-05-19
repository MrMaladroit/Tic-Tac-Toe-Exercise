using UnityEngine;

public class LineAnimationController : MonoBehaviour
{
    private LineRenderer line;

    private void Awake()
    {
        line = GetComponentInChildren<LineRenderer>();
    }

    public void PlayAnimation()
    {
        line.SetPosition(1, new Vector3(0, -10, 1));
    }
}
