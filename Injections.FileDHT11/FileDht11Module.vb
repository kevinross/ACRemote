' Injects implementation into kernel
' Author: Kevin Ross

Imports System.IO
Imports ACRemote
Imports TinyIoC

Public Class FileDht11Module
    Implements ILoadable
    Public Function Enable() As Boolean Implements ILoadable.Enable
        Dim dht22type As Type = AppDomainMethods.[GetType]("Raspberry.IO.Components.Sensors.Temperature.Dht.Dht22Connection")
        Dim fallback As Boolean = File.Exists(FileDHT11.base_path + Path.DirectorySeparatorChar + FileDHT11.temp_part)
        Return dht22type IsNot Nothing OrElse fallback
    End Function
    Public Sub Load(kernel As TinyIoCContainer) Implements ILoadable.Load
        kernel.Register(Of IDHT11, FileDHT11)().AsSingleton()
    End Sub
    Public Function ImplementingClass() As Type Implements ILoadable.ImplementingClass
        Return GetType(FileDHT11)
    End Function
End Class