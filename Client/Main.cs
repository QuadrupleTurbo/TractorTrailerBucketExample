using CitizenFX.Core;
using CitizenFX.Core.Native;
using FxEvents;

namespace TractorTrailerBucketExample.Client
{
    public class Main : BaseScript
    {
        #region Fields

        private Vehicle _veh = null;
        private Vehicle _trailer = null;
        private int _tempBucket = 0;

        #endregion

        #region Constructor 
         
        public Main() 
        {
            EventDispatcher.Initalize("6N3KsurEvL5cLKWkv9dkqFA3trThrULs", "UsYdAM3pSdKR24hNBd4VgVfdkZxf8pVh", "KfCrSp3gR4Tq2uxTSys2G56cw5LZWy9z");
            API.RegisterKeyMapping("-switch", "\u200b", "keyboard", "5");
        }

        #endregion

        #region Commands

        [Command("switch")]
        private async void Switch()
        {
            // Spawn the tractor & trailer
            if (!Game.PlayerPed.IsInVehicle())
            {
                _trailer = await World.CreateVehicle((Model)"freighttrailer", Game.PlayerPed.Position, Game.PlayerPed.Heading);
                _veh = await World.CreateVehicle((Model)"phantom", Game.PlayerPed.Position, Game.PlayerPed.Heading);
                Game.PlayerPed.SetIntoVehicle(_veh, VehicleSeat.Driver);
                API.AttachVehicleToTrailer(_veh.Handle, _trailer.Handle, 1.1f);
                await Delay(100);
            }

            // Change buckets
            if (_tempBucket == 0)
            {
                if (Game.PlayerPed.IsInVehicle() && _veh != null && _trailer != null)
                {
                    _tempBucket = 1;
                    bool success = await EventDispatcher.Get<bool>("qdx_core:setTractorTrailerRoutingBucket:Server", _tempBucket, _veh.NetworkId, _trailer.NetworkId);
                    if (success)
                    {
                        Debug.WriteLine($"Routed to bucket {_tempBucket} successfully!");
                    }
                    else
                    {
                        Debug.WriteLine($"An error occured with the routing to bucket {_tempBucket}!");
                        _tempBucket = 0;
                    }
                }
            }
            else
            {
                _tempBucket = 0;
                bool success = await EventDispatcher.Get<bool>("qdx_core:setTractorTrailerRoutingBucket:Server", _tempBucket, _veh != null ? _veh.NetworkId : 0, _trailer != null ? _trailer.NetworkId : 0);
                if (success)
                {
                    Debug.WriteLine($"Routed to default bucket successfully!");
                }
                else
                {
                    Debug.WriteLine($"An error occured with the routing to default bucket!");
                }
            }
        }

        #endregion
    }
}