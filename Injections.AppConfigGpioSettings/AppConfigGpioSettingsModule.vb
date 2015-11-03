' Injects implementation into kernel
' Author: Kevin Ross

Imports ACRemote.ACRemote
Imports TinyIoC

Namespace Injections.AppConfigGpioSettings
    Public Class AppConfigGpioSettingsModule
        Implements ILoadable
        Public Function Enable() As Boolean Implements ILoadable.Enable
            Try
                AppSettings.Get(Of Integer)("reset_pin")
                Return True
            Catch
            End Try
            Return False
        End Function
        Public Sub Load(kernel As TinyIoCContainer) Implements ILoadable.Load
            kernel.Register(Of IRemoteGpioSettings, AppConfigGpioSettings)().AsSingleton()
        End Sub
        Public Function ImplementingClass() As Type Implements ILoadable.ImplementingClass
            Return GetType(AppConfigGpioSettings)
        End Function
    End Class
End Namespace

