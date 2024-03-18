namespace Wpf_Calculator
{
    /// <summary>
    /// Contains Constants for applications
    /// </summary>
    class ConstantsForCalculation
    {
        public const int MaxInputLength = 9;
        public const int MaxHistoryLength = 33;
        public const double MaxResult = 999999999;
        public static readonly string ResultIsLargerMessage = "Rsesult is larger then " + MaxInputLength + " digit";
        public const string CanNotDivideByZeroMessage = "Cannot divide by zero";
        public const string ResultIsUndefinedMessage = "Result is undefined";
    }
}
