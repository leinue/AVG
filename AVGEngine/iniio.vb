Public Class OAEScriptEngine
    Dim path As String
    'GetAttr("item","item1","locX")
    Structure item
        Dim type As String
        Dim locX As Integer
        Dim locY As Integer
        Dim height As Integer
        Dim width As Integer
        Dim action As String
    End Structure
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Int32, ByVal lpFileName As String) As Int32
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Int32
    '定义读取配置文件函数
    Public Function GetINI(ByVal Section As String, ByVal AppName As String, ByVal lpDefault As String, ByVal FileName As String) As String
        Dim Str As String = LSet(Str, 256)
        GetPrivateProfileString(Section, AppName, lpDefault, Str, Len(Str), FileName)
        Return Microsoft.VisualBasic.Left(Str, InStr(Str, Chr(0)) - 1)
    End Function
    '定义写入配置文件函数
    Public Function WriteINI(ByVal Section As String, ByVal AppName As String, ByVal lpDefault As String, ByVal FileName As String) As Long
        WriteINI = WritePrivateProfileString(Section, AppName, lpDefault, FileName)
    End Function
    Public Sub init_main(ByVal cpath As String) '初始化main.ini文件的目录
        path = cpath
    End Sub
    Public Function GetAttr(ByVal itemName As String, ByVal itemSection As String, ByVal itemDefault As String)
        'GetINI(itemName + itemSection, itemDefault, , path)
    End Function
End Class