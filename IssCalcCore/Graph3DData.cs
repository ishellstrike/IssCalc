using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IssCalcCore
{
    public class Graph3DData {
        public double[] data;
        public int w, h;

        public Graph3DData(int i, int i1) {
            w = i;
            h = i1;
            data = new double[i*i1];
        }
    }
}
