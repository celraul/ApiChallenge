namespace Cel.GameOfLife.Application.Extensions;

public static class ListExtenstion
{
    public static bool IsAllDead(this List<List<bool>> currentState)
    {
        foreach (var line in currentState)
            if (line.Any(l => l)) return false;

        return true;
    }

    public static bool AreEqual(this List<List<bool>> a, List<List<bool>> b)
    {
        for (int i = 0; i < a.Count; i++)
        {
            var rowA = a[i];
            var rowB = b[i];

            for (int j = 0; j < rowA.Count; j++)
            {
                if (rowA[j] != rowB[j])
                    return false;
            }
        }

        return true;
    }
}
