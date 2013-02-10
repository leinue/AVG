Public Class OAECtrl
    Structure InDisplayItem
        Dim Item As OAEItem
        Dim ItemStatus As String
        Dim LastDrawStatus As String
    End Structure
    Structure ImageInfo
        Dim Image As Image
        Dim ID As String
    End Structure

    Dim ScriptFilePath As String = Application.StartupPath + "script\main.ini"
    Dim ScriptI As OAEScriptEngine
    Dim InitInfo As OAEInitInfo
    Dim MusicPlayer As System.Media.SoundPlayer
    Dim gForm As Form
    Dim DisplayedItems() As Graphics
    Dim ItemList() As InDisplayItem
    Dim imageList() As ImageInfo
    Dim g As Graphics

    Public Sub Init(ByVal ScriptFile As String, ByVal GameForm As Form)
        gForm = GameForm

        AddHandler gForm.FormClosing, AddressOf Destory

        ScriptI = New OAEScriptEngine(ScriptFilePath)
        InitInfo = ScriptI.GetInitInfo()
        If InitInfo.width > 0 Then
            gForm.Width = InitInfo.width
        End If
        If InitInfo.height > 0 Then
            gForm.Height = InitInfo.height
        End If

        g = Graphics.FromHwnd(gForm.Handle)

        ShowWindow("Main")

    End Sub

    Sub ShowWindow(ByVal WindowName As String)
        Dim Window As OAEWindow = ScriptI.GetWindow(WindowName)
        Dim ItemNameList As String()
        If Window.bgMusic <> "" Then
            MusicPlayer.SoundLocation = Window.bgMusic
            MusicPlayer.PlayLooping()
        End If
        If Window.bgImage <> "" Then
            GameForm.BackgroundImage = System.Drawing.Image.FromFile(Window.bgImage)
        End If
        If Window.itemList <> "" Then
            ItemNameList = Window.itemList.Split(",")

            ReDim ItemList(UBound(ItemNameList))

            For i As Integer = 0 To UBound(ItemList)
                ItemList(i).Item = ScriptI.GetItem(ItemNameList(i))
                ItemList(i).ItemStatus = "Normal"
                ItemList(i).LastDrawStatus = ""
                DrawItem(ItemList(i))
            Next

        End If
    End Sub

    Sub DrawItem(ByVal Item As InDisplayItem)
        Dim image As Image = GetItemImage(Item)

        g.DrawImage(image, Item.Item.locX, Item.Item.locY, Item.Item.locX + image.Width, Item.Item.locY + image.Width)

    End Sub

    Function GetItemImage(ByVal Item As InDisplayItem) As Image
        For i As Integer = 0 To UBound(imageList)
            imageList(i).ID = Item.Item.name + "-" + Item.ItemStatus
            Return imageList(i).Image
        Next

        If UBound(imageList) - LBound(imageList) = 0 Then
            ReDim Preserve imageList(2 * UBound(imageList))
        End If

        If Item.ItemStatus = "Normal" Then
            imageList(LBound(imageList)) = ScriptI.getImageRes(Item.Item.normalImage)
            Return imageList(LBound(imageList)).Image
        ElseIf Item.ItemStatus = "Hover" Then
            imageList(LBound(imageList)) = ScriptI.getImageRes(Item.Item.hoverImage)
            Return imageList(LBound(imageList)).Image
        ElseIf Item.ItemStatus = "Click" Then
            imageList(LBound(imageList)) = ScriptI.getImageRes(Item.Item.clickImage)
            Return imageList(LBound(imageList)).Image
        End If

        Throw New Exception("Unknow event type : " + Item.ItemStatus)
        Return Nothing
    End Function

    Sub Destory()
        g.Dispose()
        For i As Integer = 0 To UBound(imageList)
            imageList(i).Image.Dispose()
        Next

    End Sub

End Class
