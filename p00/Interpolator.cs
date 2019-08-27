using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace p00
{
    class Interpolator
    {
        List<int> values = new List<int>();
        public Interpolator()
        {

        }

        public Interpolator(int v1, int v2, int v3, int v4, int dummy)
        {
            switch (dummy)
            {
                case 1:
                    values.Add(int.MinValue);
                    values.Add(v2);
                    values.Add(v3);
                    values.Add(v4);
                    break;
                case 2:
                    values.Add(v1);
                    values.Add(int.MinValue);
                    values.Add(v3);
                    values.Add(v4);
                    break;
                case 3:
                    values.Add(v1);
                    values.Add(v2);
                    values.Add(int.MinValue);
                    values.Add(v4);
                    break;
                case 4:
                    values.Add(v1);
                    values.Add(v2);
                    values.Add(v3);
                    values.Add(int.MinValue);
                    break;
                default:
                    break;
            }
        }

        public Interpolator(int v1, int v2, int v3, int v4, int v5, int dummy)
        {
            switch (dummy)
            {
                case 1:
                    values.Add(int.MinValue);
                    values.Add(v2);
                    values.Add(v3);
                    values.Add(v4);
                    values.Add(v5);
                    break;
                case 2:
                    values.Add(v1);
                    values.Add(int.MinValue);
                    values.Add(v3);
                    values.Add(v4);
                    values.Add(v5);
                    break;
                case 3:
                    values.Add(v1);
                    values.Add(v2);
                    values.Add(int.MinValue);
                    values.Add(v4);
                    values.Add(v5);
                    break;
                case 4:
                    values.Add(v1);
                    values.Add(v2);
                    values.Add(v3);
                    values.Add(int.MinValue);
                    values.Add(v5);
                    break;
                case 5:
                    values.Add(v1);
                    values.Add(v2);
                    values.Add(v3);
                    values.Add(v4);
                    values.Add(int.MinValue);
                    break;
                default:
                    break;

            }
        }

    }
}
