using System.Drawing;
using Core;
using UnityEngine;
using Color = UnityEngine.Color;

public static class Constants
{
	#region GAMEPLAY CONSTANTS

	// Ball constants
	public const float BALL_BASE_VELOCITY = 4.5f;
	public const float BASE_BALL_RADIUS = 0.25f;
	public const float BALL_VELOCITY_RATIO = 0.33f;

	public const float BOUNCE_MAX_X_VELOCITY_AS_PERCENTAGE = 0.5f; // Max bounce velocity as a percentage of max velocity
	
	// Powerup Constants
	public const float POWERUP_KICKOFF_VELOCITY = 3.3f;
	public const float POWERUP_KICKOFF_ANGULAR_VEL_MAX = 50f;
	public const int POWERUP_MIN_BOUNCES = 3;
	public const int POWERUP_MAX_BOUNCES = 8;
	
	// Paddle Upsize Effect
	public const float PADDLE_SIZE_PLUS_EFFECT_TIME = 10f;
	
	public const float PADDLE_SIZE_PLUS_SCALE_TWEEN_TIME = 1f;
	public const float PADDLE_STANDARD_WIDTH = 1.0f;
	public const float PADDLE_UPSIZE_ACTIVE_WIDTH = 1.8f;

	// Center Lockout Effect
	public const float CENTER_LOCKOUT_EFFECT_TIME = 8f;
	
	// Paddle Drive Effect
	public const float PADDLE_DRIVE_EFFECT_TIME = 12f;
	
	public const float PADDLE_BASE_FORCE_MODIFIER = 1f;
	public const float PADDLE_DRIVE_FORCE_MODIFIER = 1.65f;
	
	// Homing Ball Effect
	public const float HOMING_BALL_EFFECT_TIME = 15f;
	
	public const float HOMING_BALL_MAX_HOMING_FORCE = 4.25f;
	public const float HOMING_BALL_ACCEL_FACTOR = 25f;
	public const float HOMING_BALL_DAMP_FACTOR = 60f;
	
	public const float HOMING_COLOUR_FLASH_TIME = 0.5f;



	// Field constants
	public const float SIDE_WALL_RECENT_COLLISION_COOLDOWN_TIME = 0.125f;
	public const float PADDLE_RECENT_COLLISION_COOLDOWN_TIME = 0.125f;
	public const float SIDE_WALL_MARGIN_X = 0.0125f;
	public const float GOAL_MARGIN = 0.0250f;
	
	// Paddle constants
	public static readonly Vector2 RED_PADDLE_START_POS = new Vector2(0f, 3.5f);
	public static readonly Vector2 BLUE_PADDLE_START_POS = new Vector2(0f, -3.5f);

	public const float PADDLE_BASE_MOVE_DELTA = 999f;
	public const float BASE_PADDLE_WIDTH = 1f;
	
	
	// Match constants
	public const float MATCH_START_COOLDOWN = 2f;
	public const float MATCH_END_COOLDOWN = 1.5f;
	public static readonly MatchData DEFAULT_MATCH_DATA = new MatchData(3, MatchType.VERSUS, MatchDifficulty.EASY);
	
	// Game constants
	public const string GAME_MANAGER_TAG = "GameManager";

	// Gameplay particles


	#endregion
}
