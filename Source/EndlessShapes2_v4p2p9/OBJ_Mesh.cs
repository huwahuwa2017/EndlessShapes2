using System.Collections.Generic;

namespace EndlessShapes2
{
    public class OBJ_Mesh
    {
        public string Name { get; set; }

        public List<int[][]> FaceDatas { get; } = new List<int[][]>(256);

        public List<int[]> LineDatas { get; } = new List<int[]>();
    }
}
