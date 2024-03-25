using System;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
[ProtoInclude(7, typeof(ServerRegion))]
public class Region : IUpdateFrom<Region>
{
    public Guid Id { get; }= IdGenerator.Generate();

    public List<Spawner> Spawners { get; set; }

    public void UpdateFrom(Region other)
    {
    }
}