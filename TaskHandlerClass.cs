using System;
using System.Windows.Input;
namespace Wpf_Calculator
{
    // <summary>
    /// Utility class that handles input, calculations, and history for a WPF calculator application
    /// </summary>
    class TaskHandlerClass
    {
        
        private static string firstOperand = "0";
        private static string secondOperand = "0";
        private static string operatorForCalculation = String.Empty;
        private static string history;
        private static bool isDecimal = false;
        private static bool isErrorOccure = false;
        static EnumOperationStatus operationStatus = EnumOperationStatus.AfterOneArg;

        /// <summary>
        /// Performs arithmetic calculations
        /// </summary>
        /// <param name="operand1">The first operand</param>
        /// <param name="operand2">The second operand</param>
        /// <returns>The result of the calculation as a string</returns>
        private static void CalculateValue(string operand1, string operand2)
        {
            double operandOne = double.Parse(operand1);
            double operandTwo = double.Parse(operand2);
            double result = 0;
            switch (operatorForCalculation)
            {
                case "+":
                    result = operandOne + operandTwo;
                    break;
                case "-":
                    result = operandOne - operandTwo;
                    break;
                case "*":
                    result = operandOne * operandTwo;
                    break;
                case "/":
                    if (operandOne == 0 && operandTwo == 0)
                    {
                        ClearAllDataHandler();
                        operationStatus = EnumOperationStatus.WithoutArg;
                        firstOperand = ConstantsForCalculation.ResultIsUndefinedMessage;
                        isErrorOccure = true;
                    }
                    else if (operandTwo == 0)
                    {
                        ClearAllDataHandler();
                        operationStatus = EnumOperationStatus.WithoutArg;
                        firstOperand = ConstantsForCalculation.CanNotDivideByZeroMessage;
                        isErrorOccure = true;
                    }
                    else
                    {
                        result = operandOne / operandTwo;
                    }
                    break;
            }

            if (!isErrorOccure)
            {
                validateResult(result);
            }
        }

        /// <summary>
        /// Validate result size according to constrainss
        /// </summary>
        /// <param name="result">Result of Calculation</param>
        private static void validateResult(double result)
        {
            int indexOFDecimal = result.ToString().IndexOf('.');
            if (result < ConstantsForCalculation.MaxResult  && indexOFDecimal != -1)
            {
                firstOperand = Math.Round(result, ConstantsForCalculation.MaxInputLength - indexOFDecimal).ToString();
            }
            else if (result < ConstantsForCalculation.MaxResult)
            {
                firstOperand = result.ToString();
            }
            else
            {
                ClearAllDataHandler();
                operationStatus = EnumOperationStatus.WithoutArg;
                firstOperand = ConstantsForCalculation.ResultIsLargerMessage;
                isErrorOccure = true;
            }
        }

        /// <summary>
        /// Set the history according to given size
        /// </summary>
        private static void HistoryLengthValidate()
        {
            while (history.Length >= ConstantsForCalculation.MaxHistoryLength)
            {
                int iterator = 1;
                while (true)
                {
                    if (history[iterator] == '+' || history[iterator] == '-' || history[iterator] == '/' || history[iterator] == '*')
                    {
                        history = history.Substring(iterator+1);
                        break;
                    }
                    else
                    {
                        iterator++;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the input value
        /// </summary>
        /// <param name="input">The input value</param>
        private static string InputValueSetter(string input, string operand)
        {
            int maxLength = ConstantsForCalculation.MaxInputLength;
            //Change maximum length if value is decimal
            if (isDecimal)
            {
                maxLength = ConstantsForCalculation.MaxInputLength + 1;
            }

            //Take input based on maximum length constrains
            if (operand.Length < maxLength)
            {
                if (operand == "0" && input != ".")
                {
                    operand = input;
                }
                else if (input == "." && !isDecimal)
                {
                    operand = operand + input;
                    isDecimal = true;
                }
                else if (input != ".")
                {
                    operand = operand + input;
                }
            }
            return operand;
        }

        /// <summary>
        /// Handles user input from buttons
        /// </summary>
        /// <param name="value">The input value</param>
        public static void UserNumberInputHandler(string value)
        {
            if (isErrorOccure)
            {
                isErrorOccure = false;
                firstOperand = "0";
            }

            if (operationStatus == EnumOperationStatus.AfterOperator)
            {
                operationStatus = EnumOperationStatus.AfterTwoArg;
            }
            else if (operationStatus == EnumOperationStatus.AfterEqual || operationStatus == EnumOperationStatus.WithoutArg)
            {
                history = string.Empty;
                firstOperand = "0";
                secondOperand = "0";
                operationStatus = EnumOperationStatus.AfterOneArg;
            }

            if (operationStatus == EnumOperationStatus.AfterOneArg)
            {
                firstOperand = InputValueSetter(value, firstOperand);
            }
            else
            {
                secondOperand = InputValueSetter(value, secondOperand);
            }
        }

        /// <summary>
        /// Handles keyboard input
        /// </summary>
        /// <param name="value">The input value</param>
        public static void KeyboardInput(Key value)
        {
            switch (value)
            {
                case Key.Divide:
                case Key.OemQuestion:
                    OperatorHandler("/");
                    break;
                case Key.Multiply:
                    OperatorHandler("*");
                    break;
                case Key.Add:
                    OperatorHandler("+");
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    OperatorHandler("-");
                    break;
                case Key.Enter:
                case Key.OemPlus:
                    CalculateHandler();
                    break;
                case Key.Decimal:
                case Key.OemPeriod:
                    UserNumberInputHandler(".");
                    break;
                case Key.Delete:
                case Key.Escape:
                    ClearAllDataHandler();
                    break;
                case Key.Back:
                    BackSpaceHandler();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles operators (+, -, *, /)
        /// </summary>
        /// <param name="operatorValue">The operator value</param>
        public static void OperatorHandler(string operatorValue)
        {
            switch (operationStatus)
            {
                case EnumOperationStatus.AfterOneArg:
                    operatorForCalculation = operatorValue;
                    history = Double.Parse(firstOperand) + operatorForCalculation;
                    isDecimal = false;
                    operationStatus = EnumOperationStatus.AfterOperator;
                    break;
                case EnumOperationStatus.AfterOperator:
                    operatorForCalculation = operatorValue;
                    history = history.Substring(0, history.Length - 1) + operatorForCalculation;
                    break;
                case EnumOperationStatus.AfterTwoArg:
                    CalculateValue(firstOperand, secondOperand);
                    operatorForCalculation = operatorValue;
                    history += Double.Parse(secondOperand) + operatorForCalculation;
                    if (isErrorOccure) { break; };
                    secondOperand = "0";
                    isDecimal = false;
                    operationStatus = EnumOperationStatus.AfterOperator;
                    break;
                case EnumOperationStatus.AfterEqual:
                    operatorForCalculation = operatorValue;
                    secondOperand = "0";
                    history = Double.Parse(firstOperand) + operatorForCalculation;
                    operationStatus = EnumOperationStatus.AfterOperator;
                    break;
            }
        }

        /// <summary>
        /// Clears all data
        /// </summary>
        public static void ClearAllDataHandler()
        {
            firstOperand = "0";
            secondOperand = "0";
            history = String.Empty;
            isDecimal = false;
            operationStatus = EnumOperationStatus.AfterOneArg;
            operatorForCalculation = String.Empty;
        }

        /// <summary>
        /// Performs calculation
        /// </summary>
        public static void CalculateHandler()
        {
            if (operationStatus == EnumOperationStatus.AfterTwoArg)
            {
                operationStatus = EnumOperationStatus.AfterEqual;
                history = string.Empty;
                CalculateValue(firstOperand, secondOperand);
                isDecimal = false;
            }
            else if (operationStatus == EnumOperationStatus.AfterEqual)
            {
                operationStatus = EnumOperationStatus.AfterEqual;
                CalculateValue(firstOperand, secondOperand);
                history = string.Empty;
                isDecimal = false;
            }
            else if (operationStatus == EnumOperationStatus.AfterOperator && !isErrorOccure)
            {
                secondOperand = firstOperand;
                operationStatus = EnumOperationStatus.AfterEqual;
                CalculateValue(firstOperand, secondOperand);
                history = string.Empty;
                isDecimal = false;
            }
        }
        
        /// <summary>
        /// Handles backspace operation
        /// </summary>
        public static void BackSpaceHandler()
        {
            if (operationStatus == EnumOperationStatus.AfterEqual)
            {
                ClearAllDataHandler();
            }
            //Handle backspace for first operand
            if (operationStatus == EnumOperationStatus.AfterOneArg)
            {
                if (firstOperand.Length > 0)
                {
                    if (firstOperand.EndsWith("."))
                    {
                        isDecimal = false;
                    }
                    firstOperand = firstOperand.Substring(0, firstOperand.Length - 1);
                    if (firstOperand.Length == 0)
                    {
                        firstOperand = "0";
                    }
                }
            }
            //Handle backspace for second operand
            else
            {
                if (secondOperand.EndsWith("."))
                {
                    isDecimal = false;
                }
                secondOperand = secondOperand.Substring(0, secondOperand.Length - 1);
                if (secondOperand.Length == 0)
                {
                    secondOperand = "0";
                }
            }
        }

        // <summary>
        /// Gets the current display data
        /// </summary>
        /// <returns>The display data as a string</returns>
        public static string DisplayData()
        {
            if (operationStatus == EnumOperationStatus.AfterTwoArg)
            {
                return secondOperand;
            }
            else
            {
                return firstOperand;
            }
        }

        /// <summary>
        /// Gets the display history
        /// </summary>
        public static string DisplayHistory()
        {
            if (operationStatus == EnumOperationStatus.WithoutArg || operationStatus == EnumOperationStatus.AfterOneArg)
            {
                return String.Empty;
            }
            else
            {
                HistoryLengthValidate();
                return history;
            }
        }
    }
}
