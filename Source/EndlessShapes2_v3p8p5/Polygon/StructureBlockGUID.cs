using System;

namespace EndlessShapes2.Polygon
{
    public struct StructureBlockGUID
    {
        public Guid Block;

        public Guid Slope;

        public Guid Wedge;

        public Guid Pole;

        public StructureBlockGUID(string block, string slope, string wedge, string pole)
        {
            Block = new Guid(block);
            Slope = new Guid(slope);
            Wedge = new Guid(wedge);
            Pole = new Guid(pole);
        }
    }
}
