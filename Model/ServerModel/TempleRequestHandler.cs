using System.Linq;

public class TempleRequestHandler
{
    private ServerPlayer _serverPlayer;

    public TempleRequestHandler(ServerPlayer serverPlayer)
    {
        _serverPlayer = serverPlayer;
    }

    public void HandleRequest(TempleRequestData data)
    {
        var serverTemple = GetServerTemple(data);
        switch (data.Request)
        {
            case TempleRequest.BuildTemple:
            {
                HandleBuildTempleRequest(serverTemple);
                break;
            }
            case TempleRequest.RecruitFollower:
            {
                HandleTempleRecruitFollowerRequest(serverTemple);
                break;
            }
            case TempleRequest.ConvertToFireTemple:
            {
                HandleConvertToFireTempleRequest(serverTemple);
                break;
            }
            case TempleRequest.UnlockFireImp:
            {
                HandleUnlockFireImpRequest(serverTemple);
                break;
            }
            default:
            {
                break;
            }
        }
    }

    private ServerTemple GetServerTemple(TempleRequestData data)
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

    private void HandleBuildTempleRequest(ServerTemple serverTemple)
    {
        var alreadyQueued = serverTemple.Temple.TaskQueue.Any(t => t.Type == ProgressItemType.BuildTemple);
        if (_serverPlayer.Player.Glory < Temple.BuildCost || serverTemple.Temple.IsActive || alreadyQueued)
        {
            return;
        }

        _serverPlayer.Player.Glory -= Temple.BuildCost;
        serverTemple.QueueBuild();
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

    private void HandleUnlockFireImpRequest(ServerTemple serverTemple)
    {
        var alreadyQueued = serverTemple.Temple.TaskQueue.Any(t => t.Type == ProgressItemType.ConvertToFireTemple);
        if (serverTemple.Temple.Element != Element.Fire
            || _serverPlayer.Player.Glory < Enemies.FireImpInfo.UnlockGloryCost
            || _serverPlayer.Player.Tech.FireTech.HasFlag(FireTech.FlameImp)
            || alreadyQueued)
        {
            return;
        }

        _serverPlayer.Player.Glory -= Enemies.FireImpInfo.UnlockGloryCost;
        serverTemple.QueueUnlockFlameImp();
    }
}