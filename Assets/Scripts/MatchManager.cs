using UnityEngine;

/// <summary>
/// Tracks score, possession, and match pacing for the Codex Soccer environment.
/// Implementation will be added later; for now the component only exposes
/// inspector-configurable data required by the scene.
/// </summary>
public class MatchManager : MonoBehaviour
{
    [Header("Score State")]
    public int teamAScore;
    public int teamBScore;

    [Header("Match Clock")]
    public float halfLengthSeconds = 180f;
    public bool autoStartMatch = true;

    [Header("Kickoff")]
    public Transform kickoffPoint;
    public AgentController startingPossession;

    [Header("Dependencies")]
    public EnvironmentManager environmentManager;
}
