public class DerangementCalculator
{
    public long Calculate(int n)
    {
        if (n < 0) throw new ArgumentException("Input must be non-negative.");
        if (n == 0) return 1;
        if (n == 1) return 0;

        long[] dp = new long[n + 1];
        dp[0] = 1;
        dp[1] = 0;

        for (int i = 2; i <= n; i++)
        {
            dp[i] = (i - 1) * (dp[i - 1] + dp[i - 2]);
        }

        return dp[n];
    }
}
