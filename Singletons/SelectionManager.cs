using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
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

    private object _selection;
    public object Selection
    { 
        get { return _selection; }
        set
        {
            _uiBar.ClearSelection();
            _selection = value;
        }
    }

    public void ShowButtonGroup(IButtonGroupHandler handler, IEnumerable<ButtonContext> buttonContext)
    {
        _uiBar.ButtonGroup.SetHandlerAndVisuals(handler, buttonContext);
        _uiBar.ButtonGroup.Show();
    }

    public void ShowProgressQueue(Texture2D texture, string label, List<ProgressItem> progressItems)
    {
        _uiBar.ProgressQueueUi.Label.Text = label;
        _uiBar.ProgressQueueUi.MainTexture.Texture = texture;
        _uiBar.ProgressQueueUi.SetTasks(progressItems);
        _uiBar.ProgressQueueUi.Show();
    }

    public void ShowUnitInfoUi(IUnitModel model)
    {
        _uiBar.UnitInfoUi.SetModel(model);
        _uiBar.UnitInfoUi.Show();
    }
}