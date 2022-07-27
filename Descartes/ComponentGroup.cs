using System;

namespace Descartes
{
    // Contains all the components of a single type
    public struct ComponentGroup<T> : IEquatable<ComponentGroup<T>>
    {
        internal ComponentGroup(World world, int index, int version)
        {
            World = world;
            Index = index;
            Version = version;
        }

        public readonly World World;
        public readonly int Index;
        public readonly int Version;
        
        public bool Equals(ComponentGroup<T> other) => Index == other.Index && Version == other.Version && other.World == World;
        public override bool Equals(object obj) => obj is World other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Index, Version, World);
        public static bool operator ==(ComponentGroup<T> a, ComponentGroup<T> b) => a.Equals(b);
        public static bool operator !=(ComponentGroup<T> a, ComponentGroup<T> b) => !a.Equals(b);
        public static implicit operator int(ComponentGroup<T> componentGroup) => componentGroup.Index;
    }
}