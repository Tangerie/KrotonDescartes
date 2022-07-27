using System;
using System.Collections.Generic;

namespace Descartes
{
    public partial struct World : IEquatable<World>
    {
        private World(int index, int version)
        {
            Index = index;
            Version = version;
            Entities = new EntityManager();
            Components = new ComponentManager();
        }

        public readonly int Index;
        public readonly int Version;

        public static World Create()
        {
            return new World();
        }

        public EntityManager Entities { get; }
        public ComponentManager Components { get; }


        public bool Equals(World other) => Index == other.Index && Version == other.Version;
        public override bool Equals(object obj) => obj is World other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Index, Version);
        public static bool operator ==(World a, World b) => a.Equals(b);
        public static bool operator !=(World a, World b) => !a.Equals(b);
        public static implicit operator int(World world) => world.Index;
    }
}