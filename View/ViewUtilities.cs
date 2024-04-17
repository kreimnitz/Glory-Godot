using Godot;

public static class ViewUtilites
{
    public static bool IsLeftMouseRelease(InputEvent e)
    {
        return e is InputEventMouseButton mouseEvent
            && !mouseEvent.Pressed
            && mouseEvent.ButtonIndex == MouseButton.Left;
    }
}