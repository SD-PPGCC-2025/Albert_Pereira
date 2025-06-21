namespace APAC.Core;

public class APACAgent
{
    private readonly double[,] qTable;
    private readonly int stateCount;
    private readonly int actionCount;
    private readonly double alpha;
    private readonly double gamma;
    private readonly double epsilon;
    private readonly Random random = new();

    public APACAgent(int stateCount, int actionCount, double alpha = 0.1, double gamma = 0.9, double epsilon = 0.1)
    {
        this.stateCount = stateCount;
        this.actionCount = actionCount;
        this.alpha = alpha;
        this.gamma = gamma;
        this.epsilon = epsilon;
        qTable = new double[stateCount, actionCount];
    }

    public int ChooseAction(int state)
    {
        if (random.NextDouble() < epsilon)
            return random.Next(actionCount);
        double maxQ = double.MinValue;
        int bestAction = 0;
        for (int a = 0; a < actionCount; a++)
        {
            if (qTable[state, a] > maxQ)
            {
                maxQ = qTable[state, a];
                bestAction = a;
            }
        }
        return bestAction;
    }

    public void Update(int state, int action, double reward, int nextState, int nextAction)
    {
        double predict = qTable[state, action];
        double target = reward + gamma * qTable[nextState, nextAction];
        qTable[state, action] += alpha * (target - predict);
    }

    public double[,] GetQTable() => qTable;
}