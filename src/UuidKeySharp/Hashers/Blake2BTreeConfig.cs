// BLAKE2 reference source code package - C# implementation

// Written in 2012 by Christian Winnerlein  <codesinchaos@gmail.com>

// To the extent possible under law, the author(s) have dedicated all copyright
// and related and neighboring rights to this software to the public domain
// worldwide. This software is distributed without any warranty.

// You should have received a copy of the CC0 Public Domain Dedication along with
// this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.

namespace UuidKeySharp.Hashers;

public sealed class Blake2BTreeConfig : ICloneable
{
	public int IntermediateHashSize { get; init; } = 64;
	public int MaxHeight { get; init; }
	public long LeafSize { get; init; }
	public int FanOut { get; init; }

	private Blake2BTreeConfig Clone()
	{
		var result = new Blake2BTreeConfig
		{
			IntermediateHashSize = IntermediateHashSize,
			MaxHeight = MaxHeight,
			LeafSize = LeafSize,
			FanOut = FanOut
		};
		return result;
	}

	object ICloneable.Clone()
	{
		return Clone();
	}
}
