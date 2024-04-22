using Godot;

public partial class QueueButtonContainer : Control
{
	private Label _label;

	private IProgressInfo _queueInfo;
	public IProgressInfo QueueInfo
	{ 
		get
		{
			return _queueInfo;
		} 
		set
		{
			if (value == null)
			{
				_label.Hide();
			}
			var lastValue = _queueInfo;
			_queueInfo = value;
			if (lastValue is null && value is not null)
			{
				_label.Show();
			}
		} 
	}

	public TextureButton Button { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_label = GetNode<Label>("Label");
		_label.Hide();
		Button = GetNode<ButtonContainer>("ButtonContainer").Button;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (QueueInfo != null)
		{
			_label.Text = $"{QueueInfo.CurrentValue}/{QueueInfo.Max}";
		}
	}
}
