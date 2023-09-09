using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

public static class Structures
{
    public static List<Polygon> Polygons = new List<Polygon>()
    {
        CreatePolygon<Hexagon>(new float[] {
            0, 0, 0, 86.6f, -50, 0, 
            173.21f, 0, 0, 173.21f, 100, 0, 
            86.6f, 150, 0, 0, 100, 0
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            0, 100, 0, 86.6f, 150, 0,
            97.7f, 230.9f, 57.7f, 22.1f, 261.8f, 115.5f,
            -64.5f, 211.8f, 115.5f, -75.6f, 130.09f, 57.7f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            86.6f, -50, 0, 0, 0, 0, 
            -75.6f, -30.09f, 57.7f, -64.5f, -111.8f, 115.5f, 
            22.1f, -161.8f, 115.5f, 97.7f, -130.9f, 57.7f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            173.21f, 0, 0, 173.21f, 100, 0,
            237.8f, 150, 57.7f, 302.38f, 99.99f, 115.5f,
            302.38f, 0, 115.5f, 237.8f, -50, 57.7f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            -100.36f, -111.23f, 207.38f, -147.06f, -31.16f, 243.08f,
            -158.03f, 49.46f, 186.09f, -122.3f, 50, 93.4f,
            -75.6f, -30.09f, 57.7f, -64.5f, -111.8f, 115.5f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            -148.06f, 131.11f, 245.61f, -100.73f, 212.31f, 209.45f,
            -64.5f, 211.8f, 115.5f, -75.6f, 130.09f, 57.7f,
            -122.3f, 50, 93.4f, -158.03f, 49.49f, 186.09f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            237.8f, -50, 57.7f, 191,-130.9f, 93.4f,
            209.05f, -161.73f, 186.86f, 274.33f, -111.49f, 244.33f,
            320.49f, -30.79f, 208.52f, 302.38f, 0, 115.5f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            97.7f, -130.9f, 57.7f, 22.1f, -161.8f, 115.5f,
            39.97f, -192.66f, 208.98f, 133.44f, -192.62f, 244.66f,
            209.05f, -161.73f, 186.86f, 191f, -130.9f, 93.4f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            191f, 230.9f, 93.4f, 237.8f, 150f, 57.7f,
            302.38f, 99.99f, 115.5f, 320.14f, 130.89f, 208.99f,
            273.33f, 211.8f, 244.69f, 208.75f, 261.81f, 186.89f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            97.7f, 230.9f, 57.7f, 191f, 230.9f, 93.4f,
            208.75f, 261.81f, 186.89f, 133.07f, 292.71f, 244.59f,
            39.63f, 292.70f, 208.8f, 22.1f, 261.8f, 115.5f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            273.33f, 211.8f, 244.69f, 320.14f, 130.89f, 208.99f,
            331.68f, 49.82f, 266f, 296.42f, 49.03f, 359.33f,
            249.63f, 129.32f, 395.66f, 238.09f, 210.39f, 338.65f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            296.42f, 49.03f, 359.33f, 331.68f, 49.82f, 266f,
            320.49f, -30.79f, 208.52f, 274.33f, -111.49f, 244.33f,
            239.36f, -111.59f, 337.63f, 250.55f, -30.98f, 395.11f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            -148.06f, 131.11f, 245.61f, -100.73f, 212.31f, 209.45f,
            -35.91f, 262.97f, 267.51f, -18.34f, 232.45f, 361.84f,
            -65.6f, 151.27f, 398.12f, -130.43f, 100.61f, 340.06f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            -18.34f, 232.45f, 361.84f, -35.91f, 262.97f, 267.51f,
            39.63f, 292.7f, 208.8f, 133.07f, 292.71f, 244.59f,
            151.8f, 261.98f, 337.57f, 76.26f, 232.25f, 396.28f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            133.44f, -192.62f, 244.66f, 39.97f, -192.66f, 208.98f,
            -37.19f, -160.33f, 265.05f, -20.94f, -127.2f, 358.76f,
            72.46f, -126.41f, 396.4f, 149.62f, -158.74f, 340.33f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            -20.94f, -127.2f, 358.76f, -37.19f, -160.33f, 265.05f,
            -100.36f, -111.23f, 207.38f, -147.06f, -31.16f, 243.08f,
            -129.32f, -0.03f, 333.78f, -66.15f, -49.13f, 391.45f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            151.8f, 261.98f, 337.57f, 238.09f, 210.39f, 338.65f,
            249.63f, 129.32f, 395.66f, 174.55f, 97.77f, 453.31f,
            82.76f, 157.61f, 452.03f, 76.26f, 232.25f, 396.28f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            -129.32f, -0.03f, 333.78f, -130.43f, 100.61f, 340.06f,
            -65.6f, 151.27f, 398.12f, -4.49f, 107.33f, 451.39f,
            -4.59f, 6.63f, 451.93f, -66.15f, -49.13f, 391.45f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            239.36f, -111.59f, 337.63f, 149.62f, -158.74f, 340.33f, 
            72.46f, -126.41f, 396.4f, 82.56f, -43.8f, 453.11f,
            169.82f, 6.47f, 453.75f, 250.55f, -30.98f, 395.11f
        }, Color.White),

        CreatePolygon<Hexagon>(new float[] {
            174.55f, 97.77f, 453.31f, 169.82f, 6.47f, 453.75f,
            82.56f, -43.8f, 453.11f, -4.59f, 6.63f, 451.93f,
            -4.49f, 107.33f, 451.39f, 82.76f, 157.61f, 452.03f
        }, Color.White),

        CreatePolygon<Pentagon>(new float[] {
            0, 0, 0, 0, 100, 0,
            -75.6f, 130.09f, 57.7f, -122.3f, 50, 93.4f,
            -75.6f, -30.09f, 57.7f
        }, Color.DarkBlue),

        CreatePolygon<Pentagon>(new float[] {
            86.6f, 150, 0, 173.21f, 100, 0,
            237.8f, 150, 57.7f, 191, 230.9f, 93.4f,
            97.7f, 230.9f, 57.7f
        }, Color.DarkBlue),

        CreatePolygon<Pentagon>(new float[] {
            173.21f, 0, 0, 86.6f, -50, 0,
            97.7f, -130.9f, 57.7f, 191, -130.9f, 93.4f,
            237.8f, -50, 57.7f
        }, Color.DarkBlue),

        CreatePolygon<Pentagon>(new float[] {
            -100.36f, -111.23f, 207.38f, -64.5f, -111.8f, 115.5f,
            22.1f, -161.8f, 115.5f, 39.97f, -192.66f, 208.98f,
            -37.19f, -160.33f, 265.05f
        }, Color.DarkBlue),

        CreatePolygon<Pentagon>(new float[] {
            39.63f, 292.7f, 208.80f, 22.1f, 261.8f, 115.5f,
            -64.5f, 211.8f, 115.5f, -100.73f, 212.31f, 209.45f,
            -35.91f, 262.97f, 267.51f
        }, Color.DarkBlue),

        CreatePolygon<Pentagon>(new float[] {
            320.49f, -30.79f, 208.52f, 302.38f, 0, 115.5f,
            302.38f, 99.99f, 115.5f, 320.14f, 130.89f, 208.99f,
            331.68f, 49.82f, 266f
        }, Color.DarkBlue),

        CreatePolygon<Pentagon>(new float[] {
            151.8f, 261.98f, 337.57f, 133.07f, 292.71f, 244.59f,
            208.75f, 261.81f, 186.89f, 273.33f, 211.8f, 244.69f,
            238.09f, 210.39f, 338.65f
        }, Color.DarkBlue),

        CreatePolygon<Pentagon>(new float[] {
            -129.32f, -0.03f, 333.78f, -147.06f, -31.16f, 243.08f,
            -158.03f, 49.49f, 186.09f, -148.06f, 131.11f, 245.61f,
            -130.43f, 100.61f, 340.06f,
        }, Color.DarkBlue),

        CreatePolygon<Pentagon>(new float[] {
            239.36f, -111.59f, 337.63f, 274.33f, -111.59f, 244.33f,
            209.05f, -161.73f, 186.86f, 133.44f, -192.62f, 244.66f,
            149.62f, -158.74f, 340.33f,
        }, Color.DarkBlue),

        CreatePolygon<Pentagon>(new float[] {
            174.55f, 97.77f, 453.31f, 249.63f, 129.32f, 395.66f,
            296.42f, 49.03f, 359.33f, 250.55f, -30.98f, 395.11f,
            169.82f, 6.47f, 453.75f,
        }, Color.DarkBlue),

        CreatePolygon<Pentagon>(new float[] {
            -4.49f, 107.33f, 451.39f, -65.6f, 151.27f, 398.12f,
            -18.34f, 232.45f, 361.84f, 76.26f, 232.25f, 396.28f,
            82.76f, 157.61f, 452.03f,
        }, Color.DarkBlue),

        CreatePolygon<Pentagon>(new float[] {
            82.56f, -43.8f, 453.11f, 72.46f, -126.41f, 396.4f,
            -20.94f, -127.2f, 358.76f, -66.15f, -49.13f, 391.45f,
            -4.59f, 6.63f, 451.93f,
        }, Color.DarkBlue)
    };

    private static Polygon CreatePolygon<T>(float[] points, Color color)
        where T : Polygon
    {
        var vectors = new List<Vector3>();

        for (int i = 0; i < points.Count(); i+=3)
            vectors.Add(new Vector3(points[i], points[i + 1], points[i + 2]));

        return (Polygon)Activator.CreateInstance(typeof(T), new object[] {vectors.ToArray(), color});
    }
}