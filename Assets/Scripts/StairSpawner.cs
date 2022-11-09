using UnityEngine;

public class StairSpawner : MonoBehaviour
{
    public static StairSpawner instance;
    [SerializeField] private Transform stairStepP;
    [SerializeField] private Transform stairNewelP;
    [SerializeField] private Transform scoreBoardT;
    [SerializeField] private Transform oldBoardT;
    [SerializeField] private StairStep oldStairStep;
    [SerializeField] private StairNewel oldStairNewel;

    [Header("Old Score")]
    [SerializeField] private Transform oldScoreBoardT;

    [SerializeField] private StairNewel oldScoreNewel;
    private Vector3 baseStairRotation;

    [Header("Vectors")]
    [SerializeField] private Vector3 nextStairRotation;

    [SerializeField] private Vector3 nextStairPosition;
    [SerializeField] private Vector3 nextStairNewelPos;
    public Vector3 BaseStairRotation { get => baseStairRotation; set => baseStairRotation = value; }

    private void Awake()
    {
        instance = this;
        BaseStairRotation = nextStairRotation;
    }

    public StairStep SpawnStair()
    {
        Vector3 spawnPos = oldStairStep.transform.position + nextStairPosition;
        Transform stairStep = Instantiate(stairStepP, spawnPos, Quaternion.identity);
        stairStep.SetParent(transform);
        stairStep.Rotate(nextStairRotation);
        nextStairRotation += BaseStairRotation;
        oldStairStep = stairStep.GetComponent<StairStep>();
        SpawnStairNewel();
        return oldStairStep;
    }

    private void SpawnStairNewel()
    {
        Vector3 spawnPos = oldStairNewel.transform.position + nextStairNewelPos;
        Transform newelT = Instantiate(stairNewelP, spawnPos, Quaternion.identity);
        newelT.SetParent(transform);
        oldStairNewel = newelT.GetComponent<StairNewel>();
        scoreBoardT.position = oldStairNewel.scorePos.position;
    }

    public void SpawnOldStairNewel()
    {
        Vector3 spawnPos = oldScoreNewel.transform.position + nextStairNewelPos;
        Transform newelT = Instantiate(stairNewelP, spawnPos, Quaternion.identity);
        newelT.SetParent(transform);
        oldScoreNewel = newelT.GetComponent<StairNewel>();
        oldScoreBoardT.position = oldScoreNewel.scorePos.position;
    }
}