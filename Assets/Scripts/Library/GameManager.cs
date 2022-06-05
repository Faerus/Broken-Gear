using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameManager
{
    public const int GRID_NEUTRAL_COLUMN = 3;

    public const int MAX_ROBOT_PER_TEAM = 20;
    public const int MAX_GEARS_PER_TEAM = 99;

    public const int PRICE_ROBOT = 5;
    public const int PRICE_PER_ROBOT_KILLED = 5;
    public const int PRICE_PER_BUILDING_KILLED = 20;
    public const int PRICE_DRILL = 40;
    public const int PRICE_FACTORY = 50;
    public const int PRICE_TURRET = 20;

    public static event EventHandler OnScoreChanged;

    public static Grid<Cell> Grid { get; set; }
    public static int TotalRobotCreated { get; private set; }
    public static Dictionary<Color, int> RobotCreated { get; private set; }
    public static Dictionary<Color, int> RobotDead { get; private set; }
    public static Dictionary<Color, int> Gears { get; private set; }

    public static BuildingTypes SelectedBuilding { get; set; }
    public static List<Actor> Actors { get; set; }

    public static Color ColorTeam1 { get; set; }
    public static Color ColorTeam2 { get; set; }

    public static void Initialize(Color team1, Color team2)
    {
        ColorTeam1 = team1;
        ColorTeam2 = team2;

        Grid = new Grid<Cell>(7, 5, 2.4f, 2.4f, new Vector3(-7.8f, -6f, 0), (grid, x, y) => new Cell(grid, x, y));

        Actors = new List<Actor>();
        RobotCreated = new Dictionary<Color, int>();
        RobotDead = new Dictionary<Color, int>();
        Gears = new Dictionary<Color, int>();
        SelectedBuilding = BuildingTypes.None;
    }

    public static T GetClosest<T>(Vector3 position, float maxRange, Color team, T exclusion = null) where T : Actor
    {
        var enemies = Actors
            .OfType<T>()
            .Where(b =>
                b != exclusion
                && b.HealthSystem != null
                && !b.HealthSystem.IsDead()
                && b.TeamColor == team
                && Vector3.Distance(position, b.transform.position) <= maxRange
            ).ToList();

        T closest = null;
        foreach (T enemy in enemies)
        {
            if (closest == null)
            {
                closest = enemy;
            }
            else if (Vector3.Distance(position, enemy.transform.position) < Vector3.Distance(position, closest.transform.position))
            {
                closest = enemy;
            }
        }

        return closest;
    }

    public static T GetClosestEnemy<T>(Vector3 position, float maxRange, Color myTeam) where T : Actor
    {
        return GetClosest<T>(position, maxRange, GetOtherTeamColor(myTeam));
    }

    public static T GetWeakest<T>(Vector3 position, float maxRange, Color myTeam) where T : Actor
    {
        var enemies = Actors.OfType<T>().Where(b =>
            b.HealthSystem != null
            && !b.HealthSystem.IsDead()
            && b.TeamColor == myTeam
            && Vector3.Distance(position, b.transform.position) <= maxRange
        ).ToList();

        T weakest = null;
        foreach (T enemy in enemies)
        {
            if (weakest == null)
            {
                weakest = enemy;
            }
            else if (enemy.HealthSystem.Health < weakest.HealthSystem.Health)
            {
                weakest = enemy;
            }
        }

        return weakest;
    }

    public static T GetWeakestEnemy<T>(Vector3 position, float maxRange, Color myTeam) where T : Actor
    {
        return GetWeakest<T>(position, maxRange, GetOtherTeamColor(myTeam));
    }

    public static void RegisterRobotCreated(Color team, int amount = 1)
    {
        if (!RobotCreated.ContainsKey(team))
        {
            RobotCreated.Add(team, 0);
        }

        RobotCreated[team] += amount;
        ++TotalRobotCreated;
        OnScoreChanged?.Invoke(null, EventArgs.Empty);
    }

    public static void RegisterRobotDead(Color team, int amount = 1)
    {
        if (!RobotDead.ContainsKey(team))
        {
            RobotDead.Add(team, 0);
        }

        RobotDead[team] += amount;
        OnScoreChanged?.Invoke(null, EventArgs.Empty);
    }

    public static void AddGears(Color team, int amount = 10)
    {
        if (!Gears.ContainsKey(team))
        {
            Gears.Add(team, 0);
        }

        int gears = Gears[team] + amount;
        if (gears >= MAX_GEARS_PER_TEAM)
        { 
            gears = MAX_GEARS_PER_TEAM;
        }
        Gears[team] = gears;

        OnScoreChanged?.Invoke(null, EventArgs.Empty);
    }

    public static void AddGearsToOtherTeam(Color team, int amount = 10)
    {
        Color otherTeam = GetOtherTeamColor(team);
        AddGears(otherTeam, amount);
    }

    public static bool RemoveGears(Color team, int amount = 10)
    {
        if (!Gears.ContainsKey(team))
        {
            Gears.Add(team, 0);
        }

        int gears = Gears[team];
        if(gears < amount)
        {
            return false;
        }

        Gears[team] -= amount;
        OnScoreChanged?.Invoke(null, EventArgs.Empty);
        return true;
    }

    public static int GetRobotCount(Color team)
    {
        int created = RobotCreated.ContainsKey(team) ? RobotCreated[team] : 0;
        int dead = RobotDead.ContainsKey(team) ? RobotDead[team] : 0;

        return created - dead;
    }

    public static Color GetOtherTeamColor(Color myTeam)
    {
        return ColorTeam1 == myTeam ? ColorTeam2 : ColorTeam1;
    }

    public static bool HasGameEnded(out Color winnerTeam)
    {
        if(HasTeamLost(ColorTeam1))
        {
            winnerTeam = ColorTeam2;
            return true;
        }

        if (HasTeamLost(ColorTeam2))
        {
            winnerTeam = ColorTeam1;
            return true;
        }

        winnerTeam = Color.black;
        return false;
    }

    public static bool HasTeamLost(Color team)
    {
        int robotCreated = RobotCreated.ContainsKey(team) ? RobotCreated[team] : 0;
        int robotDead = RobotDead.ContainsKey(team) ? RobotDead[team] : 0;
        int teamRobotCount = robotCreated - robotDead;

        int teamBuildingCount = Actors.OfType<Building>().Count(b => b.HealthSystem != null && !b.HealthSystem.IsDead() && b.TeamColor == team);
        
        int teamGears = Gears[team];

        return teamRobotCount < 1 && teamBuildingCount < 1 && teamGears < 20;
    }
}

