using System;

namespace Advent_Of_Code_11_20
{
    internal class IHaveABadFeelingAboutThisException : Exception
    {
        public IHaveABadFeelingAboutThisException(string msg): base(msg)
        {
        }
    }
}