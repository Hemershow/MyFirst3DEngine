using System;
using System.Linq;
using System.Drawing;
using System.Numerics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Diagnostics;
#pragma warning disable CA1050
public class Engine
{  
    private Engine() { }
    private static Engine crr = null;
    public static Engine Current => crr;
    public Camera Camera { get; protected set; }
    public CameraArgs CameraArgs { get; protected set; }
    public Vector3 CameraStartingPosition { get; private set; }
    public Vector3 CameraStartingDirection { get; private set; }
    public float CameraMovementSpeed { get; private set; }
    public Fov FieldOfView { get; protected set; }
    public FovArgs FovArgs { get; protected set; }
    public float FovVerticalAngle { get; protected set; }
    public float FovHorizontalAngle { get; protected set; }
    public KeyMap KeyMapping { get; protected set; }
    public KeyMapArgs KeyMapArgs { get; protected set; }
    public List<Polygon> Polygons { get; set; }
    public Form Forms { get; set; }
    public int FormsInterval { get; set; } = 1;
    public Graphics G { get; set; }
    public Timer Timer { get; set; }
    public PictureBox Pb { get; set; }
    public Pen DefaultPen { get; set; }
    public int ScreenWidth { get; set; }
    public int ScreenHeight { get; set; }
    public DateTime Now { get; set; }
    public Queue<DateTime> Queue { get; set; } 
    public void Run()
    {   
        ApplicationConfiguration.Initialize();

        Forms = new Form
        {
            WindowState = FormWindowState.Maximized,
            FormBorderStyle = FormBorderStyle.None,
            BackColor = Color.Black
        };

        Timer = new Timer
        {
            Interval = FormsInterval
        };
        
        Forms.KeyPreview = true;

        Timer.Tick += delegate
        {
            G.Clear(Color.Black);

            KeyMapping.SetAction();
            Camera.MoveCamera();
            Camera.ChangeDirection();

            Now = DateTime.Now;

            ConcurrentBag<Polygon> ToBeDrawn = new ConcurrentBag<Polygon>();
            List<Task> PolygonProcessing = new List<Task>();

            Parallel.For(0, Polygons.Count, i => {
                PolygonProcessing.Add(Task.Run(() => {
                    if (Polygons[i].CheckIfInFov())
                        ToBeDrawn.Add(Polygons[i]);
                }));}
            );

            Task.WaitAll(PolygonProcessing.ToArray());        

            foreach (var polygon in ToBeDrawn.OrderByDescending(polygon => polygon.AverageDistance))
            {
                var PolygonPen = new SolidBrush(polygon.Color);
                G.FillPolygon(PolygonPen, polygon.PointsInScreen.ToArray());
            }

            double fps = 0;
            Queue.Enqueue(Now);

            if (Queue.Count > 19)
            {
                DateTime old = Queue.Dequeue();
                var time = Now - old;
                fps = (int)(19 / time.TotalSeconds);
            }

            G.DrawString(
                $"FPS: {fps}",
                new Font("Arial", 25), 
                new SolidBrush(Color.White), 
                new RectangleF(0, 1000, 700, 700), 
                new StringFormat()
            );

            Pb.Refresh();
        };

        Forms.Load += delegate 
        {
            Cursor.Hide();
            
            Pb = new PictureBox
            {
                Dock = DockStyle.Fill
            };

            Forms.Controls.Add(Pb);
            var bmp = new Bitmap(Pb.Width, Pb.Height);
            G = Graphics.FromImage(bmp);
            Pb.Image = bmp;
            
            Queue = new Queue<DateTime>();
            DefaultPen = new Pen(Color.White, 4);

            ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            ScreenHeight = Screen.PrimaryScreen.Bounds.Height;

            Forms.KeyDown += (s, e) =>
            {
                if(e.KeyCode == Keys.Escape)
                    Application.Exit();
            };

            Polygons = Structures.Polygons;

            foreach (var polygon in Polygons)
            {
                polygon.CheckIfInFov();
            }

            var maxDistance = Polygons.Max(x => x.AverageDistance);
            var minDistance = Polygons.Min(x => x.AverageDistance);
            var proportion = (maxDistance - minDistance) / 100;

            foreach (var polygon in Polygons)
            {
                var lightChange = (polygon.AverageDistance - minDistance) / proportion;

                var newColor = Color.FromArgb(
                    (int)(polygon.Color.R - lightChange) < 0 ? 0 : (int)(polygon.Color.R - lightChange), 
                    (int)(polygon.Color.G - lightChange) < 0 ? 0 : (int)(polygon.Color.G - lightChange), 
                    (int)(polygon.Color.B - lightChange) < 0 ? 0 : (int)(polygon.Color.B - lightChange)
                );
                
                polygon.Color = newColor;
            }

            Timer.Start();
        };

        Application.Run(Forms);
    }

    public class EngineBuilder
    {
        private Engine Engine = new Engine();

        public Engine Build()
            => Engine;

        public EngineBuilder SetKeyMapping<T>()
            where T : KeyMap
        {
            Engine.KeyMapping = (T)Activator.CreateInstance<T>();
            return this;
        }

        public EngineBuilder SetCameraStartingPosition(float X, float Y, float Z)
        {
            Engine.CameraStartingPosition = new Vector3(X, Y, Z);
            return this;
        }

        public EngineBuilder SetCameraStartingDirection(float X, float Y, float Z)
        {
            Engine.CameraStartingDirection = new Vector3(X, Y, Z);
            return this;
        }

        public EngineBuilder SetCameraMovementSpeed(float MovementSpeed)
        {
            Engine.CameraMovementSpeed = MovementSpeed;
            return this;
        }

        public EngineBuilder SetFovAngle(float VerticalAngle, float HorizontalAngle)
        {
            Engine.FovVerticalAngle = VerticalAngle;
            Engine.FovHorizontalAngle = HorizontalAngle;
            return this;
        }

        public EngineBuilder SetCamera<T>()
            where T : Camera
        {
            Engine.Camera = (Camera)Activator.CreateInstance(typeof(T), new object[] {Engine.CameraArgs});
            return this;
        }

        public EngineBuilder SetFov<T>()
            where T : Fov
        {
            Engine.FieldOfView = (Fov)Activator.CreateInstance(typeof(T), new object[] {Engine.FovArgs});
            return this;
        }
        public EngineBuilder SetArgs()
        {
            Engine.CameraArgs = new CameraArgs(
                Engine.CameraStartingPosition,
                Engine.CameraStartingDirection,
                Engine.CameraMovementSpeed
            );

            Engine.FovArgs = new FovArgs(
                Engine.FovVerticalAngle, 
                Engine.FovHorizontalAngle,
                Engine.CameraStartingPosition,
                Engine.CameraStartingDirection
            );
            Engine.KeyMapArgs = new KeyMapArgs();
            return this;
        }
    }
    public static EngineBuilder GetBuilder()
        => new EngineBuilder();

    public static void New(EngineBuilder builder)
        => crr = builder.Build();
}
