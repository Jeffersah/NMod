using System;
using System.Collections.Generic;
using System.Text;

namespace NMod
{
    public class StackUtils
    {
        public static float LinearStack(int itemCount, float baseAmt, float perItem)
        {
            return baseAmt + perItem * (itemCount - 1);
        }
        public static int LinearStack(int itemCount, int baseAmt, int perItem)
        {
            return baseAmt + perItem * (itemCount - 1);
        }

        public static float HyperbolicStack(int itemCount, float baseAmt, float perStack)
        {
            return 1 - (float)((1 - baseAmt) * Math.Pow(1 - perStack, itemCount - 1));
        }

        public static float ExponentialStack(int itemCount, float baseAmt, float perStack)
        {
            return (float)(baseAmt * Math.Pow(perStack, itemCount));
        }
    }
}
