using System.Collections.Generic;
using System.ComponentModel;
using Godot;

public class TempleView : IButtonGroupHandler
{
    private const string TempleLabel = "Temple";
    private const string TempleFoundationLabel = "Temple Foundation";
    private const string FireTempleLabel = "Fire Temple";

    private Player _player;
    private Temple _temple;
    private int _templeIndex;
    private ActionQueue _actionQueue = new();

    public TextureButton Button { get; }

    public TempleView(Temple temple, TextureButton button, Player player)
    {
        _temple = temple;
        _player = player;
        Button = button;
        Button.TextureNormal = GetTempleTexture();
        Button.Pressed += () => SelectTemple();

        for (int i = 0; i < Player.TempleCount; i++)
        {
            if (_player.Temples[i] == _temple)
            {
                _templeIndex = i;
            }
        }

        temple.PropertyChanged += OnModelChanged;
        player.Tech.OnTechUpdate += OnTechUpdate;
    }

    public void _Process()
    {
        _actionQueue.ExecuteActions();
    }

    private void OnTechUpdate(object sender, TechUpdateEventArgs e)
    {
        if (e.NewTech.FireTech.HasFlag(FireTech.FlameImp))
        {
            _actionQueue.Add(RefreshVisuals);
        }
    }

    private void OnModelChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Temple.Element) || e.PropertyName == nameof(Temple.IsActive))
        {
            _actionQueue.Add(RefreshVisuals);
        }
    }

    private void RefreshVisuals()
    {
        Button.TextureNormal = GetTempleTexture();
        if (SelectionManager.Instance.Selection == this)
        {
            SelectTemple();
        }
    }

    private void SelectTemple()
    {
        SelectionManager.Instance.Selection = this;
        SelectionManager.Instance.ShowButtonGroup(this, GetButtonContext());
        SelectionManager.Instance.ShowProgressQueue(GetTempleTexture(), GetTempleLabel(), _temple.TaskQueue);
    }

    private Texture2D GetTempleTexture()
    {
        if (!_temple.IsActive)
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
        if (!_temple.IsActive)
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
        if (!_temple.IsActive)
        {
            yield return GetBuildTempleButton();
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
        if (_player.Tech.FireTech.HasFlag(FireTech.FlameImp))
        {
            return null;
        }
        string tooltip = $"Unlock Fire Imp\nCost: {Enemies.FireImpInfo.UnlockGloryCost}";
        return new ButtonContext(1, 0, Resources.ImpIcon, tooltip);
    }

    public void GridButtonPressed(int row, int column)
    {
        var requestType = GetRequestType(row, column);
        if (requestType is null)
        {
            return;
        }
        ClientMessageManager.Instance.SendTempleRequest(requestType.Value, _templeIndex);
    }

    private TempleRequest? GetRequestType(int row, int column)
    {
        if (!_temple.IsActive)
        {
            return TempleRequest.BuildTemple;
        }

        if (row == 0 && column == 0)
        {
            return TempleRequest.RecruitFollower;
        }

        switch (_temple.Element)
        {
            case Element.None:
            {
                if (row == 1 && column == 0)
                {
                    return TempleRequest.ConvertToFireTemple;
                }
                break;
            }
            case Element.Fire:
            {
                if (row == 1 && column == 0)
                {
                    return TempleRequest.UnlockFireImp;
                }
                break;
            }
            default:
                break;
        }
        return null;
    }
}