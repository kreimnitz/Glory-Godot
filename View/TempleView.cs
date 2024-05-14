using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class TempleView : TextureButton, IButtonGroupHandler
{
    private const string TempleLabel = "Temple";
    private const string TempleFoundationLabel = "Temple Foundation";
    private const string FireTempleLabel = "Fire Temple";

    private static List<TempleButtonInfo> _normalPositionList = new()
    {
        new (1, 0, TempleActionRequest.ConvertToFireTemple),
    };

    private static List<TempleButtonInfo> _fireTypePositionList = new()
    {
    };

    private ButtonContext _recruitFollowerButtonContext =
        new ButtonContext(0, 0, Resources.FollowerIcon, Follower.RecruitTooltip);

    private int _position;
    private Player _player;
    private Temple _temple;
    private int _templeIndex = -1;
    private ActionQueue _actionQueue = new();

    public void SetModel(int position, Player player)
    {
        _position = position;
        TextureNormal = GetTempleTexture();
        Pressed += () => SelectTemple();
        _player = player;
        SetTempleIfExists();
    }

    public void _Process()
    {
        _actionQueue.ExecuteActions();
    }

    public bool ProcessModelUpdate(PlayerUpdateInfo playerUpdateInfo)
    {
        if (_temple == null && !SetTempleIfExists())
        {
            return true;
        }

        if (playerUpdateInfo.AddedTech.FireTech.HasFlag(FireTech.FireImp))
        {
            _actionQueue.Add(RefreshVisuals);
            return false;
        }

        if (playerUpdateInfo.TempleUpdates.Updated.TryGetValue(_temple, out PropertyUpdateInfo templeUpdateInfo))
        {
            if (templeUpdateInfo.Contains(nameof(Temple.Element))
                || templeUpdateInfo.Contains(nameof(Temple.IsActive)))
            {
                _actionQueue.Add(RefreshVisuals);
                return true;
            }
        }
        return false;
    }

    private bool SetTempleIfExists()
    {
        var temple = _player.Temples.FirstOrDefault(t => t.Position == _position);
        if (temple is not null)
        {
            _temple = temple;
            _templeIndex = _player.Temples.IndexOf(temple);
            _actionQueue.Add(RefreshVisuals);
            return true;
        }
        return false;
    }

    private void RefreshVisuals()
    {
        TextureNormal = GetTempleTexture();
        if (SelectionManager.Instance.Selection == this)
        {
            SelectTemple();
        }
    }

    private List<ProgressItem> _emptyProgressItemList = new();
    private void SelectTemple()
    {
        SelectionManager.Instance.Selection = this;
        SelectionManager.Instance.ShowButtonGroup(this, GetButtonContext());
        var tasks = _temple?.TaskQueue ?? _emptyProgressItemList;
        SelectionManager.Instance.ShowProgressQueue(GetTempleTexture(), GetTempleLabel(), tasks);
    }

    private Texture2D GetTempleTexture()
    {
        if (_temple is null || !_temple.IsActive)
        {
            return Resources.TempleFoundationIcon;
        }
        switch (_temple.Element)
        {
            case Element.Fire: return Resources.FireTempleIcon;
            default: return Resources.TempleIcon;
        }
    }

    private string GetTempleLabel()
    {
        if (_temple is null || !_temple.IsActive)
        {
            return TempleFoundationLabel;
        }
        switch (_temple.Element)
        {
            case Element.Fire: return FireTempleLabel;
            default: return TempleLabel;
        }
    }

    public IEnumerable<ButtonContext> GetButtonContext()
    {
        if (_temple is null)
        {
            yield return GetBuildTempleButton();
            yield break;
        }

        if (!_temple.IsActive)
        {
            yield break;
        }

        yield return _recruitFollowerButtonContext;
        if (_temple.Element == Element.None)
        {
            yield return GetConvertToFireTempleButtonContext();
        }
        if (_temple.Element == Element.Fire)
        {

        }
    }

    private ButtonContext GetBuildTempleButton()
    {
        string tooltip = $"Build Temple\nCost: {Temple.BuildCost}";
        return new ButtonContext(0, 0, Resources.TempleIcon, tooltip);
    }

    private ButtonContext GetConvertToFireTempleButtonContext()
    {
        string tooltip = $"Upgrade to Fire Temple\nCost: {Temple.ConvertCost}";
        return new ButtonContext(1, 0, Resources.FlameIcon, tooltip);
    }

    public void GridButtonPressed(int row, int column)
    {
        if (_temple is null)
        {
            ClientMessageManager.Instance.SendBuildTempleRequest(_position);
            return;
        }

        if (row == 0 && column == 0)
        {
            ClientMessageManager.Instance.SendTempleRequest(TempleActionRequest.RecruitFollower, _templeIndex);
            return;
        }

        var buttonInfo = GetInfo(row, column);
        if (buttonInfo is null)
        {
            return;
        }
        else if (buttonInfo.ActionRequest.HasValue)
        {
            ClientMessageManager.Instance.SendTempleRequest(buttonInfo.ActionRequest.Value, _templeIndex);
        }
        else if (buttonInfo.TechRequest is not null)
        {
            ClientMessageManager.Instance.SendTempleTechRequest(buttonInfo.TechRequest, _templeIndex);
        }
    }

    private TempleButtonInfo GetInfo(int row, int column)
    {
        List<TempleButtonInfo> positionList;
        switch (_temple.Element)
        {
            case Element.Fire:
            {
                positionList = _fireTypePositionList;
                break;
            }
            default:
            {
                positionList = _normalPositionList;
                break;
            }
        }
        return positionList.FirstOrDefault(bi => bi.Row == row && bi.Column == column);
    }

    private class TempleButtonInfo
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public TempleActionRequest? ActionRequest { get; set; } = null;
        public PlayerTech TechRequest { get; set; } = null;

        public TempleButtonInfo(int row, int column, TempleActionRequest request)
        {
            Row = row;
            Column = column;
            ActionRequest = request;
        }

        public TempleButtonInfo(int row, int column, PlayerTech request)
        {
            Row = row;
            Column = column;
            TechRequest = request;
        }
    }
}