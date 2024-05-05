using System.Linq;

public class TempleRequestHandler
{
    private ServerPlayer _serverPlayer;

    public TempleRequestHandler(ServerPlayer serverPlayer)
    {
        _serverPlayer = serverPlayer;
    }

    public void HandleRequest(TempleActionRequestData data)
    {
        var serverTemple = GetServerTemple(data);
        switch (data.Request)
        {
            case TempleActionRequest.RecruitFollower:
            {
                HandleTempleRecruitFollowerRequest(serverTemple);
                break;
            }
            case TempleActionRequest.ConvertToFireTemple:
            {
                HandleConvertToFireTempleRequest(serverTemple);
                break;
            }
            default:
            {
                break;
            }
        }
    }

    private ServerTemple GetServerTemple(TempleIndexData data)
    {
        return _serverPlayer.ServerTemples[data.TempleIndex];
    }

    private void HandleTempleRecruitFollowerRequest(ServerTemple serverTemple)
    {
        if (_serverPlayer.Player.Glory < Follower.Cost)
        {
            return;
        }

        _serverPlayer.Player.Glory -= Follower.Cost;
        serverTemple.QueueNewFollower();
    }

    private void HandleConvertToFireTempleRequest(ServerTemple serverTemple)
    {
        var alreadyQueued = serverTemple.Temple.TaskQueue.Any(t => t.Type == ProgressItemType.ConvertToFireTemple);
        if (_serverPlayer.Player.Glory < Temple.ConvertCost || serverTemple.Temple.Element != Element.None || alreadyQueued)
        {
            return;
        }

        _serverPlayer.Player.Glory -= Temple.ConvertCost;
        serverTemple.QueueConvertToElement(Element.Fire);
    }
}