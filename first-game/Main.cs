using Godot;
using System;
using System.ComponentModel;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene{get; set;}

	private int _score;

	private void OnScoreTimerTimeout()
	{
		_score++;
		GetNode<Hud>("HUD").UpdateScore(_score);
	}

	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	private void OnMobTimerTimeout()
	{
		//create a new instance of the Mob Scene
		Mob mob = MobScene.Instantiate<Mob>();

		//choose a random location on Path2D
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		//set the mob's direction perpendicular to the path direction. 
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		//Set the mob's position to a random location 
		mob.Position = mobSpawnLocation.Position;

		//add some randomness to teh direction. 
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;

		//choose the velocity
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);

		//spawn the mob by adding it to the Main scene
		AddChild(mob);
	}

	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();

		GetNode<Hud>("HUD").ShowGameOver();
	}

	public void NewGame()
	{
		_score = 0; 

		var player = GetNode<Player>("Player");
		var StartPosition = GetNode<Marker2D>("StartPosition");
		player.Start(StartPosition.Position);

		GetNode<Timer>("StartTimer").Start();

		var hud = GetNode<Hud>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//NewGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
