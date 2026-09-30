namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// Helper for exceptions that may be thrown.
/// </summary>
internal class MayExcHelper
{
    /// <summary>
    /// Determines whether the exception message should be thrown.
    /// </summary>
    internal static bool MayExc(string exception)
    {
        if (exception is not null)
        {
            Console.WriteLine(exception);
            //ThisApp.Error( result.exception);
            return true;
        }

        return false;
    }
}