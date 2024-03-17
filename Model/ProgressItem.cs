using ProtoBuf;

[ProtoContract]
public class ProgressItem
{
    [ProtoMember(1)]
    public virtual double ProgressRatio { get; protected set; }
}