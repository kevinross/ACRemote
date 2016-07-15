' Interface for loading modules
' Author: Kevin Ross

Imports TinyIoC

Public Interface ILoadable
    ' should the class be used?
    Function Enable() As Boolean
    ' the class that implements the loadable module
    Function ImplementingClass() As Type
    ' load the module into the kernel
    Sub Load(kernel As TinyIoCContainer)
End Interface