' Provides magic to register null modules
' Author: Kevin Ross

Imports ACRemote
Imports TinyIoC

' inherit from this to implement a null module's registration class
' no other work needed, just have an empty class body
Public MustInherit Class NullModules(Of I, T As {I, Defaultable})
    Implements ILoadable
    Public Function Enable() As Boolean Implements ILoadable.Enable
        Return Not Defaultable.non_default_exists(Of I)(GetType(T))
    End Function
    Public Sub Load(kern As TinyIoCContainer) Implements ILoadable.Load
        kern.Register(GetType(I), GetType(T)).AsSingleton()
    End Sub
    Public Function ImplementingClass() As Type Implements ILoadable.ImplementingClass
        Return GetType(T)
    End Function
End Class
Public Class NullDhtModule
    Inherits NullModules(Of IDHT11, NullDHT11)

End Class
Public Class NullGpioModule
    Inherits NullModules(Of IPlatformGPIO, NullGPIO)

End Class
Public Class NullGpioSettingsModule
    Inherits NullModules(Of IRemoteGpioSettings, NullGPIOSettings)

End Class
Public Class NullNotifyModule
    Inherits NullModules(Of INotify, NullNotify)

End Class