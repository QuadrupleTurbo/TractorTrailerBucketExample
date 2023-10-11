using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FxEvents;

namespace TractorTrailerBucketExample.Server
{
    public class Main : BaseScript
    {
        #region Constructor

        public Main()
        {
            EventDispatcher.Initalize("6N3KsurEvL5cLKWkv9dkqFA3trThrULs", "UsYdAM3pSdKR24hNBd4VgVfdkZxf8pVh", "KfCrSp3gR4Tq2uxTSys2G56cw5LZWy9z");
            EventDispatcher.Mount("qdx_core:setTractorTrailerRoutingBucket:Server", new Func<Player, int, int, int, Task<bool>>(SetTractorTrailerRoutingBucket));
        }

        #endregion

        #region Events

        private async Task<bool> SetTractorTrailerRoutingBucket([FromSource] Player source, int bucket, int vehId, int trailerId)
        {
            if (vehId == 0 || trailerId == 0)
            {
                API.SetPlayerRoutingBucket(source.Handle, 0);
                return false;
            }

            var currTime = API.GetGameTimer();
            while (Entity.FromNetworkId(vehId) == null && Entity.FromNetworkId(trailerId) == null && API.GetGameTimer() - currTime < 7000)
            {
                Debug.WriteLine("Waiting for the vehicle & trailer to not be null...");
                await Delay(0);
            }

            if (Entity.FromNetworkId(vehId) == null || Entity.FromNetworkId(trailerId) == null)
            {
                API.SetPlayerRoutingBucket(source.Handle, 0);
                return false;
            }

            var veh = Entity.FromNetworkId(vehId);
            var trailer = Entity.FromNetworkId(trailerId);

            // Wait for the ped to exit the vehicle
            while (API.GetVehiclePedIsIn(source.Character.Handle, false) != 0)
            {
                API.TaskLeaveVehicle(source.Character.Handle, veh.Handle, 16);
                Debug.WriteLine("GET OUT!");
                await Delay(0);
            }

            // Route all the entities
            API.SetEntityRoutingBucket(veh.Handle, bucket);
            API.SetEntityRoutingBucket(trailer.Handle, bucket);
            API.SetPlayerRoutingBucket(source.Handle, bucket);
            API.SetRoutingBucketPopulationEnabled(bucket, bucket == 0);

            // Wait for the player to enter the vehicle
            while (API.GetVehiclePedIsIn(source.Character.Handle, false) == 0)
            {
                API.TaskWarpPedIntoVehicle(source.Character.Handle, veh.Handle, -1);
                Debug.WriteLine("Waiting for the fucker to be in the vehicle");
                await Delay(0);
            }

            return true;
        }

        #endregion
    }
}