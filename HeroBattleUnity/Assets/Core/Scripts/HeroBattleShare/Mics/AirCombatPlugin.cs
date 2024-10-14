using HeroBattle;
using HeroBattle.FixedMath;
using SharpSteer2;
using SharpSteer2.Database;
using System.Collections.Generic;

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
            Vector3f center = Vector3f.Zero;
            const float DIV = 10.0f;
            Vector3f divisions = new Vector3f(DIV, DIV, DIV);
            const float DIAMETER = 10.0f;//Fighter.WORLD_RADIUS * 2;
            Vector3f dimensions = new Vector3f(DIAMETER, DIAMETER, DIAMETER);
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
