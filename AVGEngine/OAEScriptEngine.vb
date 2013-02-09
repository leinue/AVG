Public Class OAEScriptEngine
    Dim ScriptPath As String
    Dim InitPath As String

    Sub New()
        ' TODO: Complete member initialization 
    End Sub

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
    Public Sub Preproccess() '预处理器

    End Sub
    Sub New(ByVal cpath As String, ByVal ipath As String) '构造函数
        ScriptPath = cpath '脚本目录
        InitPath = ipath '初始化ini目录
    End Sub
    Public Function GetWindow(ByVal WindowSectionName As String) As OAEWindow '获得类型为window的各项属性
        GetWindow.bgImage = GetINI(WindowSectionName, "bgImage", "", ScriptPath)
        GetWindow.bgbgMusic = GetINI(WindowSectionName, "bgMusic", "", ScriptPath)
        GetWindow.itemList = GetINI(WindowSectionName, "itemList", "", ScriptPath)
    End Function
    Public Function GetItem(ByVal ItemSectionName As String) As OAEItem '获得类型为item的各项属性
        GetItem.type = GetINI("item-" + ItemSectionName, "type", "", ScriptPath)
        GetItem.action = GetINI("item-" + ItemSectionName, "action", "", ScriptPath)
        GetItem.height = GetINI("item-" + ItemSectionName, "height", "", ScriptPath)
        GetItem.locX = GetINI("item-" + ItemSectionName, "locX", "", ScriptPath)
        GetItem.locY = GetINI("item-" + ItemSectionName, "locY", "", ScriptPath)
        GetItem.width = GetINI("item-" + ItemSectionName, "width", "", ScriptPath)
    End Function
    Public Function GetInitInfo() As OAEInitInfo
        GetInitInfo.height = GetINI("init", "height", "", InitPath)
        GetInitInfo.width = GetINI("init", "width", "", InitPath)
    End Function
End Class