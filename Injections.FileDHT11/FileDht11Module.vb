' Injects implementation into kernel
' Author: Kevin Ross

Imports System.IO
Imports ACRemote.ACRemote
Imports TinyIoC

Namespace Injections.LinuxFileDht11
    Public Class FileDht11Module
        Implements ILoadable
        Public Function Enable() As Boolean Implements ILoadable.Enable
            Dim dht22type As Type = AppDomainMethods.[GetType]("Raspberry.IO.Components.Sensors.Temperature.Dht.Dht22Connection")
            Dim fallback As Boolean = File.Exists(ACRemote.FileDHT11.base_path + Path.DirectorySeparatorChar + ACRemote.FileDHT11.temp_part)
            Return dht22type IsNot Nothing OrElse fallback
        End Function
        Public Sub Load(kernel As TinyIoCContainer) Implements ILoadable.Load
            kernel.Register(Of IDHT11, ACRemote.FileDHT11)().AsSingleton()
        End Sub
        Public Function ImplementingClass() As Type Implements ILoadable.ImplementingClass
            Return GetType(ACRemote.FileDHT11)
        End Function
    End Class
End Namespace

