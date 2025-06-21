namespace APAC.Core;

public class PrefetchEnvironment
{
    private readonly int stateCount;
    private readonly int actionCount;
    private readonly Random random = new();

    public PrefetchEnvironment(int stateCount, int actionCount)
    {
        this.stateCount = stateCount;
        this.actionCount = actionCount;
    }

    public (int nextState, double reward) Step(int state, int action)
    {
        double prefetchAccuracy = random.NextDouble();
        double timeliness = random.NextDouble();
        double cachePollution = random.NextDouble();

        double reward = (prefetchAccuracy * 0.5 + timeliness * 0.4 - cachePollution * 0.9);

        int nextState = random.Next(stateCount);
        return (nextState, reward);
    }
}