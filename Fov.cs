using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Drawing;
#pragma warning disable CA1050
public abstract class Fov
{
    public float FovRange { get; protected set; } 
    public float FovVerticalAngle { get; protected set; }
    public float FovHorizontalAngle { get; protected set; }
    public Vector3 UpLeft { get; protected set; }
    public Vector3 UpRight { get; protected set; }
    public Vector3 DownLeft { get; protected set; }
    public Vector3 DownRight { get; protected set; }
    public abstract void SetPoints();
    public abstract Point GetPointInScreen(Vector3 point);
    public abstract bool CheckIfPointInFov(Vector3 point);
    public abstract Dictionary<string, Vector3> GetCorners(Vector3 point);
    public abstract Vector3 RotatePoint(Vector3 Axis, float Rotation, Vector3 CurrentPoint);
}
public class DefaultFov : Fov
{
    public DefaultFov(FovArgs args)
    {
        FovHorizontalAngle = args.HorizontalViewAngle/2;
        FovVerticalAngle = args.VerticalViewAngle/2;
        FovRange = Vector3.Distance(
            args.CameraStartingDirection,
            args.CameraStartingPosition
        );
    }
    public override void SetPoints()
    {
        var corners = GetCorners(Engine.Current.Camera.CameraDirection);

        UpLeft = corners["UpLeft"];
        UpRight = corners["UpRight"];
        DownLeft = corners["DownLeft"];
        DownRight = corners["DownRight"];
    }
    public override Vector3 RotatePoint(
        Vector3 Axis, 
        float Rotation, 
        Vector3 CurrentPoint
    )
    {
        var normalizedAxisVector = Vector3.Normalize(Axis);
        var radians = MathF.PI * Rotation / 180.0f;
        var q = Quaternion.CreateFromAxisAngle(normalizedAxisVector, radians);
        return Vector3.Transform(CurrentPoint, q);
    }

    public override bool CheckIfPointInFov(Vector3 point)
    {
        var Camera = Engine.Current.Camera;
        var distance = Vector3.Distance(point, Camera.CameraPosition);

        if (distance > FovRange)
            return false;
    
        var centerPointAtDistance = ResizeDirection(distance);

        var translatedVector = Vector3.Subtract(centerPointAtDistance, Camera.CameraPosition);
        var oppositePoint = Vector3.Subtract(Camera.CameraPosition, translatedVector);
        
        var distanceFromDirection = Vector3.Distance(point, Camera.CameraDirection);
        var distanceFromOppositeDirection = Vector3.Distance(point, oppositePoint);

        if (distanceFromOppositeDirection < distanceFromDirection)
            return false;

        var TranslatedCameraDirection = Vector3.Subtract(centerPointAtDistance, Camera.CameraPosition);

        var verticalRotationAxis = new Vector3(
            -TranslatedCameraDirection.Y, 
            TranslatedCameraDirection.X, 
            0
        );

        var horizontalRotationAxis = RotatePoint(
            verticalRotationAxis, 
            -90f, 
            TranslatedCameraDirection
        );

        var UpVector = RotatePoint(
            verticalRotationAxis, 
            -FovVerticalAngle, 
            TranslatedCameraDirection
        );

        var UpRightCorner = Vector3.Add(
            RotatePoint(horizontalRotationAxis, -FovHorizontalAngle, UpVector), 
            Camera.CameraPosition
        );

        var bottomOfCamera = Vector3.Add(
            RotatePoint(verticalRotationAxis, 90f, TranslatedCameraDirection), 
            Camera.CameraPosition
        );

        var topOfCamera = Vector3.Add(
            RotatePoint(verticalRotationAxis, -90f, TranslatedCameraDirection), 
            Camera.CameraPosition
        );

        var sideOfCamera = Vector3.Add(
            RotatePoint(horizontalRotationAxis, -90, TranslatedCameraDirection), 
            Camera.CameraPosition
        );

        var normal = CalculateNormalVector(
            bottomOfCamera, 
            topOfCamera, 
            sideOfCamera
        );

        var distanceOfPointFromPlane = GetPointDistanceToPlace(
            point, 
            Camera.CameraPosition, 
            normal
        );

        var distanceOfFovCornerFromPlane = GetPointDistanceToPlace(
            UpRightCorner, 
            Camera.CameraPosition, 
            normal
        );

        if (distanceOfPointFromPlane > distanceOfFovCornerFromPlane/2)
            return true;

        return false;
    }
    public override Point GetPointInScreen(Vector3 point)
    {
        var Camera = Engine.Current.Camera;

        var distanceFromCamera = Vector3.Distance(Camera.CameraPosition, point);
        var centerPointAtDistance = ResizeDirection(distanceFromCamera);
        var corners = GetCorners(centerPointAtDistance);

        var normal = CalculateNormalVector(
            corners["UpRight"], 
            corners["UpLeft"], 
            corners["DownLeft"]
        );

        var distanceFromPlane = GetPointDistanceToPlace(
            point, 
            corners["DownLeft"],
            normal
        );

        var changeVector = Vector3.Multiply((float)distanceFromPlane, normal);
        var newPoint = Vector3.Subtract(point, changeVector);

        var newPointDistance = Vector3.Distance(Camera.CameraPosition, newPoint);

        if (newPointDistance > distanceFromCamera)
            newPoint = Vector3.Add(point, changeVector);

        var fovHeight = Vector3.Distance(corners["UpLeft"], corners["DownLeft"]);
        var fovWidth = Vector3.Distance(corners["DownLeft"], corners["DownRight"]);

        var upRightDistance = Vector3.Distance(corners["UpRight"], newPoint);
        var upLeftDistance = Vector3.Distance(corners["UpLeft"], newPoint);

        var a = fovHeight;
        var b = Vector3.Distance(corners["DownRight"], newPoint);
        var c = upRightDistance;

        var hProportion = Engine.Current.ScreenWidth/a;
        var vProportion = Engine.Current.ScreenHeight/fovHeight;

        if (upLeftDistance > upRightDistance)
        {
            b = upLeftDistance;
            c = Vector3.Distance(corners["DownLeft"], newPoint);
        }

        var s = (a + b + c) / 2;
        var h =  2 * Math.Sqrt(s*(s-a) * (s-b) * (s-c)) / a;

        var cornerToPointAngle = Math.Acos((c*c + a*a - b*b)/(2 * a * c));

        var innerTriangleAngle = GetRadians(90 - (cornerToPointAngle * (180 / Math.PI)));

        var m = Math.Sin(innerTriangleAngle) * c;

        var x = (int)((fovWidth - h) * hProportion);
        var y = (int)(m * vProportion);

        if (upLeftDistance > upRightDistance)
        {
            if (GetAngle(corners["DownLeft"], corners["DownRight"], point) > 90)
                h = -h;

            x = (int)(h*hProportion);
            y = Engine.Current.ScreenHeight - (int)(m * vProportion);
        }
            return new Point(x,y);
    }

    public double GetPointDistanceToPlace(
        Vector3 Point, 
        Vector3 PointInPlane, 
        Vector3 Normal)
    {
        var d = -(
            Normal.X * PointInPlane.X + 
            Normal.Y * PointInPlane.Y + 
            Normal.Z * PointInPlane.Z
        );

        var absolute = Math.Abs(
            Normal.X * Point.X + 
            Normal.Y * Point.Y + 
            Normal.Z * Point.Z + d
        );  
        
        var sqrt = Math.Sqrt(
            Normal.X * Normal.X + 
            Normal.Y*Normal.Y + 
            Normal.Z * Normal.Z
        );

        return absolute / sqrt;
    }
    public Vector3 ResizeDirection(float Size)
    {
        var Camera = Engine.Current.Camera;
        var translatedDirection = Vector3.Subtract(Camera.CameraDirection, Camera.CameraPosition);
        var change = Size/FovRange;
        var changedVector = Vector3.Multiply(change, translatedDirection);
        return Vector3.Add(Camera.CameraPosition, changedVector);
    }
    public Vector3 CalculateNormalVector(
        Vector3 A, 
        Vector3 B, 
        Vector3 C
    )
    {
        Vector3 AB = B - A;
        Vector3 AC = C - A;
        Vector3 normalVector = Vector3.Cross(AB, AC);
        return Vector3.Normalize(normalVector);
    }

    public double GetAngle(
        Vector3 axis, 
        Vector3 point1, 
        Vector3 point2
    )
    {
        var a = Vector3.Subtract(point1, axis);
        var b = Vector3.Subtract(point2, axis);
        var dot = Vector3.Dot(a, b);
        
        var magA = Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z);
        var magB = Math.Sqrt(b.X * b.X + b.Y * b.Y + b.Z * b.Z);
        var angle = Math.Acos(dot / ( magA * magB ));

        return angle * (180 / Math.PI);
    }

    public double GetRadians(double degrees) => degrees * (Math.PI/180);
    public override Dictionary<string, Vector3> GetCorners(Vector3 point)
    {
        var TranslatedCameraDirection = Vector3.Subtract(point, Engine.Current.Camera.CameraPosition);

        var verticalRotationAxis = new Vector3(
            -TranslatedCameraDirection.Y, 
            TranslatedCameraDirection.X, 
            0
        );

        var horizontalRotationAxis = RotatePoint(
            verticalRotationAxis, 
            -90f, 
            TranslatedCameraDirection
        );

        var UpVector = RotatePoint(
            verticalRotationAxis, 
            -FovVerticalAngle, 
            TranslatedCameraDirection
        );
        
        var DownVector = RotatePoint(
            verticalRotationAxis, 
            FovVerticalAngle, 
            TranslatedCameraDirection
        );

        var upLeft = RotatePoint(
            horizontalRotationAxis, 
            FovHorizontalAngle, 
            UpVector
        );

        var downLeft = RotatePoint(
            horizontalRotationAxis, 
            FovHorizontalAngle, 
            DownVector
        );

        var upRight = RotatePoint(
            horizontalRotationAxis, 
            -FovHorizontalAngle, 
            UpVector
        );

        var downRight = RotatePoint(
            horizontalRotationAxis, 
            -FovHorizontalAngle, 
            DownVector
        );
        
        return new Dictionary<string, Vector3>() {
            {"UpLeft", Vector3.Add(upLeft, Engine.Current.Camera.CameraPosition)}, 
            {"UpRight", Vector3.Add(upRight, Engine.Current.Camera.CameraPosition)},
            {"DownLeft", Vector3.Add(downLeft, Engine.Current.Camera.CameraPosition)},
            {"DownRight", Vector3.Add(downRight, Engine.Current.Camera.CameraPosition)}
        };
    }
}
