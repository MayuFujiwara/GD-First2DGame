using Godot;
using System;
using System.Numerics;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();

	private void OnBodyEntered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	[Export]
	public int Speed{get; set;} = 400;
	
	public Godot.Vector2 ScreenSize;
	
	public void Start(Godot.Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Godot.Vector2.Zero;
		//check for input (4 directions)
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left")){
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if(velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
		}
		else
		{
			animatedSprite2D.Stop();
		}
		//move in the given direction
		Position += velocity * (float)delta;
		Position = new Godot.Vector2(x: Mathf.Clamp(Position.X, 0, ScreenSize.X), y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y));

		//play the appropriate animation
		if(velocity.X != 0)
		{
			animatedSprite2D.Animation = "walk";
			animatedSprite2D.FlipV = false;

			animatedSprite2D.FlipH = velocity.X < 0;
		}else if (velocity.Y != 0)
		{
			animatedSprite2D.Animation = "up";
			animatedSprite2D.FlipV = velocity.Y > 0; 

			animatedSprite2D.FlipH = false;
		}
	}
}
