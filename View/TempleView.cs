using System.Collections.Generic;
using Godot;

public class TempleView : IButtonGroupHandler
{
    private Player _player;
    private Temple _temple;
    private int _templeIndex;
    public TextureButton Button { get; }

    public TempleView(Temple temple, TextureButton button, Player player)
    {
        _temple = temple;
        _player = player;
        Button = button;
        Button.Pressed += () => SelectTemple();

        for (int i = 0; i < Player.TempleCount; i++)
        {
            if (_player.Temples[i] == _temple)
            {
                _templeIndex = i;
            }
        }
    }

    public void _Process()
    {
        if (Button.TextureNormal == Resources.TempleFoundationIcon && _temple.IsActive)
        {
            Button.TextureNormal = Resources.TempleIcon;
        }
    }

    private void SelectTemple()
    {
        SelectionManager.Instance.Selection = this;
        SelectionManager.Instance.ShowButtonGroup(this, GetButtonContext());
        SelectionManager.Instance.ShowProgressQueue(Resources.TempleIcon, "Temple", _temple.TaskQueue);
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
        if (_player.Tech.HasFlag(PlayerTech.FlameImp))
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
                if (row == 1 && column == 1)
                {
                    if (_player.Tech.HasFlag(PlayerTech.FlameImp))
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