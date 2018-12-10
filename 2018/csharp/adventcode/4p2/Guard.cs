using System.Collections.Generic;

namespace _4p2
{
    internal class Guard
    {
        public string Id { get; set; }

        public Dictionary<int,int> Minutes { get; set; }
        public int Total { get; internal set; }

        public Guard()
        {
            Minutes = new Dictionary<int, int>();
        }
    }
}