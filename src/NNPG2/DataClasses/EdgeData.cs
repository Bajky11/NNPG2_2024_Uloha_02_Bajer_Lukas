using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src
{
    internal class EdgeData
    {
        private string Data;
        public EdgeData(string data)
        {
            Data = data;
        }

        public override string ToString()
        {
            return this.Data;
        }
    }
}
