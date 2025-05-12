namespace Cel.GameOfLife.Application.Extensions;

public static class ListExtenstion
{
    public static bool IsAllDead(this bool[][] currentState)
    {
        foreach (var line in currentState)
            if (line.Any(l => l)) return false;

        return true;
    }

    public static bool AreEqual(this bool[][] a, bool[][] b)
    {
        for (int i = 0; i < a.Length - 1; i++)
        {
            var rowA = a[i];
            var rowB = b[i];

            for (int j = 0; j < rowA.Length - 1; j++)
            {
                if (rowA[j] != rowB[j])
                    return false;
            }
        }

        return true;
    }
}
