using System.Numerics;
using System.Windows.Forms;
#pragma warning disable CA1050
public class ProcessArgs
{
    private static ProcessArgs empty = new ProcessArgs();
    public static ProcessArgs Empty => empty;
}

public class CameraArgs : ProcessArgs
{
    public CameraArgs(Vector3 CameraPosition, Vector3 CameraDirection, float MovementSpeed)
    {
        CameraStartingDirection = CameraDirection;
        CameraStartingPosition = CameraPosition;
        CameraSpeed = MovementSpeed;
    }
    public float CameraSpeed { get; set; } = 1;
    public Vector3 CameraStartingPosition { get; set; }
    public Vector3 CameraStartingDirection { get; set; }
}

public class KeyMapArgs : ProcessArgs
{
    public PictureBox Pb { get; set; }
    public Form Form { get; set; }
}

public class FovArgs : ProcessArgs
{
    public FovArgs(
        float VerticalAngle, 
        float HorizontalAngle,
        Vector3 CameraPosition,
        Vector3 CameraDirection
    )
    {
        VerticalViewAngle = VerticalAngle;
        HorizontalViewAngle = HorizontalAngle;
        CameraStartingDirection = CameraDirection;
        CameraStartingPosition = CameraPosition;
    }
    public float VerticalViewAngle { get; set; }
    public float HorizontalViewAngle { get; set; }
    public Vector3 CameraStartingPosition { get; set; }
    public Vector3 CameraStartingDirection { get; set; }
}