using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src
{
    internal class EdgeData
    {
        public VertexData Start { get; set; }
        public VertexData End { get; set; }

        public EdgeData(VertexData start = null, VertexData end = null)
        {
            Start = start;
            End = end;
        }

        public bool IsComplete() => Start != null && End != null;

        public void Reset()
        {
            Start = null;
            End = null;
        }
    }
}
