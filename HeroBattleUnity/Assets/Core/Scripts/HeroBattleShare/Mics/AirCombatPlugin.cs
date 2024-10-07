using HeroBattle;
using SharpSteer2.Database;
using SharpSteer2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace HeroBattleShare.Mics
{
    public class AirCombatPlugin
    {
        private readonly List<BaseMinion> _team1 = new List<BaseMinion>();
        private readonly List<BaseMinion> _team2 = new List<BaseMinion>();
        private IProximityDatabase<IVehicle> _pd;
        public void Open()
        {

        }

        private void CreateDatabase()
        {
            Vector3 center = Vector3.Zero;
            const float DIV = 10.0f;
            Vector3 divisions = new Vector3(DIV, DIV, DIV);
            const float DIAMETER = 10.0f;//Fighter.WORLD_RADIUS * 2;
            Vector3 dimensions = new Vector3(DIAMETER, DIAMETER, DIAMETER);
            _pd = new LocalityQueryProximityDatabase<IVehicle>(center, dimensions, divisions);
        }

        public void Update(float currentTime, float elapsedTime)
        {
            //foreach (var fighter in _team1)
            //    fighter.Update(currentTime, elapsedTime);
            //foreach (var fighter in _team2)
            //    fighter.Update(currentTime, elapsedTime);

            //foreach (var missile in _missiles)
            //    missile.Update(currentTime, elapsedTime);
            //_missiles.RemoveAll(m => m.IsDead);
        }

        public IEnumerable<BaseMinion> Vehicles
        {
            get
            {
                foreach (var fighter in _team1)
                    yield return fighter;
                foreach (var fighter in _team2)
                    yield return fighter;
                //foreach (var missile in _missiles)
                //    yield return missile;
            }
        }
    }
}
