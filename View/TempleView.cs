using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class TempleView : TextureButton, IButtonGroupHandler
{
    private const string TempleLabel = "Temple";
    private const string TempleFoundationLabel = "Temple Foundation";
    private const string FireTempleLabel = "Fire Temple";

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

        yield return GetRecruitFollowerButtonContext();
        if (_temple.Element == Element.None)
        {
            yield return GetConvertToFireTempleButtonContext();
        }
        if (_temple.Element == Element.Fire)
        {
            if (TryGetFireImpButtonContext() is var bc && bc is not null)
            {
                yield return bc;
            }
        }
    }

    private ButtonContext GetBuildTempleButton()
    {
        string tooltip = $"Build Temple\nCost: {Temple.BuildCost}";
        return new ButtonContext(0, 0, Resources.TempleIcon, tooltip);
    }

    private ButtonContext GetRecruitFollowerButtonContext()
    {
        return new ButtonContext(0, 0, Resources.FollowerIcon, Follower.RecruitTooltip);
    }

    private ButtonContext GetConvertToFireTempleButtonContext()
    {
        string tooltip = $"Upgrade to Fire Temple\nCost: {Temple.ConvertCost}";
        return new ButtonContext(1, 0, Resources.FlameIcon, tooltip);
    }

    private ButtonContext TryGetFireImpButtonContext()
    {
        if (_player.Tech.FireTech.HasFlag(FireTech.FireImp))
        {
            return null;
        }
        string tooltip = $"Unlock Fire Imp\nCost: {Tech.FireImpTechInfo.GloryCost}";
        return new ButtonContext(1, 0, Resources.ImpIcon, tooltip);
    }

    public void GridButtonPressed(int row, int column)
    {
        if (_temple is null)
        {
            ClientMessageManager.Instance.SendBuildTempleRequest(_position);
            return;
        }

        var requestType = GetRequestType(row, column);
        if (requestType is null)
        {
            return;
        }
        ClientMessageManager.Instance.SendTempleRequest(requestType.Value, _templeIndex);
    }

    private TempleActionRequest? GetRequestType(int row, int column)
    {
        if (row == 0 && column == 0)
        {
            return TempleActionRequest.RecruitFollower;
        }

        switch (_temple.Element)
        {
            case Element.None:
            {
                if (row == 1 && column == 0)
                {
                    return TempleActionRequest.ConvertToFireTemple;
                }
                break;
            }
            default:
                break;
        }
        return null;
    }
}