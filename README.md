# Counter
### This is a Unity script named CounterBase that provides a basic calculator functionality. It allows users to input mathematical expressions and calculates the result.

## Features
Addition, subtraction, multiplication, and division operations are supported.
Input expressions are displayed in a textmeshpro field.
The result of the calculation is displayed.
Clear button to reset the input and result.
Etc.

## Dependencies
TextMeshPro (TMP): TMP is used for displaying text in Unity.
C# System.Globalization: It is to compute the expressions.

## Usage
Attach the CounterBase script to a GameObject in a Unity scene.
In the Inspector, assign the TextMeshPro (TMP) Text components to the result_text and display_text fields of the script.
Implement UI buttons for each digit (0-9), operators (+, -, *, /), clear, and calculate.
Create button click events and connect them to the appropriate functions in the CounterBase script.

## Methods
OnButtonClick(string value): Call this method when a digit or operator button is clicked. It appends the clicked value to the input display.
OnEscape(): Call this method when the escape or delete button is clicked. It removes the last character from the input display.
OnCalculate(): Call this method when the calculate button is clicked. It evaluates the input expression and displays the result.
OnClear(): Call this method when the clear button is clicked. It clears the input and result displays.

## Description
Here's a simple calculator UI using Unity.

## License
This script is provided under the MIT License.

## Author
This script was created by 832102211汪郑贤.
