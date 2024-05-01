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
        switch (data.Request)
        {
            case TempleRequest.BuildTemple:
            {
                HandleBuildTempleRequest(data);
                break;
            }
            case TempleRequest.RecruitFollower:
            {
                HandleTempleRecruitFollowerRequest(data);
                break;
            }
            case TempleRequest.ConvertToFireTemple:
            {
                HandleConvertToFireTempleRequest(data);
                break;
            }
            case TempleRequest.UnlockFireImp:
            {
                HandleUnlockFireImpRequest(data);
                break;
            }
            default:
            {
                break;
            }
        }
    }

    private void HandleTempleRecruitFollowerRequest(TempleRequestData data)
    {
        if (_serverPlayer.Player.Glory < Follower.Cost)
        {
            return;
        }

        _serverPlayer.Player.Glory -= Follower.Cost;
        _serverPlayer.ServerTemples[data.TempleIndex].QueueNewFollower();
    }

    private void HandleBuildTempleRequest(TempleRequestData data)
    {
        var serverTemple = _serverPlayer.ServerTemples[data.TempleIndex];
        var alreadyQueued = serverTemple.Temple.TaskQueue.Any(t => t.Type == ProgressItemType.BuildTemple);
        if (_serverPlayer.Player.Glory < Temple.BuildCost || serverTemple.Temple.IsActive || alreadyQueued)
        {
            return;
        }

        _serverPlayer.Player.Glory -= Temple.BuildCost;
        serverTemple.QueueBuild();
    }

    private void HandleConvertToFireTempleRequest(TempleRequestData data)
    {
        var serverTemple = _serverPlayer.ServerTemples[data.TempleIndex];
        var alreadyQueued = serverTemple.Temple.TaskQueue.Any(t => t.Type == ProgressItemType.ConvertToFireTemple);
        if (_serverPlayer.Player.Glory < Temple.ConvertCost || serverTemple.Temple.Element != Element.None || alreadyQueued)
        {
            return;
        }

        _serverPlayer.Player.Glory -= Temple.ConvertCost;
        serverTemple.QueueConvertToElement(Element.Fire);
    }

    private void HandleUnlockFireImpRequest(TempleRequestData data)
    {
        var serverTemple = _serverPlayer.ServerTemples[data.TempleIndex];
        var alreadyQueued = serverTemple.Temple.TaskQueue.Any(t => t.Type == ProgressItemType.ConvertToFireTemple);
        if (serverTemple.Temple.Element != Element.Fire
            || _serverPlayer.Player.Glory < Enemies.FireImpInfo.UnlockGloryCost
            || _serverPlayer.Player.Tech.FireTech.HasFlag(FireTech.FlameImp)
            || alreadyQueued)
        {
            return;
        }

        _serverPlayer.Player.Glory -= Enemies.FireImpInfo.UnlockGloryCost;
        _serverPlayer.ServerTemples[data.TempleIndex].QueueUnlockFlameImp();
    }
}