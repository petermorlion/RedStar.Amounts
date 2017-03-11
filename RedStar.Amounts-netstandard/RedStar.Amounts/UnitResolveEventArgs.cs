using System;

namespace RedStar.Amounts
{
    public class UnitResolveEventArgs : EventArgs
    {
        public UnitResolveEventArgs(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}