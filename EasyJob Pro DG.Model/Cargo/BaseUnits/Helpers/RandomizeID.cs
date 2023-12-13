using System;

namespace EasyJob_ProDG.Model.Cargo
{
    internal static class RandomizeID
    {
        static private readonly Random random = new Random();
        static internal int GetNewID()
        {
            return random.Next();
        }
    }
}
