Public Class OAEScriptEngine
    Dim ScriptPath As String
<<<<<<< HEAD:AVGEngine/iniio.vb
=======

>>>>>>> f8ce43f24ce004cc09422e36e3f12fb334add52c:AVGEngine/OAEScriptEngine.vb
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
        ScriptPath = cpath
    End Sub
<<<<<<< HEAD:AVGEngine/iniio.vb
    Public Function GetWindow(ByVal WindowSectionName As String) As OAEWindow '获得类型为window的各项属性
        GetWindow.bgImage = GetINI(WindowSectionName, "bgImage", "", ScriptPath)
        GetWindow.bgbgMusic = GetINI(WindowSectionName, "bgMusic", "", ScriptPath)
        GetWindow.itemList = GetINI(WindowSectionName, "itemList", "", ScriptPath)
    End Function
    Public Function GetItem(ByVal ItemSectionName As String) As OAEItem
        GetItem.type = GetINI(ItemSectionName, "type", "", ScriptPath)
        GetItem.action = GetINI(ItemSectionName, "action", "", ScriptPath)
        GetItem.height = GetINI(ItemSectionName, "height", "", ScriptPath)
        GetItem.locX = GetINI(ItemSectionName, "locX", "", ScriptPath)
        GetItem.locY = GetINI(ItemSectionName, "locY", "", ScriptPath)
        GetItem.width = GetINI(ItemSectionName, "width", "", ScriptPath)
>>>>>>> f8ce43f24ce004cc09422e36e3f12fb334add52c:AVGEngine/OAEScriptEngine.vb
    End Function
End Class