// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuzzySearch.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Models
{
    #region Imports

    using FuzzySharp;

    using MobileApp.Interfaces;

    #endregion

    public class FuzzySearch<T> : IFuzzySearch<T> where T : INamed
    {
        #region Constants and Private Fields

        private const int ThresholdScore = 80;

        private readonly Dictionary<string, List<T>> _invertedIndex = new();

        #endregion

        #region Public Methods and Operators

        public IEnumerable<T> Search(string searchTerm)
        {
            var candidates = new HashSet<T>();
            foreach (var nGram in GenerateNGrams(searchTerm, 3))
            {
                if (!_invertedIndex.TryGetValue(nGram, out var items))
                {
                    continue;
                }

                foreach (var item in items)
                {
                    candidates.Add(item);
                }
            }

            var matchedItems = candidates.Select(candidate => new
                {
                    Item = candidate,
                    Score = Fuzz.PartialRatio(searchTerm, candidate.Name)
                })
                .Where(x => x.Score > ThresholdScore)
                .OrderByDescending(x => x.Score)
                .Select(x => x.Item);

            return matchedItems;
        }

        public void AddItem(T item)
        {
            foreach (var nGram in GenerateNGrams(item.Name, 3))
            {
                if (!_invertedIndex.ContainsKey(nGram))
                {
                    _invertedIndex[nGram] = new List<T>();
                }

                _invertedIndex[nGram].Add(item);
            }
        }

        #endregion

        #region Private Methods

        private IEnumerable<string> GenerateNGrams(string input, int n)
        {
            for (int i = 0; i < input.Length - n + 1; i++)
            {
                yield return input.Substring(i, n);
            }
        }

        #endregion
    }
}