using PluginAPI;
using PluginAPI.Enums;
using PluginAPI.Events;
using PluginAPI.Events.EventArgs;
using PluginAPI.Interfaces;
using SCP_ET.Player;
using System;
using SCP_ET.World.Doors;
using System.Linq;
using MEC;

namespace StayShut
{
    public class MainPlugin : Plugin<Config>
    {
        public override string Name => "StayShut";
        public override string Author => "Thunder";
        public override PluginType Type => PluginType.GameplayMod;
        public override Version Version => new Version(0, 0, 0);

        public static readonly DoorType[] ValidDoors = new[]
        {
            DoorType.ContainmentDoor,
            DoorType.Default,
            DoorType.Hcz,
            DoorType.OfficeDoor,
        };

        public override void OnEnabled()
        {
            ServerEvents.InteractDoor += OnDoorInteract;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            ServerEvents.InteractDoor -= OnDoorInteract;
            base.OnDisabled();
        }

        public void OnDoorInteract(InteractDoorEvent ev)
        {
            if (ev.Finalized && ev.Entity is PlayerMain && ValidDoors.Contains(ev.DoorType))
            {
                if (ev.DoorType == DoorType.ContainmentDoor && Config.CloseGates == false)
                    return;
                Timing.CallDelayed(Config.OpenTime, () =>
                {
                    if (ev.Door.IsOpen)
                    {
                        ev.Door.Close();
                    }
                });
            }
        }
    }

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int OpenTime { get; set; } = 5;
        public bool CloseGates { get; set; } = true;
    }
}
