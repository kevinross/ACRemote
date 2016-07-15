' Interface for using GPIO pins
' Author: Kevin Ross

Public Enum direction
    in_
    out_
End Enum
Public Interface IPlatformGPIO
    ' setup a pin
    Sub setup(pin As Integer, dir As direction)
    ' get or set the value of a pin
    Default Property Item(val As Integer) As Boolean
End Interface