using System.Collections;
using System.Linq;

public class ClientRequestHandler
{
    private ServerPlayer _serverPlayer;
    private TempleRequestHandler _templeRequestHandler;

    public ClientRequestHandler(ServerPlayer player)
    {
        _serverPlayer = player;
        _templeRequestHandler = new(player);
    }

    public void HandleRecruitFollowerRequest()
    {
        if (_serverPlayer.Player.Glory < Follower.Cost)
        {
            return;
        }

        _serverPlayer.Player.Glory -= Follower.Cost;
        _serverPlayer.QueueNewFollower();
    }

    public void HandleTempleRequest(TempleRequestData data)
    {
        _templeRequestHandler.HandleRequest(data);
    }
}