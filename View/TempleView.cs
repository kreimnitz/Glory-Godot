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

    private IEnumerable<ButtonContext> GetButtonContext()
    {
        if (!_temple.IsActive)
        {
            // return build temple button
            yield break;
        }

        yield return GetRecruitFollowerButtonContext();
        if (_temple.Element == Element.None)
        {
            yield return GetConvertToFireTempleButtonContext();
        }
        if (_temple.Element == Element.Fire)
        {
            yield return GetFireImpButtonContext();
        }
    }

    private ButtonContext GetRecruitFollowerButtonContext()
    {
        return new ButtonContext()
        {
            Column = 0,
            Row = 0,
            Texture = Resources.FollowerIcon,
            LabelInfo = null
        };
    }

    private ButtonContext GetConvertToFireTempleButtonContext()
    {
        return new ButtonContext()
        {
            Column = 0,
            Row = 1,
            Texture = Resources.FlameIcon,
            LabelInfo = null
        };
    }

    private ButtonContext GetFireImpButtonContext()
    {
        IProgressInfo labelInfo;
        if (_player.Tech.FireTech.HasFlag(FireTech.FlameImp))
        {
            labelInfo = _temple.GetSpawnerForType(EnemyType.FireImp);
        }
        else
        {
            labelInfo = null;
        }
        
        return new ButtonContext()
        {
            Column = 0,
            Row = 1,
            Texture = Resources.ImpIcon,
            LabelInfo = labelInfo
        };
    }

    public void Execute(int row, int column)
    {
        var requestType = GetRequestType(row, column);
        if (requestType is null)
        {
            return;
        }
        ClientMessageManager.Instance.SendTempleIndexMessage(requestType.Value, _templeIndex);
    }

    private ClientRequestType? GetRequestType(int row, int column)
    {
        if (row == 0 && column == 0)
        {
            return ClientRequestType.AddFollower;
        }

        switch (_temple.Element)
        {
            case Element.None:
            {
                if (row == 1 && column == 0)
                {
                    return ClientRequestType.ConvertToFireTemple;
                }
                break;
            }
            case Element.Fire:
            {
                if (row == 1 && column == 0)
                {
                    if (_player.Tech.FireTech.HasFlag(FireTech.FlameImp))
                    {
                        return ClientRequestType.SpawnFireImp;
                    }
                    return ClientRequestType.UnlockFireImp;
                }
                break;
            }
            default:
                break;
        }
        return null;
    }
}