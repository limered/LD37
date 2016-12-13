using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Systems.SaveThePlayer
{
    public class TeleportOnDropComponent : GameComponent
    {
        public float MinHeightTrigger;
        public Transform TeleportLocation;
    }
}
