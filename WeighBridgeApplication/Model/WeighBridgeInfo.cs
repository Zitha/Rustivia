using System;

namespace WeighBridgeApplication.Model
{
    public class WeighBridgeInfo
    {
        public int Id { get; set; }

        public long FirstMass { get; set; }

        public long SecondMass { get; set; }

        public long NettMass { get; set; }

        public DateTime DateIn { get; set; }

        public DateTime DateOut { get; set; }

        public string Comments { get; set; }

        public string Product { get; set; }
    }
}
