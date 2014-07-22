using System;
using System.Collections.Generic;
using System.Linq;

namespace KitchIn.Core.Services.TextMatching
{
    /// <summary>
    /// Algorithm Levenshtein distance
    /// </summary>
    public class LevenshteinDistance
    {
        private readonly int maxDistance;
        private readonly string searchText;

        public LevenshteinDistance(string searchText, int percCorrect)
        {
            this.maxDistance = searchText.Length - (int)(searchText.Length * percCorrect / 100);
            this.searchText = searchText.ToLower();
        }

        ///<summary>
        ///Levenshtein distance is an algorithm to measure the similarity between two strings. 
        ///The distance is how different two strings are, where distance = 0 means you are comparing the same strings.
        ///Instead of calculating the exact distance between two strings, we instead check if two words are similar 
        ///to a certain given percentage (we will use 70% in our example). When iterating over each character in the 
        ///two strings we will stop if it turns out that the strings are more than 70% different; we don’t have a match. 
        ///This way checking becomes a little bit faster than having to compute the exact
        /// </summary>
        public bool PercentageCheck(string checkText)
        {
            checkText = checkText.ToLower();
            int nDiagonal = searchText.Length - System.Math.Min(searchText.Length, checkText.Length);
            int mDiagonal = checkText.Length - System.Math.Min(searchText.Length, checkText.Length);
 
            if (searchText.Length == 0) return checkText.Length <= maxDistance;
            if (checkText.Length == 0) return searchText.Length <= maxDistance;
 
            int[,] matrix = new int[searchText.Length + 1, checkText.Length + 1];
 
            for (int i = 0; i <= searchText.Length; matrix[i, 0] = i++);
            for (int j = 0; j <= checkText.Length; matrix[0, j] = j++);
 
            int cost;
 
            for (int i = 1; i <= searchText.Length;i++)
            {
                for (int j = 1; j <= checkText.Length;j++)
                {
                    if (checkText.Substring(j - 1, 1) == searchText.Substring(i - 1, 1))
                    {
                        cost = 0;
                    }
                    else {
                        cost = 1;
                    }
 
                    int valueAbove = matrix[i - 1, j];
                    int valueLeft = matrix[i, j - 1] + 1;
                    int valueAboveLeft = matrix[i - 1, j - 1];
                    matrix[i, j] = Min(valueAbove + 1, valueLeft + 1, valueAboveLeft + cost);
                }
 
                if (i >= nDiagonal)
                {
                    if (matrix[nDiagonal, mDiagonal] > maxDistance)
                    {
                        return false;
                    }
                    else
                    {
                        nDiagonal++;
                        mDiagonal++;
                    }
                }
            }
 
            return true;
        }
 
        private int Min(int n1, int n2, int n3)
        {
            return System.Math.Min(n1, System.Math.Min(n2, n3));
        }    

        ///<summary>
        /// Classic Levenshtein algorithm to select the most similar string
        /// </summary>
        public long GetStringWithMinimumDistanceLevenshtein(List<KeyValuePair<string, long>> candidates)
        {
            long result;
            if (candidates.Count == 1)
            {
                return candidates.FirstOrDefault().Value;
            }

            var minDistanceLevenshtein = ComputeLevenshteinDistance(searchText, candidates[0].Key);
            result = candidates[0].Value;
            for (int i = 1; i < candidates.Count; i++)
            {
                var tmp = ComputeLevenshteinDistance(searchText, candidates[i].Key);
                if (tmp < minDistanceLevenshtein)
                {
                    minDistanceLevenshtein = tmp;
                    result = candidates[i].Value;
                }
            }
            return result;
        }

        private int ComputeLevenshteinDistance(string s, string t)
        {
            s = s.ToLower();
            t = t.ToLower();
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }
            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }
            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }
     }
}
