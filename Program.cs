using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

var builder = Engine.GetBuilder();

builder
    .SetCameraStartingPosition(0, 2000, 0)
    .SetCameraStartingDirection(0, 0 ,0)
    .SetCameraMovementSpeed(5)
    .SetFovAngle(60, 90)
    .SetArgs()
    .SetKeyMapping<DefaultKeyMap>()
    .SetCamera<DefaultCamera>()
    .SetFov<DefaultFov>();

Engine.New(builder);

Engine.Current.Run();