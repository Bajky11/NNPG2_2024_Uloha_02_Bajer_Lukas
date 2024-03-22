using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPG2_2024_Uloha_02_Bajer_Lukas.src.SEMA
{
    internal class Data<Key>
    {
        public Key[] Vertices;
        public Key[] InputVertices;
        public Key[] OutputVertices;
        public Key[][] Cross;
        public Key[][] Edges;


        public List<List<Key>> GetListOfCrossPaths()
        {
            List<List<Key>> crossPathsList = new List<List<Key>>();

            // Assuming crossPaths is initialized elsewhere
            foreach (Key[] crossPath in Cross)
            {
                List<Key> path = new List<Key>(crossPath);
                crossPathsList.Add(path);
            }

            return crossPathsList;
        }

    }


}
