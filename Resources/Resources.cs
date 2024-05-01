using System.Collections.Generic;
using Godot;

public static class Resources
{
    private const string ResourcesPrefix = "res://Resources/";

    private const string FollowerIconPath = ResourcesPrefix + "FollowerIcon.bmp";
    private static Texture2D _followerIcon;
    public static Texture2D FollowerIcon
    { 
        get
        {
            if (_followerIcon is null)
            {
                _followerIcon = GD.Load<Texture2D>(FollowerIconPath);
            }
            return _followerIcon;
        }
    }

    private const string BlankIconPath = ResourcesPrefix + "ButtonIcon.bmp";
    private static Texture2D _blankIcon;
    public static Texture2D BlankIcon
    {
        get
        {
            if (_blankIcon is null)
            {
                _blankIcon = GD.Load<Texture2D>(BlankIconPath);
            }
            return _blankIcon;
        }
    }

    private const string GrayIconPath = ResourcesPrefix + "GrayIcon.bmp";
    private static Texture2D _grayIcon;
    public static Texture2D GrayIcon
    {
        get
        {
            if (_grayIcon is null)
            {
                _grayIcon = GD.Load<Texture2D>(BlankIconPath);
            }
            return _grayIcon;
        }
    }

    private const string DemigodIconPath = ResourcesPrefix + "DemigodIcon.bmp";
    private static Texture2D _demigodIcon;
    public static Texture2D DemigodIcon
    {
        get
        {
            if (_demigodIcon is null)
            {
                _demigodIcon = GD.Load<Texture2D>(DemigodIconPath);
            }
            return _demigodIcon;
        }
    }

    private const string ImpIconPath = ResourcesPrefix + "ImpIcon.png";
    private static Texture2D _impIcon;
    public static Texture2D ImpIcon
    {
        get
        {
            if (_impIcon is null)
            {
                _impIcon = GD.Load<Texture2D>(ImpIconPath);
            }
            return _impIcon;
        }
    }

    private const string FlameIconPath = ResourcesPrefix + "FlameIcon.png";
    private static Texture2D _flameIcon;
    public static Texture2D FlameIcon
    {
        get
        {
            if (_flameIcon is null)
            {
                _flameIcon = GD.Load<Texture2D>(FlameIconPath);
            }
            return _flameIcon;
        }
    }

    private const string TempleIconPath = ResourcesPrefix + "Temple.png";
    private static Texture2D _templeIcon;
    public static Texture2D TempleIcon
    {
        get
        {
            if (_templeIcon is null)
            {
                _templeIcon = GD.Load<Texture2D>(TempleIconPath);
            }
            return _templeIcon;
        }
    }

    private const string FireTempleIconPath = ResourcesPrefix + "FireTemple.png";
    private static Texture2D _fireTempleIcon;
    public static Texture2D FireTempleIcon
    {
        get
        {
            if (_fireTempleIcon is null)
            {
                _fireTempleIcon = GD.Load<Texture2D>(FireTempleIconPath);
            }
            return _fireTempleIcon;
        }
    }

    private const string TempleFoundationIconPath = ResourcesPrefix + "TempleFoundation.png";
    private static Texture2D _templeFoundationIcon;
    public static Texture2D TempleFoundationIcon
    {
        get
        {
            if (_templeFoundationIcon is null)
            {
                _templeFoundationIcon = GD.Load<Texture2D>(TempleFoundationIconPath);
            }
            return _templeFoundationIcon;
        }
    }

    private const string MainTempleIconPath = ResourcesPrefix + "MainTempleIcon.png";
    private static Texture2D _mainTempleIcon;
    public static Texture2D MainTempleIcon
    {
        get
        {
            if (_mainTempleIcon is null)
            {
                _mainTempleIcon = GD.Load<Texture2D>(MainTempleIconPath);
            }
            return _mainTempleIcon;
        }
    }

    private const string SummonGateIconPath = ResourcesPrefix + "GateMediumIcon.png";
    private static Texture2D _summonGateIcon;
    public static Texture2D SummonGateIcon
    {
        get
        {
            if (_summonGateIcon is null)
            {
                _summonGateIcon = GD.Load<Texture2D>(SummonGateIconPath);
            }
            return _summonGateIcon;
        }
    }

    public static readonly Dictionary<ProgressItemType, Texture2D> ProgressItemTextures = new()
    {
        { ProgressItemType.RecruitingFollower, FollowerIcon },
        { ProgressItemType.ConvertToFireTemple, FlameIcon },
        { ProgressItemType.UnlockFireImp, ImpIcon },
        { ProgressItemType.BuildTemple, TempleIcon }
    };
}