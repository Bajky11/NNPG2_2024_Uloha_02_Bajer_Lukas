using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src.NNPG2
{
    internal class EdgeNNPG2 : Edge<int, VertexData, EdgeData>
    {
        public EdgeNNPG2(Vertex<int, VertexData, EdgeData> startVertex, Vertex<int, VertexData, EdgeData> endVertex, EdgeData data) : base(startVertex, endVertex, data) { }

        public bool IsComplete()
        {
            return this.StartVertex != null && this.EndVertex != null;
        }

        public void Reset()
        {
            this.StartVertex = null;
            this.EndVertex = null;
        }

    }


}
