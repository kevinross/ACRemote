' Injects implementation into kernel
' Author: Kevin Ross

Imports ACRemote
Imports TinyIoC

Public Class MQTTNotifyModule
    Implements ILoadable
    Public Function Enable() As Boolean Implements ILoadable.Enable
        Return AppDomainMethods.GetType("uPLibrary.Networking.M2Mqtt.MqttClient") IsNot Nothing
    End Function
    Public Sub Load(kernel As TinyIoCContainer) Implements ILoadable.Load
        kernel.Register(Of INotify, MQTTNotify)().AsSingleton()
    End Sub
    Public Function ImplementingClass() As Type Implements ILoadable.ImplementingClass
        Return GetType(MQTTNotify)
    End Function
End Class