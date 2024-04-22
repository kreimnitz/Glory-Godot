using System.Collections.Generic;
using Godot;

public class SelectionManager
{
    public static SelectionManager Instance { get; private set; }
    private UiBar _uiBar;

    private SelectionManager(UiBar uiBar)
    {
        _uiBar = uiBar;
    }

    public static void CreateSingleton(UiBar uiBar)
    {
        if (Instance is null)
        {
            Instance = new SelectionManager(uiBar);
        }
    }

    public object Selection { get; set; }

    public void ShowButtonGroup(IButtonGroupHandler handler, IEnumerable<ButtonContext> buttonContext)
    {
        _uiBar.ButtonGroup.SetHandlerAndVisuals(handler, buttonContext);
    }

    public void ShowProgressQueue(Texture2D texture, string label, List<ProgressItem> progressItems)
    {
        _uiBar.ProgressQueueUi.Label.Text = label;
        _uiBar.ProgressQueueUi.MainTexture.Texture = texture;
        _uiBar.ProgressQueueUi.SetTasks(progressItems);
    }
}