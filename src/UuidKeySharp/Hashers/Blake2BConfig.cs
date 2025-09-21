// BLAKE2 reference source code package - C# implementation

// Written in 2012 by Christian Winnerlein  <codesinchaos@gmail.com>

// To the extent possible under law, the author(s) have dedicated all copyright
// and related and neighboring rights to this software to the public domain
// worldwide. This software is distributed without any warranty.

// You should have received a copy of the CC0 Public Domain Dedication along with
// this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.

namespace UuidKeySharp.Hashers;

public sealed class Blake2BConfig : ICloneable
{
	public byte[]? Personalization { get; private set; }
	public byte[]? Salt { get; private set; }
	public byte[]? Key { get; private set; }
	public int OutputSizeInBytes { get; init; } = 64;

	private Blake2BConfig Clone()
	{
		var result = new Blake2BConfig
		{
			OutputSizeInBytes = OutputSizeInBytes
		};
		if (Key != null)
			result.Key = (byte[])Key.Clone();
		if (Personalization != null)
			result.Personalization = (byte[])Personalization.Clone();
		if (Salt != null)
			result.Salt = (byte[])Salt.Clone();
		return result;
	}

	object ICloneable.Clone()
	{
		return Clone();
	}
}