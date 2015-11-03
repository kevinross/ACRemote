' Interface for loading modules
' Author: Kevin Ross

Imports TinyIoC

Namespace ACRemote
  Public Interface ILoadable
    Function Enable() As Boolean
    Function ImplementingClass() As Type
    Sub Load(kernel As TinyIoCContainer)
  End Interface
End Namespace

