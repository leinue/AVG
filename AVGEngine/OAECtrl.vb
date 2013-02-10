Public Class OAECtrl
    Dim ScriptFilePath As String = Application.StartupPath + "script\main.ini"
    Dim ScriptI As New OAEScriptEngine
    Dim InitInfo As OAEInitInfo
    Dim MusicPlayer As System.Media.SoundPlayer
    Dim GameForm As Form
    Dim DisplayedItems() As Graphics

    Public Sub Init(ByVal ScriptFile As String, ByVal gForm As Form)
        GameForm = gForm
        ScriptI = New OAEScriptEngine(ScriptFilePath)
        InitInfo = ScriptI.GetInitInfo()
        If InitInfo.width > 0 Then
            GameForm.Width = InitInfo.width
        End If
        If InitInfo.height > 0 Then
            GameForm.Height = InitInfo.height
        End If

        ShowWindow("Main")

    End Sub

    Sub ShowWindow(ByVal WindowName As String)
        Dim Window As OAEWindow = ScriptI.GetWindow(WindowName)
        Dim ItemList As String()
        If Window.bgMusic <> "" Then
            MusicPlayer.SoundLocation = Window.bgMusic
            MusicPlayer.PlayLooping()
        End If
        If Window.bgImage <> "" Then
            GameForm.BackgroundImage = System.Drawing.Image.FromFile(Window.bgImage)
        End If
        If Window.itemList <> "" Then
            ItemList = Window.itemList.Split(",")

            For i As Integer = 0 To UBound(ItemList)
                ShowItem(ItemList(i))
            Next

        End If
    End Sub

    Sub ShowItem(ByVal ItemName As String)
        Dim Item As OAEItem
        Item = ScriptI.GetItem(ItemName)


    End Sub

End Class