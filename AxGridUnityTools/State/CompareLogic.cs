using KellermanSoftware.CompareNetObjects;

namespace AxGrid.State
{
    public static class SmartComparator
    {
        private static CompareLogic _comparator;
        private static CompareLogic Comparator =>
            _comparator ?? (_comparator = new CompareLogic
            {
                Config =
                {
                    CompareProperties = true,
                    MaxDifferences = 1000,
                    ShowBreadcrumb = true
                }
            });

        public static ComparisonResult Compare(object a, object b)
        {
            return Comparator.Compare(a, b);
        }
    }
}