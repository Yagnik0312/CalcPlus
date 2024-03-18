namespace Wpf_Calculator
{
    /// <summary>
    /// Contains type of operator
    /// </summary>
    enum EnumOperationStatus
    {
        WithoutArg = 0,
        AfterOneArg = 1,
        AfterTwoArg = 2,
        AfterEqual = 3,
        AfterOperator = 4,
    }
}
