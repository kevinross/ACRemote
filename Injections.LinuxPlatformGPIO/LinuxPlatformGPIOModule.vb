' Injects implementation into kernel
' Author: Kevin Ross

Imports System.Reflection
Imports ACRemote.ACRemote

Imports TinyIoC

Namespace Injections.LinuxPlatformGPIO
    Public Class LinuxPlatformGPIOModule
        Implements ILoadable
        Public Function Enable() As Boolean Implements ILoadable.Enable
            ' dynamically pull in the mono dll and syscall functions to see if using mono on linux platform
            ' not adding Mono.Posix to references so it can be compiled on windows in VS
            Try
                ' Don't want to depend on pubkey that may change which Load requires so use deprecated partial name function
#Disable Warning
                Dim mono As Assembly = Assembly.LoadWithPartialName("Mono.Posix")
                Dim syscall As Type = (From t In mono.GetTypes() Where t.FullName.Contains("Syscall")).First()
                Dim utsname As Type = (From t In mono.GetTypes() Where t.FullName.Contains("Utsname")).First()
                ' calling method via Invoke() that has out param: http://stackoverflow.com/a/2438069
                Dim results As Object = Nothing
                Dim out_params As Object() = New Object() {results}
                Dim res As Object = syscall.GetMethod("uname").Invoke(Nothing, out_params)
                results = out_params(0)

                If DirectCast(res, Integer) <> 0 Then
                    Throw New Exception("Syscall failed!")
                End If
                Dim field As FieldInfo = utsname.GetField("sysname")
                ' if on mono but not linux, uname.sysname would be Darwin
                Return DirectCast(field.GetValue(results), String) = "Linux"
            Catch
                Return False
            End Try
        End Function

        Public Sub Load(kernel As TinyIoCContainer) Implements ILoadable.Load
            kernel.Register(Of IPlatformGPIO, ACRemote.LinuxGPIO)().AsSingleton()
        End Sub

        Public Function ImplementingClass() As Type Implements ILoadable.ImplementingClass
            Return GetType(ACRemote.LinuxGPIO)
        End Function
    End Class
End Namespace

