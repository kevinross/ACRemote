' Interface for using GPIO pins
' Author: Kevin Ross

Namespace ACRemote
    Public Enum direction
        in_
        out_
    End Enum
    Public Interface IPlatformGPIO
        Sub setup(pin As Integer, dir As direction)
        Default Property Item(val As Integer) As Boolean
    End Interface
End Namespace
