using System;

namespace Ugen.Audio
{
    public readonly struct AudioInputDeviceInfo : IEquatable<AudioInputDeviceInfo>
    {
        public string Name { get; }
        public string Id { get; }
        public bool IsValid => !string.IsNullOrEmpty(Id);

        public AudioInputDeviceInfo(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public static readonly AudioInputDeviceInfo Empty = new("", "");

        public bool Equals(AudioInputDeviceInfo other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is AudioInputDeviceInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id != null ? Id.GetHashCode() : 0;
        }

        public static bool operator ==(AudioInputDeviceInfo left, AudioInputDeviceInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AudioInputDeviceInfo left, AudioInputDeviceInfo right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return IsValid ? $"{Name} ({Id})" : "Empty";
        }
    }
}
