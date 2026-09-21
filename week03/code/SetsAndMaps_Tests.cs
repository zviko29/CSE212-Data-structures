using Microsoft.VisualStudio.TestTools.UnitTesting;

public class SetsAndMaps_Tests
{
    // Normalizes "ma & am" and "am & ma" to the same form so tests don't
    // depend on the order of words inside each string.
    private static string Normalize(string pair)
    {
        var parts = pair.Split(" & ");
        Array.Sort(parts);
        return string.Join(" & ", parts);
    }

    private static string[] NormalizeAll(string[] pairs)
    {
        var normalized = pairs.Select(Normalize).ToArray();
        Array.Sort(normalized);
        return normalized;
    }

    // Problem 1: FindPairs 

    [TestMethod]
    public void FindPairs_ExampleFromAssignment()
    {
        var result = SetsAndMaps.FindPairs(new[] { "am", "at", "ma", "if", "fi" });
        CollectionAssert.AreEqual(new[] { "am & ma", "fi & if" }, NormalizeAll(result));
    }

    [TestMethod]
    public void FindPairs_NoPairs()
    {
        var result = SetsAndMaps.FindPairs(new[] { "ab", "cd", "ef" });
        Assert.AreEqual(0, result.Length);
    }

    [TestMethod]
    public void FindPairs_SameLettersAreIgnored()
    {
        var result = SetsAndMaps.FindPairs(new[] { "aa", "bb", "ab", "ba" });
        CollectionAssert.AreEqual(new[] { "ab & ba" }, NormalizeAll(result));
    }

    [TestMethod]
    public void FindPairs_EmptyList()
    {
        var result = SetsAndMaps.FindPairs(Array.Empty<string>());
        Assert.AreEqual(0, result.Length);
    }

    [TestMethod]
    public void FindPairs_ReversedOrderInInput()
    {
        var result = SetsAndMaps.FindPairs(new[] { "ma", "am" });
        CollectionAssert.AreEqual(new[] { "am & ma" }, NormalizeAll(result));
    }

    // Problem 2: SummarizeDegrees

    [TestMethod]
    public void SummarizeDegrees_CountsColumnFour()
    {
        
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(path, new[]
            {
                "39, State-gov, 77516, Bachelors, 13",
                "50, Self-emp, 83311, Bachelors, 13",
                "38, Private, 215646, HS-grad, 9",
                "53, Private, 234721, 11th, 7",
                "28, Private, 338409, Masters, 14",
                "37, Private, 284582, Masters, 14",
                "49, Private, 160187, HS-grad, 9",
            });

            var result = SetsAndMaps.SummarizeDegrees(path);

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(2, result["Bachelors"]);
            Assert.AreEqual(2, result["HS-grad"]);
            Assert.AreEqual(2, result["Masters"]);
            Assert.AreEqual(1, result["11th"]);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void SummarizeDegrees_EmptyFile()
    {
        var path = Path.GetTempFileName();
        try
        {
            var result = SetsAndMaps.SummarizeDegrees(path);
            Assert.AreEqual(0, result.Count);
        }
        finally
        {
            File.Delete(path);
        }
    }

    // Optional: runs against the real census.txt if it is next to the test output.
    [TestMethod]
    public void SummarizeDegrees_CensusFile()
    {
        if (!File.Exists("census.txt"))
            Assert.Inconclusive("census.txt not found in the test working directory.");

        var result = SetsAndMaps.SummarizeDegrees("census.txt");
        Assert.IsTrue(result.Count > 0);
        Assert.IsTrue(result.Values.All(v => v > 0));
    }

    // Problem 3: IsAnagram 

    [TestMethod]
    public void IsAnagram_CatAct()
    {
        Assert.IsTrue(SetsAndMaps.IsAnagram("CAT", "ACT"));
    }

    [TestMethod]
    public void IsAnagram_DogGood_False()
    {
        Assert.IsFalse(SetsAndMaps.IsAnagram("DOG", "GOOD"));
    }

    [TestMethod]
    public void IsAnagram_IgnoresCase()
    {
        Assert.IsTrue(SetsAndMaps.IsAnagram("Ab", "bA"));
    }

    [TestMethod]
    public void IsAnagram_IgnoresSpaces()
    {
        Assert.IsTrue(SetsAndMaps.IsAnagram("Eleven plus two", "Twelve plus one"));
    }

    [TestMethod]
    public void IsAnagram_DifferentLetterCounts_False()
    {
        Assert.IsFalse(SetsAndMaps.IsAnagram("aab", "abb"));
    }

    [TestMethod]
    public void IsAnagram_DifferentLengths_False()
    {
        Assert.IsFalse(SetsAndMaps.IsAnagram("abc", "abcd"));
    }

    [TestMethod]
    public void IsAnagram_SameWord()
    {
        Assert.IsTrue(SetsAndMaps.IsAnagram("listen", "listen"));
    }

    [TestMethod]
    public void IsAnagram_ListenSilent()
    {
        Assert.IsTrue(SetsAndMaps.IsAnagram("Listen", "Silent"));
    }

    // ---------- Problem 5: EarthquakeDailySummary ----------
    // This test needs internet access. The data changes daily, so it only
    // checks the format, not specific values.

    [TestMethod]
    public void EarthquakeDailySummary_ReturnsFormattedStrings()
    {
        var result = SetsAndMaps.EarthquakeDailySummary();

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0, "Expected at least one earthquake today.");
        foreach (var line in result)
        {
            StringAssert.Contains(line, " - Mag ");
        }
    }
}
