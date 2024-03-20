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
}