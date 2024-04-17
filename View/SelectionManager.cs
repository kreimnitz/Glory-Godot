using Utilities.Comms;

public class SelectionManager
{
    private UiBar _uiBar;
    private ClientMessageTransmitter _clientMessageTrasmitter;

    public void Initialize(UiBar uiBar, ClientMessageTransmitter clientMessageTransmitter)
    {
        _uiBar = uiBar;

    }

    public void SelectTemple(Player player, int templeIndex)
    {
        var temple = player.Temples[templeIndex];
    }
}