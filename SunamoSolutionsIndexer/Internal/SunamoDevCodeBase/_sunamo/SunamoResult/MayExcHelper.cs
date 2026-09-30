namespace SunamoDevCode._sunamo.SunamoResult;

internal class MayExcHelper
{
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