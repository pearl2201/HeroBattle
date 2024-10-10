using HeroBattle.FixedMath;
using System.Numerics;

namespace SharpSteer2
{
    /// <summary>
    /// A flow field which can be sampled at arbitrary locations
    /// </summary>
    public interface IFlowField
    {
        /// <summary>
        /// Sample the flow field at the given location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        Vector3f Sample(Vector3f location);
    }
}
